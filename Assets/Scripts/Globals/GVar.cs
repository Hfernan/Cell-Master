using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KT
{
  public static class GVar
  {
    public enum Scene : int
    {
      Title = 0,
      Credits = 1,
      Main = 2,
      Instructions = 3,
    }

    public const float dClickTime = 0.25f; // Max time between 2 clicks to detect as double click.

    // -- Physics.

    public const int TerrainLayer = 1 << 8;
    public const int ActorsLayer  = 1 << 9;
  }
}