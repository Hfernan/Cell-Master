using UnityEngine;

// From https://wiki.unity3d.com/index.php/ProceduralPrimitives#C.23_-_Plane

namespace KT
{
  static class PlaneCreator
  {
    /// <summary>
    /// Creates a flat plane.
    /// </summary>
    /// <param name="mesh">Mesh to create the plane on. It is cleared.</param>
    /// <param name="width">Plane width on X axis.</param>
    /// <param name="length">Plane length on Z axis.</param>
    /// <param name="resX">Number of subdivisioons on X axis.</param>
    /// <param name="resZ">Number of subdivisioons on Z axis.</param>
    public static void Create ( Mesh mesh , float width , float length , int resX , int resZ )
    {
      // You can change that line to provide another MeshFilter
      mesh.Clear();

      //int resX = 2; // 2 minimum
      //int resZ = 2;

      resX = Mathf.Max( 2 , resX );
      resZ = Mathf.Max( 2 , resZ );

      #region Vertices		
      Vector3[] vertices = new Vector3[ resX * resZ ];

      for ( int z = 0 ; z < resZ ; ++z )
      {
        // [ -length / 2, length / 2 ]
        float zPos = ( ( float ) z / ( resZ - 1 ) - .5f ) * length;

        for ( int x = 0 ; x < resX ; ++x )
        {
          // [ -width / 2, width / 2 ]
          float xPos = ( ( float ) x / ( resX - 1 ) - .5f ) * width;

          vertices[ x + ( z * resX ) ] = new Vector3( xPos , 0f , zPos );
        }
      }
      #endregion

      #region Normales
      Vector3[] normales = new Vector3[ vertices.Length ];

      for ( int i = 0 , n = normales.Length ; ( i < n ) ; ++i )
      {
        normales[i] = Vector3.up;
      }
      #endregion

      #region UVs		
      Vector2[] uvs = new Vector2[ vertices.Length ];

      for ( int v = 0 ; v < resZ ; ++v )
      {
        for ( int u = 0 ; u < resX ; ++u )
        {
          uvs[u + ( v * resX ) ] = new Vector2( ( float ) u / ( resX - 1 ) , ( float ) v / ( resZ - 1 ) );
        }
      }
      #endregion

      #region Triangles
      int nbFaces = ( resX - 1 ) * ( resZ - 1 );

      int[] triangles = new int[ nbFaces * 6 ];

      int t = 0;

      for ( int face = 0 ; face < nbFaces ; ++face )
      {
        // Retrieve lower left corner from face ind
        int i = ( face % ( resX - 1 ) ) + ( face / ( resZ - 1 ) * resX );

        // Be careful with the winding.
        triangles[t++] = i + resX;
        triangles[t++] = i + 1   ;
        triangles[t++] = i       ;

        triangles[t++] = i + resX    ;
        triangles[t++] = i + resX + 1;
        triangles[t++] = i        + 1;
      }
      #endregion

      mesh.vertices = vertices;
      mesh.normals = normales;
      mesh.uv = uvs;
      mesh.triangles = triangles;

      mesh.RecalculateBounds();
      mesh.Optimize();
    }
  }
}
