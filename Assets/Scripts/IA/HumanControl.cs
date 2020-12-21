using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

namespace KT
{
  public class HumanControl : ActorControl, IHungry
  {
    [HideInInspector] public class HumanClickEvent       : UnityEvent<HumanControl> { }
    [HideInInspector] public class HumanDataChangedEvent : UnityEvent<HumanControl> { }
    [HideInInspector] public class HumanDeathEvent       : UnityEvent<HumanControl> { }

    public HumanClickEvent       onHumanClick      = new HumanClickEvent();
    public HumanDataChangedEvent onHumanDataChange = new HumanDataChangedEvent();
    public HumanDeathEvent       onHumanDeath      = new HumanDeathEvent();

    public static readonly float foodStarving = 0.15f; // % of max food.
    public static readonly float foodHungry   = 0.40f;
    public static readonly float foodNice     = 0.75f;
    public static readonly float foodFull     = 1.00f;

    float splitChance = .25f;

    [Serializable]
    public class HumanDNA
    {
      public float hue;
    }

    [Serializable]
    public struct Data
    {
      public string name;
      public Calendar.Date birthDate;
      public uint age; // Days alive.
      public float height;
      public float curFood;
      public float maxFood;
      public float hue;
    }

    public HumanDNA dna;
    public Data data;

    protected override void Awake ()
    {
      base.Awake();

      state = ( IActorState ) new ActorWander();

      meshHeight = 1.0f; //Default height.

      data.maxFood = 168;
      data.curFood = data.maxFood;
    }

    // Start is called before the first frame update
    protected override void Start ()
    {
      base.Start();
    }

    // Update is called once per frame
    protected override void Update ()
    {
      base.Update();
    }

    private void OnEnable ()
    {
      ServiceLoc.Instance.GetService<TimeControl>().onMinChanged .AddListener( OnMinChange  );
      ServiceLoc.Instance.GetService<TimeControl>().onHourChanged.AddListener( OnHourChange );
    }

    private void OnDisable ()
    {
      ServiceLoc.Instance.GetService<TimeControl>().onMinChanged .RemoveListener( OnMinChange  );
      ServiceLoc.Instance.GetService<TimeControl>().onHourChanged.RemoveListener( OnHourChange );
    }

    private void OnDestroy ()
    {
      onHumanClick.RemoveAllListeners();
    }

    public void Death ()
    {
      onHumanDeath.Invoke( this );

      Destroy( gameObject );
    }

    public override void Teleport ( Vector3 tgt )
    {
      base.Teleport( tgt );

      state = new ActorWander();
    }

    override protected void OnPointerClick ( PointerEventData eventData )
    {
      base.OnPointerClick( eventData );

      onHumanClick.Invoke( this );
    }

    public void SetBirth ( Calendar.Date birth )
    {
      data.birthDate = new Calendar.Date( birth );

      data.age = 0;

      onHumanDataChange.Invoke( this );
    }

    public void SetHue ( float _hue ) { data.hue = _hue; dna.hue = _hue; }

    void OnMinChange ( Calendar.Date date )
    {
      //Birthday check.
      if ( ( date.minute == data.birthDate.minute ) && ( date.hour == data.birthDate.hour ) )
      {
        data.age += 1;

        onHumanDataChange.Invoke( this );
      }
    }

    void OnHourChange ( Calendar.Date date )
    {
      data.curFood -= 1.5f;

      // This death is here, instead of being a command because who don't need to decide who is dying.
      if ( data.curFood < 0f )
      {
        Death();
      }
      else if ( data.curFood > foodNice * data.maxFood )
      {
        if ( UnityEngine.Random.value < splitChance )
        {
          // New human.

          HumanControl h = HumanBuilder.Create( dna ).GetComp<HumanControl>();

          h = h.Instantiate( transform.position , transform.up );

          h.data.curFood = .5f * data.curFood;

          data.curFood = .5f * data.curFood;
        }
      }

      onHumanDataChange.Invoke( this );
    }

    public void SetName ( string _name )
    {
           data.name = _name;
      transform.name = _name;

      onHumanDataChange.Invoke( this );
    }

    public override void SetHeight ( float h )
    {
      base.SetHeight( h );

      data.height = h;
    }

    bool IHungry.IsHungry ()
    {
      // Less than Nice%
      return data.curFood < ( foodNice * data.maxFood );
    }
  }
}