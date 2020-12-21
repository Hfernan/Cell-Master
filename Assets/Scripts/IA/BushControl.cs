using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace KT
{
  public class BushControl : ActorControl, IEatable
  {
    [HideInInspector] public class BushClickEvent       : UnityEvent<BushControl> { }
    [HideInInspector] public class BushDataChangedEvent : UnityEvent<BushControl> { }

    public BushClickEvent       onBushClick = new BushClickEvent();
    public BushDataChangedEvent onBushDataChange = new BushDataChangedEvent();

    public Data data;

    [Serializable]
    public struct Data
    {
      public float curFood;
    }

    // Start is called before the first frame update
    protected override void Start ()
    {
      base.Start();

      name = Mathf.FloorToInt( UnityEngine.Random.value * 1000 ).ToString( "000" );

      data.curFood = 20f;
    }

    // Update is called once per frame
    protected override void Update ()
    {
      base.Update();
    }

    protected override void OnPointerClick ( PointerEventData eventData )
    {
      base.OnPointerClick( eventData );

      onBushClick.Invoke( this );
    }

    float IEatable.TakeFood ()
    {
      float food = data.curFood;

      data.curFood = 0;

      Destroy( gameObject );

      return food;
    }
  }
}