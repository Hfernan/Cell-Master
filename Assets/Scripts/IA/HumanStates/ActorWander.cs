using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KT
{
  // Aimlessly move around.
  public class ActorWander : IActorState
  {
    float nextWanderTime = 1f;
    float wanderStepTime = 3f;

    void IActorState.OnStart ( ActorControl actor ) { }

    IActorState IActorState.OnUpdate ( ActorControl actor )
    {
      IActorState nextState = null;

      if ( nextWanderTime < Time.time )
      {
        nextWanderTime = Time.time + wanderStepTime;

        Walk( actor );
      }

      if ( actor is IHungry hunger )
      {
        if ( hunger.IsHungry() )
        {
          nextState = new ActorSearchFood();
        }
      }
      else
      {
        nextState = new ActorWander();
      }

      return nextState;
    }

    void Walk ( ActorControl actor )
    {
      Vector3 dest = ServiceLoc.Instance.GetService<PlanetControl>().RandNearOnPlaneSurface( actor.transform.position , /* distance */ 10f , out _ , actor.GetHeight() );

      if ( dest != Vector3.zero )
      {
        actor.MoveTo( dest );
      }
    }
  }
}