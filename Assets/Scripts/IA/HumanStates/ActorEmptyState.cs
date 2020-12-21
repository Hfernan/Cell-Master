using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KT
{
  // Empty / idle state.
  public class ActorEmptyState : IActorState
  {
    void IActorState.OnStart ( ActorControl human ){}

    IActorState IActorState.OnUpdate ( ActorControl h ) { return null; }
  }
}
