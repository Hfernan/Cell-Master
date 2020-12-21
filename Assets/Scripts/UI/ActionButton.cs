using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace KT
{
  [RequireComponent( typeof( Image  ) )]
  [RequireComponent( typeof( Button ) )]
  public class ActionButton : MonoBehaviour
  {
    [HideInInspector] public class ActionClickedEvent : UnityEvent<UICommand.Id> {}

    public ActionClickedEvent onActionClick = new ActionClickedEvent();

    [SerializeField] UICommand.Id actionId = UICommand.Id.None;

    [Tooltip("Seconds of cooldown")]
    [SerializeField] float coolDown = 0f;

    float coolDownLeft = 0f;

    Button actBtn;

    Image imgBtn;

    private void Start ()
    {
      actBtn = gameObject.GetComp<Button>();
      imgBtn = gameObject.GetComp<Image>();
    }

    private void Update ()
    {
      if ( !actBtn?.IsInteractable() ?? false )
      {
        coolDownLeft -= Time.deltaTime;

        UpdateFill();

        if ( coolDownLeft < 0f )
        {
          actBtn.interactable = true;

          imgBtn.fillAmount = 1f;
        }
      }
    }

    private void UpdateFill ()
    {
      if ( imgBtn != null )
      {
        imgBtn.fillAmount = Mathf.Clamp01( 1 - coolDownLeft / coolDown );
      }
    }

    private void OnDestroy ()
    {
      onActionClick.RemoveAllListeners();
    }

    public void OnClick ()
    {
      if ( actBtn?.IsInteractable() ?? false )
      {
        coolDownLeft = coolDown;

        actBtn.interactable = false;

        onActionClick.Invoke( actionId );
      }
    }
  }
}