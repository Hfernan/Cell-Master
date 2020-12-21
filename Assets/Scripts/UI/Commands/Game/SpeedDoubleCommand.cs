using UnityEngine;

namespace KT
{
  public class SpeedDoubleCommand : UICommand
  {
    public SpeedDoubleCommand ()
    {
      commandName = "Double Speed";

      cmdId = Id.SpeedDouble;
    }

    public override void DoCommand ()
    {
      Time.timeScale = 2f;
    }
  }
}