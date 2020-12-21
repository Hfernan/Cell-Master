using System.Collections.Generic;
using UnityEngine;

// From http://wiki.unity3d.com/index.php/ProceduralPrimitives?_ga=2.259372879.437086687.1602807994-1006901955.1594258337#C.23_-_IcoSphere

namespace KT
{
  static class IcoSphere
  {
    private struct TriangleIndices
    {
      public int v1;
      public int v2;
      public int v3;

      public TriangleIndices ( int v1 , int v2 , int v3 )
      {
        this.v1 = v1;
        this.v2 = v2;
        this.v3 = v3;
      }
    }

    /// <summary>
    // Returns index of point in the middle of p1 and p2
    /// </summary>
    private static int getMiddlePoint ( int p1 , int p2 , ref List<Vector3> vertices , ref Dictionary<long , int> cache , float radius )
    {
      // Check if we have it already.
      bool firstIsSmaller = p1 < p2;
      long smallerIndex = firstIsSmaller ? p1 : p2;
      long greaterIndex = firstIsSmaller ? p2 : p1;
      long key = (smallerIndex << 32) + greaterIndex;

      if ( cache.TryGetValue( key , out int ret ) ) // Declares ret and uses it inside the if scope.
      {
        return ret;
      }

      // Not in cache, calculate it.
      Vector3 point1 = vertices[p1];
      Vector3 point2 = vertices[p2];

      Vector3 middle = new Vector3
    (
            ( point1.x + point2.x ) / 2f,
            ( point1.y + point2.y ) / 2f,
            ( point1.z + point2.z ) / 2f
    );

      // add vertex makes sure point is on unit sphere
      int i = vertices.Count;

      vertices.Add( middle.normalized * radius );

      // store it, return index
      cache.Add( key , i );

      return i;
    }

    /// <summary>
    /// Creates a icosphere subdividing an icosahedron.
    /// </summary>
    /// <param name="mesh">Mesh to build the icosphere. It is cleared.</param>
    /// <param name="recursionLevel">Number of mesh subdivisions.</param>
    /// <param name="radius">Icosphere radius.</param>
    public static void Create ( Mesh mesh , int recursionLevel = 3 , float radius = 1f )
    {
      mesh.Clear();

      List<Vector3> vertList = new List<Vector3>();

      Dictionary<long, int> middlePointIndexCache = new Dictionary<long, int>();

      // Create 12 vertices of a icosahedron.
      float t = ( 1f + Mathf.Sqrt( 5f ) ) / 2f;

      vertList.Add( new Vector3( -1f ,  t , 0f ).normalized * radius );
      vertList.Add( new Vector3(  1f ,  t , 0f ).normalized * radius );
      vertList.Add( new Vector3( -1f , -t , 0f ).normalized * radius );
      vertList.Add( new Vector3(  1f , -t , 0f ).normalized * radius );

      vertList.Add( new Vector3( 0f , -1f ,  t ).normalized * radius );
      vertList.Add( new Vector3( 0f ,  1f ,  t ).normalized * radius );
      vertList.Add( new Vector3( 0f , -1f , -t ).normalized * radius );
      vertList.Add( new Vector3( 0f ,  1f , -t ).normalized * radius );

      vertList.Add( new Vector3(  t , 0f , -1f ).normalized * radius );
      vertList.Add( new Vector3(  t , 0f ,  1f ).normalized * radius );
      vertList.Add( new Vector3( -t , 0f , -1f ).normalized * radius );
      vertList.Add( new Vector3( -t , 0f ,  1f ).normalized * radius );

      // create 20 triangles of the icosahedron
      List<TriangleIndices> triIdx = new List<TriangleIndices>();

      // 5 faces around point 0
      triIdx.Add( new TriangleIndices( 0 , 11 ,  5 ) );
      triIdx.Add( new TriangleIndices( 0 ,  5 ,  1 ) );
      triIdx.Add( new TriangleIndices( 0 ,  1 ,  7 ) );
      triIdx.Add( new TriangleIndices( 0 ,  7 , 10 ) );
      triIdx.Add( new TriangleIndices( 0 , 10 , 11 ) );

      // 5 adjacent faces 
      triIdx.Add( new TriangleIndices(  1 ,  5 , 9 ) );
      triIdx.Add( new TriangleIndices(  5 , 11 , 4 ) );
      triIdx.Add( new TriangleIndices( 11 , 10 , 2 ) );
      triIdx.Add( new TriangleIndices( 10 ,  7 , 6 ) );
      triIdx.Add( new TriangleIndices(  7 ,  1 , 8 ) );

      // 5 faces around point 3
      triIdx.Add( new TriangleIndices( 3 , 9 , 4 ) );
      triIdx.Add( new TriangleIndices( 3 , 4 , 2 ) );
      triIdx.Add( new TriangleIndices( 3 , 2 , 6 ) );
      triIdx.Add( new TriangleIndices( 3 , 6 , 8 ) );
      triIdx.Add( new TriangleIndices( 3 , 8 , 9 ) );

      // 5 adjacent faces 
      triIdx.Add( new TriangleIndices( 4 , 9 ,  5 ) );
      triIdx.Add( new TriangleIndices( 2 , 4 , 11 ) );
      triIdx.Add( new TriangleIndices( 6 , 2 , 10 ) );
      triIdx.Add( new TriangleIndices( 8 , 6 ,  7 ) );
      triIdx.Add( new TriangleIndices( 9 , 8 ,  1 ) );

      // Refine triangles
      for ( int i = 0 ; i < recursionLevel ; ++i )
      {
        List<TriangleIndices> faces2 = new List<TriangleIndices>();

        foreach ( var tri in triIdx )
        {
          // Replace triangle by 4 triangles.
          int a = getMiddlePoint( tri.v1, tri.v2, ref vertList, ref middlePointIndexCache, radius);
          int b = getMiddlePoint( tri.v2, tri.v3, ref vertList, ref middlePointIndexCache, radius);
          int c = getMiddlePoint( tri.v3, tri.v1, ref vertList, ref middlePointIndexCache, radius);

          faces2.Add( new TriangleIndices( tri.v1 , a , c ) );
          faces2.Add( new TriangleIndices( tri.v2 , b , a ) );
          faces2.Add( new TriangleIndices( tri.v3 , c , b ) );
          faces2.Add( new TriangleIndices(      a , b , c ) );
        }
        triIdx = faces2;
      }

      mesh.vertices = vertList.ToArray();

      List< int > triList = new List<int>();

      for ( int i = 0, n = triIdx.Count ; ( i < n ) ; ++i )
      {
        triList.Add( triIdx[i].v1 );
        triList.Add( triIdx[i].v2 );
        triList.Add( triIdx[i].v3 );
      }

      mesh.triangles = triList.ToArray();

      // Normals.

      Vector3[] normals = new Vector3[ vertList.Count];

      for ( int i = 0, n = normals.Length ; ( i < n ) ; ++i )
      {
        normals[i] = vertList[i].normalized;
      }

      mesh.normals = normals;

      //UVs

      Vector2[] uvlist = new Vector2[ mesh.vertices.Length ];

      MeshHelper.SphUV( mesh.vertices , uvlist );

      mesh.SetUVs( 0 , uvlist );

      mesh.RecalculateBounds();
      mesh.Optimize();
    }

    /// <summary>
    /// Creates a uv mapping of the mesh with spherical projection.
    /// </summary>
    private static void SphUV ( Vector3[] vertices , Vector2[] uv )
    {
      for ( int i = 0, n = vertices.Length ; ( i < n ) ; ++i )
      {
        Vector3 v = vertices[i];

        Vector2 textureCoordinates;

        textureCoordinates.x = Mathf.Atan2( v.x , v.z ) / ( -2f * Mathf.PI );

        if ( textureCoordinates.x < 0f )
        {
          textureCoordinates.x += 1f;
        }

        textureCoordinates.y = Mathf.Asin( v.y ) / Mathf.PI + 0.5f;

        uv[i] = textureCoordinates;
      }
    }
  }
}