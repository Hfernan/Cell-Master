using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

namespace KT
{
  [RequireComponent( typeof( Seeker ) )]
  public class ActorPath : MonoBehaviour
  {
    // Check publics.
    Seeker seeker;

    Path path;

    bool isCalculating = false;

    public float height  = 0f;

    [SerializeField] float speed = 5f;

    [SerializeField] float nextWaypointDistance = 3;

    private int currentWaypoint = 0;

    public bool reachedEndOfPath;

    public void Start ()
    {
      // Get a reference to the Seeker component we added earlier
      seeker = GetComponent<Seeker>();
    }

    public void Update ()
    {
      // We have no path to follow yet, so don't do anything
      if ( path != null )
      {
        // Check in a loop if we are close enough to the current waypoint to switch to the next one.
        // We do this in a loop because many waypoints might be close to each other and we may reach
        // several of them in the same frame.
        reachedEndOfPath = false;

        // The distance to the next waypoint in the path
        float distanceToWaypoint = 0f;

        int safety = 1000;
        int safety_cnt = 0;

        while ( safety_cnt < safety )
        {
          if ( ++safety_cnt == safety ) { Debug.LogError( "This loop exceded safety count." ); }

          if ( path.vectorPath.Count < Math.Max( 0 , currentWaypoint - 2 ) ) { break; }

          // If you want maximum performance you can check the squared distance instead to get rid of a
          // square root calculation. But that is outside the scope of this tutorial.
          distanceToWaypoint = Vector3.Distance( transform.position , path.vectorPath[currentWaypoint] );

          if ( distanceToWaypoint < nextWaypointDistance )
          {
            // Check if there is another waypoint or if we have reached the end of the path
            if ( currentWaypoint + 1 < path.vectorPath.Count )
            {
              ++currentWaypoint;
            }
            else
            {
              // Set a status variable to indicate that the agent has reached the end of the path.
              // You can use this to trigger some special code if your game requires that.
              reachedEndOfPath = true;
              break;
            }
          }
          else
          {
            break;
          }
        }

        // Slow down smoothly upon approaching the end of the path
        // This value will smoothly go from 1 to 0 as the agent approaches the last waypoint in the path.

        if ( !reachedEndOfPath && ( path.vectorPath.Count > 1 ) )
        {
          var speedFactor = reachedEndOfPath ? Mathf.Sqrt( distanceToWaypoint / nextWaypointDistance ) : 1f;

          // Direction to the next waypoint.
          Vector3 dir = ( path.vectorPath[currentWaypoint] - path.vectorPath[currentWaypoint - 1] ).normalized;

          // Multiply the direction by our desired speed to get a velocity
          Vector3 velocity = dir * speed * speedFactor;

          transform.position += velocity * Time.deltaTime;

          // Rotate to accomodate ground normal.
          //AdjustRotation( in dir );
        }
      }
    }

#if false
    //Can be used in spherical / rugged terrain.
    void AdjustRotation ( in Vector3 dir )
    {
      Vector3 locVert = transform.position.normalized;

      // Get ground position and normal.

      if ( Physics.Raycast( transform.position , -transform.position , out RaycastHit hit , Mathf.Infinity , GVar.TerrainLayer ) )
      {
        // Compare offset from desired elevation and correct.
        float elevation = ( transform.position - hit.point ).magnitude;

        transform.position += ( height - elevation ) * locVert;

        locVert = hit.normal;

        //Debug.DrawLine( transform.position , transform.position + dir * 2 , Color.blue );
        //Debug.DrawLine( transform.position , transform.position + locVert * 2 , Color.green );

        // Rotate to accomodate local vertical and movement direction.
        transform.rotation = Quaternion.LookRotation( dir , locVert );
      }
    }
#endif

    public void OnPathComplete ( Path p )
    {
      if ( !p.error )
      {
        path = p;

        // Reset the waypoint counter so that we start to move towards the first point in the path
        currentWaypoint = 0;
      }
      else
      {
        Debug.LogWarning( "Path could not be calculated: " + p.errorLog );
      }

      isCalculating = false;
    }

    /// <summary>
    /// This fails if the point is on the other side of the sphere. I think.
    /// </summary>
    /// <param name="point">Point to go to.</param>
    public void GoTo ( Vector3 point )
    {
      // Start to calculate a new path to the targetPosition object, return the result to the OnPathComplete method.
      // Path requests are asynchronous, so when the OnPathComplete method is called depends on how long it
      // takes to calculate the path. Usually it is called the next frame.

      seeker.StartPath( transform.position , point , OnPathComplete );

      isCalculating = true;
    }

    public void Teleport ( Vector3 point )
    {
      path = null;

      transform.position = point;

      EndPath();
    }

    public void EndPath ()
    {
      reachedEndOfPath = true;
    }

    public bool IsCalculating () { return isCalculating; }
  }
}