using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace KT
{
  public class TimeUIControl : MonoBehaviour
  {
    [SerializeField] TextMeshProUGUI timeStamp;
    [SerializeField] TextMeshProUGUI scoreStamp;

    [SerializeField] Image targetImage;

    string date_str;
    string score_str;

    private void Awake ()
    {
       date_str = ""     ;
      score_str = "00,00";
    }

    private void Start ()
    {
      UpdateText();
    }

    public void UpdateTarget ( float hue )
    {
      targetImage.color = Color.HSVToRGB( hue , 1f , 1f );
    }

    void UpdateText ()
    {
      timeStamp.text = date_str;

      scoreStamp.text = "Score: " + score_str + " Target: ";
    }

    public void UpdateTimeStamp ( string txt )
    {
      date_str = txt;

      UpdateText();
    }

    public void UpdateTimeStamp ( in Calendar.Date data )
    {
      date_str = ServiceLoc.Instance.GetService<TimeControl>().TimeString( in data ,/*fmt*/ "r" );

      UpdateText();
    }

    public void UpdateScore ( float score )
    {
      score_str = score.ToString( "00.00" );

      UpdateText();
    }
  }
}