using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KT
{
  // Interface for classes that define actor states.
  public interface IActorState
  {
    void OnStart ( ActorControl human );

    IActorState OnUpdate ( ActorControl h );
  }
}