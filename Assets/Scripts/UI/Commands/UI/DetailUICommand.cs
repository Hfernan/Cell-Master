using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KT
{
  public class DetailUICommand : UICommand
  {
    public enum SubType : int
    {
      None,
      Disable,
      Enable,
      Clear,
      Cnt
    }

    SubType type;

    public DetailUICommand ( SubType _type = SubType.None )
    {
      commandName = "Update Detail UI";

      cmdId = Id.DetailUI;

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
        case SubType.Clear:
          {
            if ( ui is GameObject pnl )
            {
              DetailUIObserver observer = pnl.GetComp<DetailUIObserver>();

              observer?.Clean();

              observer.observedActor = null;
            }
          }
          break;
      }
    }
  }
}