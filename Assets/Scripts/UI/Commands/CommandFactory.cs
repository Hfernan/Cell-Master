using UnityEngine;

namespace KT
{
  // Class to create UI Commands.
  public static class CommandFactory
  {
    public static UICommand Create ( UICommand.Id id , object v0 = null , object v1 = null )
    {
      switch ( id )
      {
        case UICommand.Id.NewPlanet:
          return new NewPlanetCommand();

        case UICommand.Id.KillHuman:
          return new KillHumanCommand( ( v0 == null ) ? KillHumanCommand.Reason.None : ( KillHumanCommand.Reason ) v0 );
        case UICommand.Id.BornHuman:
          return new BornHumanCommand();
        case UICommand.Id.TeleportHuman:
          return new TeleportHumanCommand();
        case UICommand.Id.CloneHuman:
          if ( v0 is HumanControl.HumanDNA dna ) return new CloneHumanCommand( dna );
          else return null;

        case UICommand.Id.StartGame:
          return new StartGameCommand();

        case UICommand.Id.BornRndBush:
          return new BornRndBushCommand();
        case UICommand.Id.BornBush:
          return new BornBushCommand();
        case UICommand.Id.KillBush:
          return new KillBushCommand();

        case UICommand.Id.ActionUI:
          return new ActionUICommand( ( v0 == null ) ? ActionUICommand.SubType.None : ( ActionUICommand.SubType ) v0 );
        case UICommand.Id.DetailUI:
          return new DetailUICommand( ( v0 == null ) ? DetailUICommand.SubType.None : ( DetailUICommand.SubType ) v0 );
        case UICommand.Id.TimeUI:
          {
            if ( v1 == null )
            {
              return new TimeUICommand( ( v0 == null ) ? TimeUICommand.SubType.None : ( TimeUICommand.SubType ) v0 );
            }
            else
            {
              return new TimeUICommand( ( Calendar.Date ) v1 , ( TimeUICommand.SubType ) v0 );
            }
          }
        case UICommand.Id.TopUI:
          return new TopUICommand( ( TopUICommand.SubType ) v0 );

        case UICommand.Id.SpeedHalf:
          return new SpeedHalfCommand();
        case UICommand.Id.SpeedNormal:
          return new SpeedNormalCommand();
        case UICommand.Id.SpeedDouble:
          return new SpeedDoubleCommand();
        case UICommand.Id.SpeedTriple:
          return new SpeedTripleCommand();
        case UICommand.Id.SpeedPause:
          return new SpeedPauseCommand();

        case UICommand.Id.QuitGame:
          return new QuitGameCommand();

        case UICommand.Id.Cnt:
        case UICommand.Id.None:
        default:
          Debug.LogError( "Couldnt create a " + id + " command." );
          return null;
      }
    }
  }
}