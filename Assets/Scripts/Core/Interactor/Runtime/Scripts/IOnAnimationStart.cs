using System.Linq;
using Cysharp.Threading.Tasks;
using Domain.Notificator;
using Scellecs.Morpeh;

namespace Interactions
{
    public interface IOnAnimationStart
    {
        UniTask OnAnimationStart(Entity a_subject, World a_world);
    }

    public interface IOnAnimationEnd
    {
        UniTask OnAnimationEnd(Entity a_subject, World a_world);
    }

    public sealed class UpdateActorStateOnAnimating : BaseInteraction, IOnAnimationStart, IOnAnimationEnd
    {
        public async UniTask OnAnimationStart(Entity a_subject, World a_world)
        {
            var stash_actorState = a_world.GetStash<ActorActionStatesComponent>();
            var evt_actorStateChanged = a_world.GetEvent<ActorActionStatesChanged>();

            if (stash_actorState.Has(a_subject) == false) { return; }

            stash_actorState.Get(a_subject).m_Values.Add(ActorActionStates.Animating);

            evt_actorStateChanged.NextFrame(new ActorActionStatesChanged
            {
                m_Actor = a_subject,
                m_Values = stash_actorState.Get(a_subject).m_Values
            });

            await UniTask.CompletedTask;
        }
        public async UniTask OnAnimationEnd(Entity a_subject, World a_world)
        {
            var stash_actorState = a_world.GetStash<ActorActionStatesComponent>();
            var evt_actorStateChanged = a_world.GetEvent<ActorActionStatesChanged>();

            if (stash_actorState.Has(a_subject) == false) { return; }

            if (stash_actorState.Get(a_subject).m_Values.Contains(ActorActionStates.Animating))
            {
                stash_actorState.Get(a_subject).m_Values.Remove(ActorActionStates.Animating);
            }

            evt_actorStateChanged.NextFrame(new ActorActionStatesChanged
            {
                m_Actor = a_subject,
                m_Values = stash_actorState.Get(a_subject).m_Values
            });

            await UniTask.CompletedTask;
        }
    }
}

