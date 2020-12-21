using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KT
{
  public class PlanetControl : MonoBehaviour
  {
    public PlanetBuilder.WorldData data;

    /// <summary>
    /// Returns a random point over the plane surface at distance dist or less.
    /// </summary>
    /// <param name="pt">Point around where to look.</param>
    /// <param name="dist">Max distance from pt</param>
    /// <param name="normal">Out the normal on the found point.</param>
    /// <returns></returns>
    public Vector3 RandNearOnPlaneSurface ( Vector3 pt , float dist , out Vector3 normal , float height )
    {
      Vector3 ret = pt.normalized;

      normal = Vector3.zero;

      Vector2 rand;

      bool found = false;

      // Limited attempts.
      for ( int i = 0, n = 10 ; ( ( i < n ) && !found ) ; ++i )
      {
        rand = dist * UnityEngine.Random.insideUnitCircle;

        ret = SurfacePoint( pt + new Vector3( rand.x , 0 , rand.y ) , out normal , height );

        found = CheckInside( ret );
      }

      if ( !found ) ret = Vector3.zero;

      return ret;
    }

    //~ - ,    \
    //      ' , \
    //          ,\
    //           ,\
    //            ,\
    //            , \
    /// <summary>
    /// Returns a random point over the sphere surface at dist dist or less.
    /// The appoximation works for small angles.
    /// </summary>
    /// <param name="pt">Point around where to look.</param>
    /// <param name="dist">Max distance from pt</param>
    /// <param name="normal">Out the normal on the found point.</param>
    /// <returns></returns>
    public Vector3 RandNearOnSphericalSurface ( Vector3 pt , float dist , out Vector3 normal , float height )
    {
      // First, get the direction, from the sphere origin to the new point.
      Vector3 ret = pt.normalized;

      Vector2 rand = dist * UnityEngine.Random.insideUnitCircle;

      // Contained in the plane tangential to the sphere in pt.
      Vector3 dir1 = new Vector3( -ret.y , ret.x , ret.z );

      // The other tangent.
      Vector3 dir2 = Vector3.Cross( ret , dir1 );

      // A random point on the plane. Then project to the sphere.
      ret = SurfacePoint( pt + ( rand.x * dir1 ) + ( rand.y * dir2 ) , out normal , height );

      return ret;
    }

    /// <summary>
    /// Random point in a plane centered at origin.
    /// </summary>
    /// <param name="height">Y-Height over the plane</param>
    /// <returns></returns>
    public Vector3 RandOnSurface ( out Vector3 normal , float height = 0f )
    {
      Vector3 pt = new Vector3( UnityEngine.Random.Range( -data.halfWidth , data.halfWidth ),
                                0,
                                UnityEngine.Random.Range( -data.halfHeight , data.halfHeight ));

      return SurfacePoint( pt , out normal , height );
    }

    /// <summary>
    /// Given a direction returns the point of the surface.
    /// </summary>
    /// <param name="dir">Direction to check. Unnormalized is ok.</param>
    /// <param name="normal">Out. Normal vector at given point.</param>
    /// <param name="height">Elevates the point above/under the ground following the normal direction.</param>
    /// <returns>Point of the mesh at given direction.</returns>
    public Vector3 SurfacePoint ( Vector3 dir , out Vector3 normal , float height = 0f )
    {
      Vector3 pt = new Vector3( dir.x , 0 , dir.z );

      normal = Vector3.up;

      return pt;
    }

    private bool CheckInside ( Vector3 pt )
    {
      return ( pt.x > -data.halfWidth  ) && ( pt.x < data.halfWidth  ) &&
             ( pt.z > -data.halfHeight ) && ( pt.z < data.halfHeight );
    }
  }
}