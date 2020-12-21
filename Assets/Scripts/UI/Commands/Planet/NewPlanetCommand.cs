using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KT
{
  public class NewPlanetCommand : UICommand
  {
    public NewPlanetCommand ()
    {
      commandName = "New Planet";

      cmdId = Id.NewPlanet;
    }

    public override void DoCommand ()
    {
      // Generate World.
#if false
      // We cant use this atm bc Pathfinder can't be dynamically generated yet.
      GameObject world = new GameObject( "World" );
#else
      GameObject world = GameObject.Find( "World" );
#endif
      PlanetControl planetControl = world.AddComponent<PlanetControl>();

      ServiceLoc.Instance.RegisterService( planetControl );

      PlanetBuilder.WorldData wData = PlanetBuilder.WorldData.Default();
      {
        PlanetBuilder.Generate( ref world , ref wData );
      }

      planetControl.data = wData;

      // Populate with random humans.

      HumanControl.HumanDNA dna = new HumanControl.HumanDNA();

      dna.hue = ServiceLoc.Instance.GetService<GameManager>()?.GetTargetHue() ?? 0f;

      if ( dna.hue > 0.5 ) dna.hue -= .5f;
      else dna.hue += .5f;

      for ( int i = 0 ; i < 6 ; ++i )
        ServiceLoc.Instance.GetService<GameManager>().OnCommandRcvd( CommandFactory.Create( Id.CloneHuman , dna ) );

      ServiceLoc.Instance.GetService<HumanityControl>()?.InitialPosition();

      // Add initial food at random.

      for ( int i = 0 ; i < 48 ; ++i )
        ServiceLoc.Instance.GetService<GameManager>().OnCommandRcvd( CommandFactory.Create( Id.BornRndBush ) );
    }
  }
}