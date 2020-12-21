using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KT
{
  public class ActionUICommand : UICommand
  {
    public enum SubType : int
    {
      None,
      Disable,
      Enable,
      Cnt
    }

    SubType type;

    public ActionUICommand ( SubType _type = SubType.None )
    {
      commandName = "Update Action UI";

      cmdId = Id.ActionUI;

      type = _type;
    }

    public override void DoCommand ( object ui )
    {
      switch ( type )
      {
        case SubType.Disable:
          {
            GameObject pnl = ( GameObject ) ui;

            pnl.SetActive( false );
          }
          break;
        case SubType.Enable:
          {
            GameObject pnl = ( GameObject ) ui;

            pnl.SetActive( true );
          }
          break;
      }
    }
  }
}