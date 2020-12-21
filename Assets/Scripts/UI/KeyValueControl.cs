using UnityEngine;
using TMPro;

namespace KT
{
  public class KeyValueControl : MonoBehaviour
  {
    [SerializeField] TextMeshProUGUI txtKey;
    [SerializeField] TextMeshProUGUI txtValue;

    public void SetKey ( string t )
    {
      txtKey.text = t + ":";
    }

    public void SetValue ( string t )
    {
      txtValue.text = t;
    }
  }
}