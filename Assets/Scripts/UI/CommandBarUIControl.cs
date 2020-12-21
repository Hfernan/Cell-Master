using UnityEngine;
using UnityEngine.Events;

namespace KT
{
  // This class instantiates an array of GO, listen to their click event and send the command.
  public class CommandBarUIControl : MonoBehaviour
  {
    [HideInInspector] public class CommandClickEvent : UnityEvent<UICommand> { }

    public CommandClickEvent onCommandClicked = new CommandClickEvent();

    [SerializeField] GameObject[] actionArray;

    protected virtual void Start ()
    {
      Fill();
    }

    public void Fill ()
    {
      foreach ( GameObject go in actionArray )
      {
        ActionButton btn = Instantiate( go , this.transform ).GetComp<ActionButton>();

        btn.onActionClick.AddListener( OnActionClicked );
      }
    }

    // Receives a click event from a button identified by id.
    protected virtual void OnActionClicked ( UICommand.Id id )
    {
      //Create the command object.
      UICommand command = CommandFactory.Create( id ) ;

      //Send
      if ( command != null )
      {
        onCommandClicked.Invoke( command );
      }
    }
  }
}