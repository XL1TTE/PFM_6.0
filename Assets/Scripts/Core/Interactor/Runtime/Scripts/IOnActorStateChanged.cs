using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Domain.Notificator;
using Game;
using Scellecs.Morpeh;

namespace Interactions.ActorStateChanged
{
    public interface IOnActorStateChanged
    {
        UniTask OnStateChanged(Entity a_subject, List<ActorActionStates> a_states, World a_world);
    }

    public sealed class UpdateAbilityButtonsState : BaseInteraction, IOnActorStateChanged
    {
        public UniTask OnStateChanged(Entity a_subject, List<ActorActionStates> a_states, World a_world)
        {
            G.UpdateAbilityButtonsState(a_world);
            return UniTask.CompletedTask;
        }
    }
}

