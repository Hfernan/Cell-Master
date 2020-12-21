using UnityEngine;

namespace KT
{
  public class BornRndBushCommand : UICommand
  {
    public BornRndBushCommand ()
    {
      commandName = "Born Random Bush";

      cmdId = Id.BornRndBush;
    }

    public override void DoCommand ( object parent )
    {
      PlanetControl pControl = ServiceLoc.Instance.GetService<PlanetControl>();

      // Get a human.

      if ( parent is Transform p )
      {
        BushControl bush = BushBuilder.Create().GetComp<BushControl>();

        // Get spawn point.

        Vector3 normal = Vector3.up;

        Vector3 pt;

        pt = pControl.RandOnSurface( out normal );

        bush.Instantiate( pt , normal , p );
      }
    }
  }
}