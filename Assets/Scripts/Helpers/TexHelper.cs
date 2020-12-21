using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KT
{
  // Fns to create textures.
  public static class TexHelper
  {
    /// <summary>
    /// Creates a BW texture from a float[,] map.
    /// </summary>
    /// <param name="heightMap"></param>
    /// <returns></returns>
    public static Texture2D BuildTexture ( in float[,] heightMap )
    {
      int mapDepth = heightMap.GetLength (0);
      int mapWidth = heightMap.GetLength (1);

      Color[] colorMap = new Color[mapDepth * mapWidth];

      for ( int zIndex = 0 ; zIndex < mapDepth ; ++zIndex )
      {
        for ( int xIndex = 0 ; xIndex < mapWidth ; ++xIndex )
        {
          // transform the 2D map index is an Array index
          int colorIndex = ( zIndex * mapWidth ) + xIndex;

          float height = heightMap[zIndex, xIndex];

          // Assign the color.
          colorMap[colorIndex] = Color.Lerp( Color.white , Color.black , height );
        }
      }

      // create a new texture and set its pixel colors
      Texture2D tileTexture = new Texture2D (mapWidth, mapDepth);

      tileTexture.wrapMode = TextureWrapMode.Clamp;

      tileTexture.SetPixels( colorMap );

      tileTexture.Apply();

      return tileTexture;
    }
  }
}