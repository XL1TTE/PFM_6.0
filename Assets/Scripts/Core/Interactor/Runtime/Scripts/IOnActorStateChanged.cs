using System.Collections.Generic;
using Core.Utilities;
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
            if (F.IsMonster(a_subject, a_world) == false) { return UniTask.CompletedTask; }
            G.UpdateAbilityButtonsState(a_world);
            return UniTask.CompletedTask;
        }
    }
}

