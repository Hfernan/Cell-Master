using UnityEngine;

namespace KT
{
  public class GameManager : MonoBehaviour
  {
    [Tooltip("Parent to the bushes actors.")]
    [SerializeField] Transform bushParent;

    [Tooltip("Panel to show actor data.")]
    [SerializeField] DetailUIObserver detailPanel;
    [Tooltip("Panel that holds skills buttons.")]
    [SerializeField] CommandBarUIControl bottomPanel;
    [Tooltip("Panel that holds speed buttons.")]
    [SerializeField] CommandBarUIControl topPanel;
    [Tooltip("Panel that shows time and score.")]
    [SerializeField] TimeUIControl timePanel;

    [Tooltip("Panel that shows the win message.")]
    [SerializeField] UIWinPanel winPanel;

    float target_hue; // Hue that the player must get close to in order to win.

    float timeNextFood = 60f; // Time when the next food will spawn.
    float foodTimeStep = 5f;  // Gap between food spawns.

    private void Start ()
    {
      ServiceLoc.Instance.RegisterService( this );

      // Listen to UI commands.

      bottomPanel.onCommandClicked.AddListener( OnCommandRcvd );

      topPanel.onCommandClicked.AddListener( OnCommandRcvd );

      // Tell builders needed references.

      LinkBuilders();

      // Start game.
      OnCommandRcvd( CommandFactory.Create( UICommand.Id.StartGame ) );
    }

    private void Update ()
    {
      // Speed keyboard shortcuts.
      if ( Input.GetKeyDown( KeyCode.Alpha1 ) ) { OnCommandRcvd( CommandFactory.Create( UICommand.Id.SpeedHalf   ) ); }
      if ( Input.GetKeyDown( KeyCode.Alpha2 ) ) { OnCommandRcvd( CommandFactory.Create( UICommand.Id.SpeedNormal ) ); }
      if ( Input.GetKeyDown( KeyCode.Alpha3 ) ) { OnCommandRcvd( CommandFactory.Create( UICommand.Id.SpeedDouble ) ); }
      if ( Input.GetKeyDown( KeyCode.Alpha4 ) ) { OnCommandRcvd( CommandFactory.Create( UICommand.Id.SpeedTriple ) ); }
      if ( Input.GetKeyDown( KeyCode.Alpha0 ) ) { OnCommandRcvd( CommandFactory.Create( UICommand.Id.SpeedPause  ) ); }

      if ( Input.GetKeyDown( KeyCode.Escape ) ) { Application.Quit(); }

      // Food creation.

      if ( Time.time > timeNextFood )
      {
        OnCommandRcvd( CommandFactory.Create( UICommand.Id.BornRndBush ) );

        timeNextFood = Time.time + foodTimeStep;
      }
    }

    private void OnDestroy ()
    {
      bottomPanel.onCommandClicked.RemoveListener( OnCommandRcvd );
         topPanel.onCommandClicked.RemoveListener( OnCommandRcvd );
    }

    public float GetTargetHue () { return target_hue; }

    /// <summary>
    /// Things to be decided when the game starts.
    /// </summary>
    public void InitialDecisions ()
    {
      target_hue = UnityEngine.Random.value;

      timePanel.UpdateTarget( target_hue );
    }

    public void OnMinChanged ( Calendar.Date date )
    {
      // Update time UI.
      OnCommandRcvd( CommandFactory.Create( UICommand.Id.TimeUI , TimeUICommand.SubType.UpdateTime , date ) );
    }

    public void OnHourChanged ( Calendar.Date date )
    {
      // Check scores.
      float score = ServiceLoc.Instance.GetService<HumanityControl>().CalcScore();

      timePanel.UpdateScore( 100 * score );

      if ( score > 0.95f )
      {
        winPanel.gameObject.SetActive( true );

        // An approximation. Valid as long as game starts 1st min of 1st year and hours contain 60 mins.
        uint defHourPerDay = ServiceLoc.Instance.GetService<TimeControl>()?.GetDefMinInHour( Calendar.MiniDate.Hour( date.hour, date.day , date.month, date.year ) ) ?? 60u;

        winPanel.Show( date.unixTime / defHourPerDay );

        // Pause on win.
        OnCommandRcvd( CommandFactory.Create( UICommand.Id.SpeedPause ) );
      }
    }

    public void OnCommandRcvd ( UICommand cmd )
    {
      UICommand.Id cmdId = cmd.GetId();

#if UNITY_EDITOR && false
      Debug.Log( "Got cmd " + cmdId );
#endif

      switch ( cmdId )
      {
        case UICommand.Id.NewPlanet:
          {
            cmd.DoCommand();
            break;
          }
        case UICommand.Id.KillHuman:
          {
            //Who? The last clicked.
            if ( detailPanel.observedActor != null )
            {
              cmd.DoCommand( detailPanel.observedActor );
            }
            break;
          }
        case UICommand.Id.BornHuman:
          {
            cmd.DoCommand();
            break;
          }
        case UICommand.Id.CloneHuman:
          {
            cmd.DoCommand();
          }
          break;
        case UICommand.Id.TeleportHuman:
          {
            if ( detailPanel.observedActor != null )
            {
              cmd.DoCommand( detailPanel.observedActor );
            }
            break;
          }
        case UICommand.Id.TopUI:
          {
            cmd.DoCommand( topPanel.gameObject );
            break;
          }
        case UICommand.Id.StartGame:
          {
            cmd.DoCommand( this.gameObject );
            break;
          }
        case UICommand.Id.ActionUI:
          {
            cmd.DoCommand( bottomPanel.gameObject );
            break;
          }
        case UICommand.Id.TimeUI:
          {
            cmd.DoCommand( timePanel );
            break;
          }
        case UICommand.Id.KillBush:
          {
            //Who? The last clicked.
            if ( detailPanel.observedActor != null )
            {
              cmd.DoCommand( detailPanel.observedActor );
            }
            break;
          }
        case UICommand.Id.BornRndBush:
          {
            cmd.DoCommand( bushParent );
            break;
          }
        case UICommand.Id.BornBush:
          {
            cmd.DoCommand( bushParent );
            break;
          }
        case UICommand.Id.DetailUI:
          {
            cmd.DoCommand( detailPanel.gameObject );
            break;
          }
        case UICommand.Id.SpeedHalf:
        case UICommand.Id.SpeedNormal:
        case UICommand.Id.SpeedDouble:
        case UICommand.Id.SpeedTriple:
          {
            cmd.DoCommand();
            break;
          }
        case UICommand.Id.QuitGame:
          {
            cmd.DoCommand();
            break;
          }
        case UICommand.Id.None:
        case UICommand.Id.Cnt:
        default:
          {
            Debug.LogWarning( "Unknown command received " + cmdId );
            break;
          }
      }
    }

    void LinkBuilders ()
    {
      // Link Builders with needed references to observers.
      BushBuilder.UILink = detailPanel;
      HumanBuilder.UILink = detailPanel;
      HumanBuilder.CamLink = Camera.main.GetComponent<CamPlaneMovement>();
    }
  }
}