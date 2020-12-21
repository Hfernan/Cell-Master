using UnityEngine;

namespace KT
{
  public class CloneHumanCommand : UICommand
  {
    public CloneHumanCommand ( HumanControl.HumanDNA _dna )
    {
      commandName = "Clone Human";

      cmdId = Id.CloneHuman;

      dna = _dna;
    }

    HumanControl.HumanDNA dna;

    public override void DoCommand ()
    {
      PlanetControl pControl = ServiceLoc.Instance.GetService<PlanetControl>();

      // Get a human.

      HumanControl human = HumanBuilder.Create( dna , /* clone */ true ).GetComp<HumanControl>();

      // Get spawn point.

      Vector3 normal = Vector3.up;

      Vector3 pt = pControl.SurfacePoint( Camera.main.transform.position , out normal , human.data.height );

      human.Instantiate( pt , normal );
    }
  }
}