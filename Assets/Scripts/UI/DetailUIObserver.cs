using System;
using System.Reflection;
using TMPro;
using UnityEngine;

namespace KT
{
  public class DetailUIObserver : MonoBehaviour, IHumanTracker, IBushTracker
  {
    [SerializeField] GameObject KVTxtBox;

    public ActorControl observedActor;

    KeyValueControl kvName   = null;
    KeyValueControl kvAge    = null;
    KeyValueControl kvHunger = null;
    KeyValueControl kvColor  = null;

    private void Start ()
    {
      kvName   = InstKVBox( TextLocalizer.Get( TextLocalizer.Id.DetName   ) );
      kvAge    = InstKVBox( TextLocalizer.Get( TextLocalizer.Id.DetAge    ) );
      kvHunger = InstKVBox( TextLocalizer.Get( TextLocalizer.Id.DetHunger ) );
      kvColor  = InstKVBox( TextLocalizer.Get( TextLocalizer.Id.DetColor  ) );
    }

    void ShowType<T> ( T data )
    {
      Type t = typeof(T);

      FieldInfo[] myField = t.GetFields();

      // Instantiate a UI prefab for each of the possible fields.

      for ( int i = 0, n = myField.Length ; ( i < n ) ; ++i )
      {
        Type fType = myField[i].FieldType;

             if ( fType == typeof(    int ) ){ InstKVBox( myField[i].Name , ( (    int ) myField[i].GetValue( data ) ).ToString(   "00" ) );}
        else if ( fType == typeof(   uint ) ){ InstKVBox( myField[i].Name , ( (   uint ) myField[i].GetValue( data ) ).ToString(   "00" ) );}
        else if ( fType == typeof(  float ) ){ InstKVBox( myField[i].Name , ( (  float ) myField[i].GetValue( data ) ).ToString( "0.00" ) );}
        else if ( fType == typeof( string ) ){ InstKVBox( myField[i].Name ,   ( string ) myField[i].GetValue( data ) );}
        else if ( fType == typeof( Calendar.Date ) )
        {
          Calendar.Date date = (Calendar.Date ) myField[i].GetValue( data );

          string txt = ServiceLoc.Instance.GetService<TimeControl>().TimeString( in date , "d-m-y" );

          InstKVBox( myField[i].Name , txt );
        }
        else
        {
          Debug.LogWarning( fType + " is not supported by UI Observer" );
        }
      }
      UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate( ( RectTransform ) transform );
    }

    private void ShowHuman ( HumanControl.Data data )
    {
      kvName  .SetValue( data.name                   );
      kvAge   .SetValue( data.age.ToString( "0" )    );
      kvHunger.SetValue( GetFoodString( in data )    );
      kvColor .SetValue( data.hue.ToString( "0.00" ) );
    }

    private void ShowBush ( BushControl bush )
    {
      Clean();

      kvName.SetValue( "Bush" );
    }

    private string GetFoodString ( in HumanControl.Data data )
    {
      string txt = "";

      float ratio = data.curFood / data.maxFood;

           if ( ratio < HumanControl.foodStarving ) txt = TextLocalizer.Get( TextLocalizer.Id.DetStarving );
      else if ( ratio < HumanControl.foodHungry   ) txt = TextLocalizer.Get( TextLocalizer.Id.DetHungry   );
      else if ( ratio < HumanControl.foodNice     ) txt = TextLocalizer.Get( TextLocalizer.Id.DetOK       );
      else txt = TextLocalizer.Get( TextLocalizer.Id.DetFull );

      return txt;
    }

    public void OnHumanClick ( HumanControl human )
    {
      // Remove previous human listeners and observe the clicked human.
      RemoveActorListeners();

      observedActor = human;

      AddActorListeners();

      ShowHuman( human.data );
    }

    private void RemoveActorListeners ()
    {
      if ( observedActor is HumanControl human )
      {
        human.onHumanDataChange.RemoveListener( OnHumanDataChange );
        human.onHumanDeath.RemoveListener( OnHumanDeath );
      }
    }

    private void AddActorListeners ()
    {
      if ( observedActor is HumanControl human )
      {
        human.onHumanDataChange.AddListener( OnHumanDataChange );
        human.onHumanDeath.AddListener( OnHumanDeath );
      }
    }

    public void OnHumanDataChange ( HumanControl human )
    {
      if ( human == observedActor )
      {
        OnHumanClick( human );
      }
    }

    public void OnHumanDeath ( HumanControl h )
    {
      if ( h == observedActor )
      {
        Clean();

        observedActor = null;
      }
    }

    public void OnBushClick ( BushControl bush )
    {
      // Clear and refresh UI.
      Clean();

      observedActor = bush;

      ShowBush( bush );
    }

    KeyValueControl InstKVBox ( string key , string value = " " )
    {
      KeyValueControl kv = Instantiate( KVTxtBox , transform ).GetComp<KeyValueControl>();

      kv.SetKey( key.UFirstCharToUpper() );
      kv.SetValue( value.UFirstCharToUpper() );

      return kv;
    }

    public void Clean ()
    {
      kvName  .SetValue( " " );
      kvAge   .SetValue( " " );
      kvHunger.SetValue( " " );
      kvColor .SetValue( " " );
    }
  }
}