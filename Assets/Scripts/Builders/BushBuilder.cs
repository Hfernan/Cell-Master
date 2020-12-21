using UnityEngine;

namespace KT
{
  public static class BushBuilder
  {
    public static IBushTracker UILink;

    // Get the base actor from Resources and create it.
    public static GameObject Create ()
    {
      GameObject bush = Resources.Load<GameObject>( "Actors/Bush" );

      bush.GetCompOrAdd<BushControl>( out _ );

      return bush;
    }

    public static void Instantiate ( this BushControl bush , Vector3 pos , Vector3 normal , Transform parent )
    {
      // Instantiation clones and return a new GO so we cannot link human and have to fetch the new HumanControl of the clone.
      BushControl b = GameObject.Instantiate( bush.gameObject , pos , Quaternion.FromToRotation( Vector3.up , normal ) ).GetComp<BushControl>();

      b.AfterInstInit();

      b.transform.SetParent( parent );
    }

    /// <summary>
    /// Links the actor to the observer classes. Should be done after Instatiation so the cloned go is linked.
    /// </summary>
    public static void AfterInstInit ( this BushControl b )
    {
      b.onBushClick.AddListener( UILink.OnBushClick );
    }
  }
}