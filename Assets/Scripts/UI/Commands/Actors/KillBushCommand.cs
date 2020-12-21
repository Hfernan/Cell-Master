using UnityEngine;

namespace KT
{
  public class KillBushCommand : UICommand
  {
    public KillBushCommand ()
    {
      commandName = "Kill Bush";

      cmdId = Id.KillBush;
    }

    public override void DoCommand ( object actor )
    {
      if ( actor is BushControl bush )
      {
        ServiceLoc.Instance.GetService<GameManager>().OnCommandRcvd( CommandFactory.Create( Id.DetailUI , DetailUICommand.SubType.Clear ) );

        Object.Destroy( bush.gameObject );
      }
    }
  }
}