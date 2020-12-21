using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KT
{
  // Fns to edit Meshes.
  public static class MeshHelper
  {
    /// <summary>
    /// Moves vertices in/outwards their normals following a texture.
    /// 0.5 keeps the vertex in the same point.
    /// </summary>
    /// <param name="mesh">Mesh to edit.</param>
    /// <param name="heightMap">Height map.</param>
    /// <param name="curve">Relation between heightMap values and actual movement.</param>
    /// <param name="heightScale">Global multiplier of the movement.</param>
    public static void UpdateMeshVertices ( Mesh mesh , Texture2D heightMap , AnimationCurve curve , float heightScale = 1f )
    {
      Vector3[] meshVertices = mesh.vertices;
      Vector3[] meshNormals = mesh.normals;
      Vector2[] meshUVs = mesh.uv;

      // iterate through all the heightMap coordinates, updating the vertex index
      int vertexIndex = 0;

      for ( int i = 0, n = meshVertices.Length ; ( i < n ) ; ++i )
      {
        Vector2 uv = meshUVs[ vertexIndex ];

        float height = heightMap.GetPixelBilinear( uv.x , uv.y ).grayscale;

        Vector3 vertex = meshVertices[ vertexIndex ];
        Vector3 normal = meshNormals [ vertexIndex ];

        // change the vertex Y coordinate, proportional to the height value
        meshVertices[vertexIndex] = vertex + ( ( curve.Evaluate( height ) - .5f ) * heightScale * normal );

        ++vertexIndex;
      }

      // update the vertices in the mesh and update its properties
      mesh.vertices = meshVertices;

      mesh.RecalculateBounds();

      mesh.RecalculateNormals();
    }

    /// <summary>
    /// Sets UV values sequentially for each vertex.
    /// </summary>
    /// <param name="mesh">Mesh to set uvs.</param>
    /// <param name="mapWidth">Number of vertex per line.</param>
    /// <param name="mapHeight">Number of vertex per column.</param>
    public static void SeqUVs ( ref Mesh mesh , int mapWidth , int mapHeight )
    {
      // Z       V
      // ^       ^
      // |       |
      // +--> X  + --> U

      Vector2[] uvlist = new Vector2[ mesh.vertexCount ];

      Debug.Assert( ( mapWidth * mapHeight ) >= mesh.vertexCount , "Not enough space for all points." );

      for ( int i = 0, n = mesh.vertexCount ; ( i < n ) ; ++i )
      {
        //int xIndex = i % mapWidth;
        int zIndex = Mathf.FloorToInt ( i / ( ( float ) mapWidth ) );

        float u = Mathf.InverseLerp( 0 , mapWidth - 1 , i % mapWidth );
        float v = Mathf.InverseLerp( 0 , mapHeight -1 , zIndex );

        uvlist[i] = new Vector2( u , v );
      }

      mesh.SetUVs( 0 , uvlist );
    }

    /// <summary>
    /// Spherical unwrapping of UVs.
    /// </summary>
    /// <param name="vertices">Vertex list.</param>
    /// <param name="uv">Already initialized UV vector.</param>
    public static void SphUV ( Vector3[] vertices , Vector2[] uv )
    {
      for ( int i = 0, n = vertices.Length ; ( i < n ) ; ++i )
      {
        Vector3 v = vertices[i].normalized;

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