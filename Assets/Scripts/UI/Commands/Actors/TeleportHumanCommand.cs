using UnityEngine;

namespace KT
{
  public class TeleportHumanCommand : UICommand
  {
    public TeleportHumanCommand ()
    {
      commandName = "Teleport Human";

      cmdId = Id.TeleportHuman;
    }

    public override void DoCommand ( object actor )
    {
      if ( actor is HumanControl human )
      {
        PlanetControl pControl = ServiceLoc.Instance.GetService<PlanetControl>();

        // Get point.

        Vector3 normal = Vector3.up;

        Vector3 pt = pControl.SurfacePoint( Camera.main.transform.position , out normal , human.data.height );

        human.Teleport( pt );
      }
    }
  }
}