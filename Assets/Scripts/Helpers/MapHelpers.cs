using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KT
{
  // Fns to create certain maps.
  public static class MapHelper
  {
    /// <summary>
    /// Creates a Perlin Noise Map to cover all vertex of a given mesh.
    /// </summary>
    /// <param name="mesh">Mesh to cover.</param>
    /// <param name="mapScale">Perlin noise scale.</param>
    /// <returns></returns>
    public static float[,] GenerateMap ( Mesh mesh , float mapScale = 1f , int offsetX = 0 , int offsetZ = 0 )
    {
      // calculate tile depth and width based on the mesh vertices
      Vector3[] meshVertices = mesh.vertices;

      int mapDepth = Mathf.CeilToInt( Mathf.Sqrt (meshVertices.Length));

      int mapWidth = mapDepth;

      return PerlinNoiseMatrix.GenerateNoiseMap( mapDepth , mapWidth , mapScale , offsetX , offsetZ );
    }
  }
}