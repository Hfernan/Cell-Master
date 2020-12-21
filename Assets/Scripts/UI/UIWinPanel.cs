using UnityEngine;
using TMPro;

namespace KT
{
  public class UIWinPanel : MonoBehaviour
  {
    [SerializeField] TextMeshProUGUI winTxt;

    public void Show ( float time )
    {
      winTxt.text = winTxt.text.Replace( "XX" , time.ToString( "000" ) );
    }

    public void OnClick ()
    {
      Application.Quit();
    }
  }
}
