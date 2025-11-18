using System.Linq;
using Cysharp.Threading.Tasks;
using Domain.Notificator;
using Interactions.ActorStateChanged;
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
            if (a_world.IsDisposed) { return; }
            var stash_actorState = a_world.GetStash<ActorActionStatesComponent>();

            if (stash_actorState.Has(a_subject) == false) { return; }

            stash_actorState.Get(a_subject).m_Values.Add(ActorActionStates.Animating);

            Interactor.CallAll<IOnActorStateChanged>(
                async h => await h.OnStateChanged(a_subject, stash_actorState.Get(a_subject).m_Values, a_world)).Forget();

            await UniTask.CompletedTask;
        }
        public async UniTask OnAnimationEnd(Entity a_subject, World a_world)
        {
            if (a_world.IsDisposed) { return; }
            var stash_actorState = a_world.GetStash<ActorActionStatesComponent>();

            if (stash_actorState.Has(a_subject) == false) { return; }

            if (stash_actorState.Get(a_subject).m_Values.Contains(ActorActionStates.Animating))
            {
                stash_actorState.Get(a_subject).m_Values.Remove(ActorActionStates.Animating);
            }

            Interactor.CallAll<IOnActorStateChanged>(
                async h => await h.OnStateChanged(a_subject, stash_actorState.Get(a_subject).m_Values, a_world)).Forget();

            await UniTask.CompletedTask;
        }
    }
}

