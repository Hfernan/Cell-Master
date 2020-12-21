using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KT
{
  public class TopUICommand : UICommand
  {
    public enum SubType : int
    {
      None,
      Disable,
      Enable,
      StatePlaying, // Speed Buttons.
      Cnt
    }

    SubType type;

    public TopUICommand ( SubType _type = SubType.None )
    {
      commandName = "Update Top UI";

      cmdId = Id.TopUI;

      type = _type;
    }

    public override void DoCommand ( object ui )
    {
      switch ( type )
      {
        case SubType.Disable:
          {
            if ( ui is GameObject pnl )
            {
              pnl.SetActive( false );
            }
          }
          break;
        case SubType.Enable:
          {
            if ( ui is GameObject pnl )
            {
              pnl.SetActive( true );
            }
          }
          break;
        case SubType.StatePlaying:
          {
            if ( ui is GameObject pnl )
            {
              pnl.transform.GetChild( 0 )?.gameObject.SetActive( true );
              pnl.transform.GetChild( 1 )?.gameObject.SetActive( true );
              pnl.transform.GetChild( 2 )?.gameObject.SetActive( true );
              pnl.transform.GetChild( 3 )?.gameObject.SetActive( true );
              pnl.transform.GetChild( 4 )?.gameObject.SetActive( true );
            }
            break;
          }
      }
    }
  }
}