using UnityEngine;

namespace KT
{
  [RequireComponent( typeof( Camera ) )]
  public class CamPlaneMovement : MonoBehaviour, IHumanTracker
  {
    Camera cam;

    [Header("Velocities")]
    [Tooltip("Speed to pan.")]
    [SerializeField] float speedXZ = 50.0f;

    [Tooltip("Speed to zoom in/out.")]
    [SerializeField] float speedZoom = .5f;

    Vector2 dXZ; // Delta of movement in the XZ plane.
    float dZoom; // Delta of zoom movement.

    Transform tfFollow = null; // Current transform to follow.
    float followHeight = 0f; // Altitude to follow the transform.

    void Start ()
    {
      cam = gameObject.GetComp<Camera>();
    }

    void Update ()
    {
      dXZ = Vector2.zero;
      dZoom = 0f;

      // -- Position

           if ( Input.GetKey( KeyCode.D ) ) dXZ.x =  1;
      else if ( Input.GetKey( KeyCode.A ) ) dXZ.x = -1;

           if ( Input.GetKey( KeyCode.W ) ) dXZ.y =  1;
      else if ( Input.GetKey( KeyCode.S ) ) dXZ.y = -1;

      // -- Zoom

           if ( Input.mouseScrollDelta.y >  0.1f ) dZoom = ( Input.mouseScrollDelta.y * speedZoom );
      else if ( Input.mouseScrollDelta.y < -0.1f ) dZoom = ( Input.mouseScrollDelta.y * speedZoom );
    }

    // This moves the camera.
    void LateUpdate ()
    {
      if ( dXZ != Vector2.zero )
      {
        dXZ.Normalize();

        transform.Translate( speedXZ * dXZ * Time.unscaledDeltaTime );

        tfFollow = null;
      }
      else if ( tfFollow != null )
      {
        transform.position = tfFollow.position + followHeight * Vector3.up;
      }

      if ( dZoom != 0f )
      {
        cam.orthographicSize = Mathf.Clamp( cam.orthographicSize + dZoom , 5 , 20 );
      }
    }

    public void SetFollow ( Transform follow )
    {
      tfFollow = follow;

      followHeight = ( transform.position.y - follow.position.y );
    }

    void IHumanTracker.OnHumanClick      ( HumanControl h ) { SetFollow( h.transform ); }

    void IHumanTracker.OnHumanDataChange ( HumanControl h ) { throw new System.NotImplementedException(); }
    void IHumanTracker.OnHumanDeath      ( HumanControl h ) { throw new System.NotImplementedException(); }
  }
}