using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace KT
{
  public class InstructionsControl : MonoBehaviour
  {
    [SerializeField] GameObject[] panels;

    [SerializeField] Button prevBtn;
    [SerializeField] TextMeshProUGUI nextBtn;

    int curPnl = 0;

    private void Start ()
    {
      for ( int i = 0, n = panels.Length ; ( i < n ) ; ++i )
      {
        panels[i].SetActive( i == 0 );
      }

      UpdateBtns();
    }

    private void UpdateBtns ()
    {
      prevBtn.interactable = ( curPnl != 0 );

      nextBtn.text = ( curPnl < panels.Length - 1 ) ? ">>>" : TextLocalizer.Get(TextLocalizer.Id.End );
    }

    public void OnClickNext ()
    {
      if ( curPnl == panels.Length - 1 )
      {
        // Last
        SceneManager.LoadScene( ( int ) GVar.Scene.Title );
      }
      else
      {
        panels[  curPnl].SetActive( false );
        panels[++curPnl].SetActive( true  );
      }

      UpdateBtns();
    }

    public void OnClickPrev ()
    {
      if ( curPnl > 0 )
      {
        panels[  curPnl].SetActive( false );
        panels[--curPnl].SetActive( true  );
      }

      UpdateBtns();
    }

    public void OnClickExit ()
    {
      SceneManager.LoadScene( ( int ) GVar.Scene.Title );
    }
  }
}