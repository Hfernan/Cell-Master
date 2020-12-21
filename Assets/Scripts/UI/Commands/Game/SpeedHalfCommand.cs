using UnityEngine;

namespace KT
{
  public class SpeedHalfCommand : UICommand
  {
    public SpeedHalfCommand ()
    {
      commandName = "Half Speed";

      cmdId = Id.SpeedHalf;
    }

    public override void DoCommand ()
    {
      Time.timeScale = 0.5f;
    }
  }
}