using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KT
{
  /// <summary>
  /// This class makes the world IBL / Skybox rotate around as the time changes.
  /// </summary>
  [RequireComponent( typeof( Light ) )]
  public class LightControl : MonoBehaviour
  {
    float offset = 90;// Arbitrary number to offset the rotation angle.

    void Start ()
    {
      transform.position = Vector3.zero;
    }

    public void OnMinChanged ( Calendar.Date date )
    {
      float angle = ( date.dayFrac * 360 );

      transform.rotation = Quaternion.LookRotation( Quaternion.Euler( 0 , angle + offset , 0 ) * Vector3.right , transform.up );

      RenderSettings.skybox.SetFloat( "_Rotation" , -angle );
    }
  }
}