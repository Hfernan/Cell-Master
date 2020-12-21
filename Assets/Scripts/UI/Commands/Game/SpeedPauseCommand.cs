using UnityEngine;

namespace KT
{
  public class SpeedPauseCommand : UICommand
  {
    public SpeedPauseCommand ()
    {
      commandName = "Pause Game";

      cmdId = Id.SpeedDouble;
    }

    public override void DoCommand ()
    {
      Time.timeScale = 0f;
    }
  }
}