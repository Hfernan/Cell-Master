using UnityEngine;

namespace KT
{
  /// <summary>
  /// This class builds a planet.
  /// Actually it is a flat plane.
  /// </summary>
  public static class PlanetBuilder
  {
    public struct WorldData
    {
      public int seed;

      public int   subdiv;     // Mesh subdivisions.
      public float halfWidth;  // Half the width of the planet.
      public float halfHeight; // Half the height of the planet.

      // Unused atm.
      public float noiseScale;
      public float heightMultiplier;
      public AnimationCurve heightCurve;

      public static WorldData Default ()
      {
        WorldData data;

        data.seed = 722;

        data.subdiv = 4;

        data.halfHeight = 25f;
        data.halfWidth  = 25f;

        data.noiseScale = 3f;
        data.heightMultiplier = 5f;

        AnimationCurve heightCurve = new AnimationCurve();

#if false
        // This makes a flat curve.
        mapToHeight.AddKey( new Keyframe( 0f , 0.5f , 0f , 0f ) );
        mapToHeight.AddKey( new Keyframe( 1f , 0.5f , 0f , 0f ) );
#else
        heightCurve.AddKey( new Keyframe( 0f , 0.35f , 0f , 0f ) );
        heightCurve.AddKey( new Keyframe( 0.4f , 0.4f , .5f , .5f ) );
        heightCurve.AddKey( new Keyframe( 0.8f , 0.8f , .5f , .5f ) );
        heightCurve.AddKey( new Keyframe( 1f , .85f , 0f , 0f ) );
#endif

        data.heightCurve = heightCurve;

        return data;
      }

      public static WorldData Init ( int seed , int subdiv , float width , float height , float noiseScale , float heightMultiplier , AnimationCurve heightCurve )
      {
        WorldData data;

        data.seed = seed;

        data.subdiv = subdiv;

        data.halfWidth  = width  / 2;
        data.halfHeight = height / 2;

        data.noiseScale       = noiseScale;
        data.heightMultiplier = heightMultiplier;
        data.heightCurve      = heightCurve;

        return data;
      }
    }

    /// <summary>
    /// Generates a planet with default parameters.
    /// </summary>
    /// <param name="gameObject">Planet GO.</param>
    public static void Generate ( ref GameObject gameObject )
    {
      WorldData data = WorldData.Default();

      Generate( ref gameObject , ref data );
    }

    /// <summary>
    /// Generates a planet with given parameters.
    /// </summary>
    /// <param name="gameObject">Planet GO.</param>
    /// <param name="data">Parameters.</param>
    public static void Generate ( ref GameObject gameObject , ref WorldData data )
    {
      Random.InitState( data.seed );

      MeshFilter mf;

      MeshRenderer mr;

      gameObject.GetCompOrAdd( out mr );// MeshRenderer
      gameObject.GetCompOrAdd( out mf );// MeshFilter

      // Create mesh.

      Mesh mesh = mf.mesh;

      PlaneCreator.Create( mesh , /* Width */2 * data.halfWidth , /* Heigth */ 2 * data.halfHeight , /* ResX */ Mathf.FloorToInt( data.halfWidth ) , /* ResZ */ Mathf.FloorToInt( data.halfHeight ) );

      // Get noise map

      //int offX = Mathf.FloorToInt( Random.value * 100000 );
      //int offZ = Mathf.FloorToInt( Random.value * 100000 );
      //
      //float[,] noiseMap = MapHelper.GenerateMap( mesh , data.noiseScale , offX , offZ );

      // Attach texture

      //Texture2D mapTexture = TexHelper.BuildTexture( in noiseMap );
      //
      //mr.material.mainTexture = mapTexture;

      // Move vertices

      //MeshHelper.UpdateMeshVertices( mesh , mapTexture , data.heightCurve , data.heightMultiplier );

      // Collider.
      MeshCollider mc;

      gameObject.GetCompOrAdd<MeshCollider>( out mc );

      mc.sharedMesh = mesh;

      // Pathfinding.
      AddPathfinder( gameObject , mesh );
    }

    private static void AddPathfinder ( GameObject gameObject , Mesh mesh )
    {
      AstarPath pathfinder;

      gameObject.GetCompOrAdd( out pathfinder );

      Pathfinding.NavMeshGraph graph = pathfinder.data.navmesh; Debug.Assert( graph != null , "PointGraph not found." );

      graph.sourceMesh = mesh;

      graph.Scan();
    }
  }
}