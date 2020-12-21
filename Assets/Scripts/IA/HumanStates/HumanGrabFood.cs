using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KT
{
  // Move close to a food source and grab/eat it.
  public class HumanGrabFood : IActorState
  {
    ActorControl target;

    float grabDist = 2f;

    public HumanGrabFood ( ActorControl actor )
    {
      target = actor;
    }

    void IActorState.OnStart ( ActorControl actor )
    {
      actor.MoveTo( ServiceLoc.Instance.GetService<PlanetControl>().SurfacePoint( target.transform.position , out _ , actor.GetHeight() ) , /* override */ true );
    }

    IActorState IActorState.OnUpdate ( ActorControl actor )
    {
      IActorState nextState = null;

      if ( target == null )
      {
        nextState = new ActorWander();
      }
      else if ( Vector3.Distance( actor.transform.position , target.transform.position ) < grabDist )
      {
        // Get food.
        if ( target is IEatable food )
        {
          if ( actor is HumanControl human )
          {
            human.data.curFood += food.TakeFood();

            human.onHumanDataChange.Invoke( human );

            nextState = new ActorWander();
          }
          else
          {
            Debug.LogError( actor.name + " is not a human." );
          }
        }
      }

      return nextState;
    }
  }
}