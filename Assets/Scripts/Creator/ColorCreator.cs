using UnityEngine;

namespace KT
{
  public static class ColorCreator
  {
    public static Color HSVRandom ()
    {
      return Color.HSVToRGB( UnityEngine.Random.value , 1f , 1f );
    }
  }
}