using UnityEngine;

namespace KT
{
  public class SpeedTripleCommand : UICommand
  {
    public SpeedTripleCommand ()
    {
      commandName = "Triple Speed";

      cmdId = Id.SpeedTriple;
    }

    public override void DoCommand ()
    {
      Time.timeScale = 3f;
    }
  }
}