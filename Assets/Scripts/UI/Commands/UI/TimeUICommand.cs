using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KT
{
  public class TimeUICommand : UICommand
  {
    public enum SubType : int
    {
      None,
      Disable,
      Enable,
      UpdateTime,
      UpdateTarget,
      Cnt
    }

    SubType type;

    Calendar.Date date;

    public TimeUICommand ( SubType _type = SubType.None )
    {
      commandName = "Update Time UI";

      cmdId = Id.TimeUI;

      type = _type;
    }

    public TimeUICommand ( Calendar.Date _date , SubType _type = SubType.None )
    {
      commandName = "Update Time UI";

      cmdId = Id.TimeUI;

      type = _type;

      date = _date;
    }

    public override void DoCommand ( object ui )
    {
      switch ( type )
      {
        case SubType.Disable:
          {
            GameObject pnl = ( ( TimeUIControl ) ui ).gameObject;

            pnl.SetActive( false );
          }
          break;
        case SubType.Enable:
          {
            GameObject pnl = ( ( TimeUIControl ) ui ).gameObject;

            pnl.SetActive( true );
          }
          break;
        case SubType.UpdateTime:
          {
            TimeUIControl pnl = ( TimeUIControl ) ui;

            pnl.UpdateTimeStamp( ServiceLoc.Instance.GetService<TimeControl>().TimeString( date , "r" ) );
          }
          break;
        default:
          {
            base.DoCommand( ui );
          }
          break;
      }
    }

    public override void DoCommand ( object ui , object color )
    {
      switch ( type )
      {
        case SubType.UpdateTarget:
          {
            if ( ( ui is TimeUIControl timeControl ) && ( color is float col ) )
            {
              timeControl.UpdateTarget( col );
            }
            break;
          }
        default:
          {
            base.DoCommand( ui , color );
          }
          break;
      }
    }
  }
}