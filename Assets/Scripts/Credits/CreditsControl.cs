using UnityEngine;
using UnityEngine.SceneManagement;

namespace KT
{
  public class CreditsControl : MonoBehaviour
  {
    public void OnClickExit ()
    {
      SceneManager.LoadScene( ( int ) GVar.Scene.Title );
    }
  }
}