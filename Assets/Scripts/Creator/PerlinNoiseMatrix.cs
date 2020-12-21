using UnityEngine;

// From: https://gamedevacademy.org/complete-guide-to-procedural-level-generation-in-unity-part-1/

namespace KT
{
  public class PerlinNoiseMatrix
  {
    /// <summary>
    /// Creates a matrix of random perlin noise.
    /// </summary>
    public static float[,] GenerateNoiseMap ( int mapDepth , int mapWidth , float scale , float offsetX , float offsetZ )
    {
      // create an empty noise map with the mapDepth and mapWidth coordinates
      float[,] noiseMap = new float[mapDepth, mapWidth];

      for ( int zIndex = 0 ; zIndex < mapDepth ; ++zIndex )
      {
        for ( int xIndex = 0 ; xIndex < mapWidth ; ++xIndex )
        {
          // calculate sample indices based on the coordinates and the scale
          float sampleX = ( xIndex + offsetX ) / scale;
          float sampleZ = ( zIndex + offsetZ ) / scale;

          // generate noise value using PerlinNoise
          float noise = Mathf.PerlinNoise ( sampleX, sampleZ );

          noiseMap[zIndex , xIndex] = noise;
        }
      }

      return noiseMap;
    }
  }
}