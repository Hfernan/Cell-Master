using UnityEngine;

namespace KT
{
  public class SpeedNormalCommand : UICommand
  {
    public SpeedNormalCommand ()
    {
      commandName = "Normal Speed";

      cmdId = Id.SpeedNormal;
    }

    public override void DoCommand ()
    {
      Time.timeScale = 1f;
    }
  }
}