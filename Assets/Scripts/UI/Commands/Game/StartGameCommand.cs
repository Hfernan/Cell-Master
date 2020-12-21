using UnityEngine;

namespace KT
{
  public class StartGameCommand : UICommand
  {
    public StartGameCommand ()
    {
      commandName = "Start Game";

      cmdId = Id.StartGame;
    }

    public override void DoCommand ( object _timeParent )
    {
      GameObject timeParent = (GameObject) _timeParent;

      // Start the GameManager.
      GameManager gameManager = ServiceLoc.Instance.GetService<GameManager>();

      // Decide win params.
      gameManager.InitialDecisions();

      // Start Humanity registry.
      HumanityControl humanityControl = new HumanityControl();

      ServiceLoc.Instance?.RegisterService<HumanityControl>( humanityControl );

      // Start timing.
      TimeControl timeControl = timeParent.AddComponent<TimeControl>();

      ServiceLoc.Instance.RegisterService<TimeControl>( timeControl );

      timeControl.onMinChanged .AddListener( gameManager.OnMinChanged  );
      timeControl.onHourChanged.AddListener( gameManager.OnHourChanged );

      // New Planet
      gameManager.OnCommandRcvd( CommandFactory.Create( UICommand.Id.NewPlanet ) );

      // Adjust UI
      gameManager.OnCommandRcvd( CommandFactory.Create( Id.ActionUI , ActionUICommand.SubType.Enable                                      ) );

      gameManager.OnCommandRcvd( CommandFactory.Create( Id.TimeUI   , TimeUICommand  .SubType.Enable                                      ) );
      gameManager.OnCommandRcvd( CommandFactory.Create( Id.TimeUI   , TimeUICommand  .SubType.UpdateTime   , timeControl.GetCurrentDate() ) );

      gameManager.OnCommandRcvd( CommandFactory.Create( Id.TopUI    , TopUICommand   .SubType.StatePlaying                                ) );
    }
  }
}