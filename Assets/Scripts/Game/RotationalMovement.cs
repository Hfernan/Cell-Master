using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KT
{
  // Controls the movement of the camera around a point.
  // Click + drag: Rotate around the center.
  // Double click + drag: Rotates the camera itself.
  // Rueda del Raton: Zoom in/out.

  [RequireComponent( typeof( Camera ) )]
  public class RotationalMovement : MonoBehaviour
  {
    public enum State : int
    {
      None,
      Click,
      DClick,
      Cnt,
    }

    [SerializeField] Light dirLight;

    [Tooltip("The point to rotate around.")]
    [SerializeField] Vector3 ptCenter = Vector3.zero;

    [Tooltip("If the camera should follow a tranform.")]
    [SerializeField] Transform toFollow = null;

    [Header("Velocities")]
    [Tooltip("Speed to zoom in/out.")]
    [SerializeField] float speedR = 50.0f;

    [Tooltip("Speed to rotate horizontally.")]
    [SerializeField] float speedTheta = 3f;

    [Tooltip("Speed to rotate vertically.")]
    [SerializeField] float speedPhi = 3f;

    [Tooltip("Speed to rotate the cam itself.")]
    [SerializeField] float speedRoll = 300f;

    State curState = State.None;

    float dClickTimeLimit;

    Vector2 ptDrag     = Vector2.zero;
    Vector2 prevPtDrag = Vector2.zero;

    // Used for delta of movement in spherical coordinates.
    float dR     = 0f;
    float dPhi   = 0f;
    float dTheta = 0f;

    // Used for delta of camera roll.
    float dRoll = 0f;

    // Distance to center the previous frame.
    float prevDist = 1f;

    void Awake ()
    {
      toFollow = null;
    }

    void Start ()
    {
      prevDist = ( transform.position - ptCenter ).magnitude;
    }

    private void Update ()
    {
      dR     = 0f;
      dPhi   = 0f;
      dTheta = 0f;
      dRoll  = 0f;

      // R delta.
      dR = Input.mouseScrollDelta.y * speedR;

      if ( curState == State.None )
      {
        // Go to click
        if ( Input.GetMouseButtonDown( 0 ) )
        {
          //Save pt and click time.

          ptDrag = Input.mousePosition;

          dClickTimeLimit = Time.unscaledTime + GVar.dClickTime;

          curState = State.Click;
        }
      }
      else if ( curState == State.Click )// A single click moves the camera around.
      {
        prevPtDrag = ptDrag;

        ptDrag = Input.mousePosition;

        // Clicked and before time limit -> double click.
        if ( Input.GetMouseButtonDown( 0 ) && ( Time.unscaledTime < dClickTimeLimit ) )
        {
          curState = State.DClick;
        }
        else if ( Input.GetMouseButton( 0 ) ) // Button is still hold down.
        {
          // Or rotate
          dTheta = ( ptDrag.x - prevPtDrag.x );
          dPhi   = ( ptDrag.y - prevPtDrag.y );
        }
        else if ( Time.unscaledTime > dClickTimeLimit ) // Button is up and after double click time.
        {
          curState = State.None;
        }
      }
      else if ( curState == State.DClick ) // Double click changes the camera roll.
      {
        prevPtDrag = ptDrag;

        ptDrag = Input.mousePosition;

        // Rotate
        if ( Input.GetMouseButton( 0 ) )
        {
          dRoll = ( ptDrag.x - prevPtDrag.x );
        }
        else
        {
          // Or go to none.
          curState = State.None;
        }
      }

#if UNITY_EDITOR
      float speed = 2.0f; // Free to change.

           if ( Input.GetAxisRaw( "Horizontal" ) >  0.5f ) dTheta = -speed;
      else if ( Input.GetAxisRaw( "Horizontal" ) < -0.5f ) dTheta =  speed;

           if ( Input.GetAxisRaw( "Vertical" ) >  0.5f ) dPhi = -speed;
      else if ( Input.GetAxisRaw( "Vertical" ) < -0.5f ) dPhi =  speed;

           if ( Input.GetKey( KeyCode.E ) ) dRoll = -speed;
      else if ( Input.GetKey( KeyCode.Q ) ) dRoll =  speed;
#endif
    }

    // This moves the camera.
    void LateUpdate ()
    {
      if ( toFollow == null ) // No one to follow. Move with input.
      {
        bool hasR     = ( dR     != 0f );
        bool hasTheta = ( dTheta != 0f );
        bool hasPhi   = ( dPhi   != 0f );
        bool hasRoll  = ( dRoll  != 0f );

        if ( Input.GetKeyDown( KeyCode.P ) )
        {
          // After several movements and roll it is good to have
          // a fixed orientation as reference.
          ReorientCamera();
        }
        else if ( hasTheta || hasPhi || hasR ) // More likely to have theta/phi changes than roll.
        {
          // The minus on dTheta and dPhi are intended.
          transform.Translate( Time.deltaTime * new Vector3( -dTheta * speedTheta , -dPhi * speedPhi , dR ) );

          // Look to center again.
          LookCenter();

          if ( hasR )
          {
            // Save new center distance.
            prevDist = ( transform.position - ptCenter ).magnitude;
          }
          else
          {
            // Reset distance to center.
            float curDist = ( transform.position - ptCenter ).magnitude;

            transform.Translate( Vector3.forward * ( curDist - prevDist ) );
          }
        }
        else if ( hasRoll ) // The current control-scheme forbids roll and angle changing at the same time. But can be separated if needed.
        {
          transform.Rotate( Vector3.forward , dRoll * speedRoll * Time.deltaTime );
        }
      }
      else
      {
        //Following a Transform.

        // Move camera to the vertical of the followed transform.

        Vector3 dir = ( toFollow.position - ptCenter ).normalized;

        transform.position = ptCenter + ( prevDist * dir );

        // Reorient.
        LookCenter();
      }
    }

    /// <summary>
    /// Look at center point.
    /// </summary>
    void LookCenter ()
    {
      transform.LookAt( ptCenter , transform.up );
    }

    void StartFollowing ( Transform tf ) { toFollow = tf; }

    /// <summary>
    /// Look at center point and orientate it upwards.
    /// </summary>
    void ReorientCamera ()
    {
      transform.LookAt( ptCenter , Vector3.up );
    }
  }
}