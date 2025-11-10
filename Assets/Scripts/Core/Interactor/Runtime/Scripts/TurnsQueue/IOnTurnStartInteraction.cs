

using Cysharp.Threading.Tasks;
using Domain.Extentions;
using Domain.TurnSystem.Events;
using Domain.TurnSystem.Tags;
using Scellecs.Morpeh;

namespace Interactions
{
    public interface IOnTurnStartInteraction
    {
        /// <summary>
        /// Invoke to start turn.
        /// </summary>
        /// <param name="a_turnTaker">Entity which turn is it.</param>
        /// <param name="a_world"></param>
        /// <returns></returns>
        UniTask OnTurnStart(Entity a_turnTaker, World a_world);
    }

    public sealed class AddTurnTakerMarkInteraction : BaseInteraction, IOnTurnStartInteraction
    {
        public UniTask OnTurnStart(Entity a_turnTaker, World a_world)
        {
            if (a_turnTaker.isNullOrDisposed(a_world)) { return UniTask.CompletedTask; }

            var stash_turnTaker = a_world.GetStash<CurrentTurnTakerTag>();
            stash_turnTaker.Set(a_turnTaker);
            return UniTask.CompletedTask;
        }
    }


    public sealed class OnTurnStartECSNotifyInteraction : BaseInteraction, IOnTurnStartInteraction
    {
        public UniTask OnTurnStart(Entity a_turnTaker, World a_world)
        {
            a_world.GetEvent<NextTurnStartedEvent>().NextFrame(new NextTurnStartedEvent
            {
                m_CurrentTurnTaker = a_turnTaker
            });
            return UniTask.CompletedTask;
        }
    }

}
