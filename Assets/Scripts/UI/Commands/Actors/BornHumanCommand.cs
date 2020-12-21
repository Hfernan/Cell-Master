using UnityEngine;

namespace KT
{
  public class BornHumanCommand : UICommand
  {
    public BornHumanCommand ()
    {
      commandName = "Born Human";

      cmdId = Id.BornHuman;
    }

    public override void DoCommand ()
    {
      PlanetControl pControl = ServiceLoc.Instance.GetService<PlanetControl>();

      // Get a human.

      HumanControl human = HumanBuilder.Create().GetComp<HumanControl>();

      // Get spawn point.

      Vector3 normal = Vector3.up;

      Vector3 pt = pControl.SurfacePoint( Camera.main.transform.position , out normal , human.data.height );

      human.Instantiate( pt , normal );
    }
  }
}