using UnityEngine;

namespace KT
{
  public class UICommand
  {
    // Add below! ( Just above Cnt ).
    public enum Id : int
    {
      None,
      NewPlanet,
      KillHuman,
      BornHuman,
      TeleportHuman,
      TopUI,
      StartGame,
      ActionUI,
      TimeUI,
      BornRndBush,
      KillBush,
      BornBush,
      DetailUI,
      SpeedHalf,
      SpeedNormal,
      SpeedDouble,
      SpeedTriple,
      SpeedPause,
      CloneHuman,
      QuitGame,
      Cnt
    }

    protected string commandName;
    protected Id cmdId;

    public UICommand ()
    {
      commandName = "Def Command";

      cmdId = Id.None;
    }

    public string GetName () { return commandName; }

    public Id GetId () { return cmdId; }

    void Default () { Debug.LogError( "Bad number of command arguments." ); }

    public virtual void DoCommand (                                         ) { Default(); }
    public virtual void DoCommand ( object arg0                             ) { Default(); }
    public virtual void DoCommand ( object arg0 , object arg1               ) { Default(); }
    public virtual void DoCommand ( object arg0 , object arg1 , object arg2 ) { Default(); }
  }
}