using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KT
{
  /// <summary>
  /// This class registers alive Human actors.
  /// </summary>
  public class HumanityControl
  {
    List<HumanControl> humanList = new List<HumanControl>();

    public void RegisterHuman ( HumanControl human ){ humanList.Add( human ); }

    public float CalcScore ()
    {
      float score = 0f;

      float target = ServiceLoc.Instance.GetService<GameManager>()?.GetTargetHue() ?? 0f;

      int count = 0;

      // We don't care about dead humans.
      // Their GO may have been destroyed.

      humanList.RemoveAll( ( h ) => ( h == null ) );

      for ( int i = 0, n = humanList.Count ; ( i < n ) ; ++i )
      {
        HumanControl h = humanList[i];

        // Just in case they died in between.
        if ( h != null && h.isActiveAndEnabled )
        {
          // Calculate score for this human.
          float dist =  Mathf.Abs( humanList[i].data.hue - target );

          if ( dist > .5f ) dist = 1 - dist;

          score += ( 1 - ( 2 * dist ) );

          ++count;
        }
      }

      if ( count != 0 ) score /= count;

      return score;
    }

    public void InitialPosition ()
    {
      if ( humanList.Count >= 6 )
      {
        humanList[0].transform.position = new Vector3( -1 , 0 , 1 );
        humanList[1].transform.position = new Vector3( -1 , 0 , 0 );
        humanList[2].transform.position = new Vector3(  0 , 0 , 1 );
        humanList[3].transform.position = new Vector3(  0 , 0 , 0 );
        humanList[4].transform.position = new Vector3(  1 , 0 , 1 );
        humanList[5].transform.position = new Vector3(  1 , 0 , 0 );
      }
    }
  }
}