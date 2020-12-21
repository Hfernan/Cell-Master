using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KT
{
  // Search for a food source.
  public class ActorSearchFood : IActorState
  {
    float nextWanderTime = 1f;
    float wanderStepTime = 3f;

    float nextSearchTime = 2f;
    float searchStepTime = 5f;

    float searchRadius = 10f;

    Collider[] hits = new Collider[10];

    public void OnStart ( ActorControl h )
    {
      nextWanderTime = Time.time + wanderStepTime;
      nextSearchTime = Time.time + searchStepTime;
    }

    IActorState IActorState.OnUpdate ( ActorControl human )
    {
      IActorState nextState = null;

      if ( nextWanderTime < Time.time )
      {
        nextWanderTime = Time.time + wanderStepTime;

        Walk( human );
      }

      if ( nextSearchTime < Time.time )
      {
        nextSearchTime = Time.time + searchStepTime;

        ActorControl tgt = SearchFood( human );

        if ( tgt != null )
        {
          nextState = new HumanGrabFood( tgt );
        }
      }

      return nextState;
    }

    private ActorControl SearchFood ( ActorControl actor )
    {
      ActorControl tgt = null;

      int cnt = Physics.OverlapSphereNonAlloc( actor.transform.position , searchRadius , hits , GVar.ActorsLayer , QueryTriggerInteraction.Collide );

      if ( cnt > 0 )
      {
        for ( int i = 0 ; ( i < cnt && tgt == null ) ; ++i )
        {
          if ( hits[i].CompareTag( "FoodPlant" ) )
          {
            tgt = hits[i].GetComponent<ActorControl>();
          }
        }
      }

      return tgt;
    }

    void Walk ( ActorControl actor )
    {
      Vector3 dest = ServiceLoc.Instance.GetService<PlanetControl>().RandNearOnPlaneSurface( actor.transform.position , /* distance */ 20f , out _ , actor.GetHeight() );

      if ( dest != Vector3.zero )
      {
        actor.MoveTo( dest );
      }
    }
  }
}