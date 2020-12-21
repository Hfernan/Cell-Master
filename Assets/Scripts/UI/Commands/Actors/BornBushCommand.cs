using UnityEngine;

namespace KT
{
  public class BornBushCommand : UICommand
  {
    public BornBushCommand ()
    {
      commandName = "Born Bush";

      cmdId = Id.BornBush;
    }

    public override void DoCommand ( object parent )
    {
      if ( parent is Transform p )
      {
        PlanetControl pControl = ServiceLoc.Instance.GetService<PlanetControl>();

        // Get a human.

        BushControl bush = BushBuilder.Create().GetComp<BushControl>();

        // Get spawn point.

        Vector3 normal = Vector3.up;

        Vector3 pt = pControl.SurfacePoint( Camera.main.transform.position , out normal );

        bush.Instantiate( pt , normal , p );
      }
    }
  }
}