using UnityEngine;

namespace KT
{
  public class QuitGameCommand : UICommand
  {
    public QuitGameCommand ()
    {
      commandName = "Quit Game";

      cmdId = Id.QuitGame;
    }

    public override void DoCommand ()
    {
      Application.Quit();
    }
  }
}