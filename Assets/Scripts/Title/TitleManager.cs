using UnityEngine;
using UnityEngine.SceneManagement;

namespace KT
{
  public class TitleManager : MonoBehaviour
  {
    public void OnClickStart       (){ SceneManager.LoadScene( ( int ) GVar.Scene.Main         ); }
    public void OnClickInstruction (){ SceneManager.LoadScene( ( int ) GVar.Scene.Instructions ); }
    public void OnClickCredits     (){ SceneManager.LoadScene( ( int ) GVar.Scene.Credits      ); }

    public void OnClickQuit ()
    {
#if UNITY_EDITOR
      Debug.LogWarning( "Application wants to quit." );
#endif
      Application.Quit();
    }
  }
}