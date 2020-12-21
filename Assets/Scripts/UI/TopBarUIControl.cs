namespace KT
{
  public class TopBarUIControl : CommandBarUIControl
  {
    protected override void Start ()
    {
      base.Start();

      transform.GetChild( 0 )?.gameObject.SetActive( false );
      transform.GetChild( 1 )?.gameObject.SetActive( false );
      transform.GetChild( 2 )?.gameObject.SetActive( false );
      transform.GetChild( 3 )?.gameObject.SetActive( false );
    }

    protected override void OnActionClicked ( UICommand.Id id )
    {
      base.OnActionClicked( id );
    }
  }
}