namespace KT
{
  public class KillHumanCommand : UICommand
  {
    public enum Reason
    {
      None,
      GodKilled,
      Hunger,
      Cnt,
    }

    Reason reason;

    public KillHumanCommand ( Reason r )
    {
      commandName = "Kill Human";

      cmdId = Id.KillHuman;

      reason = r;
    }

    public override void DoCommand ( object actor )
    {
      if ( actor is HumanControl human )
      {
        ServiceLoc.Instance.GetService<GameManager>().OnCommandRcvd( CommandFactory.Create( Id.DetailUI , DetailUICommand.SubType.Clear ) );

        human.Death();
      }
    }
  }
}