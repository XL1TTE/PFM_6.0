

using Cysharp.Threading.Tasks;
using Domain.Extentions;
using Domain.TurnSystem.Tags;
using Scellecs.Morpeh;

namespace Interactions
{
    public interface IOnTurnEndInteraction
    {
        /// <summary>
        /// Invoke for ends turn.
        /// </summary>
        /// <param name="a_turnTaker">Entity which turn is over.</param>
        /// <param name="a_world"></param>
        /// <returns></returns>
        UniTask OnTurnEnd(Entity a_turnTaker, World a_world);
    }


    public sealed class RemoveTurnTakerMark : BaseInteraction, IOnTurnEndInteraction
    {
        public UniTask OnTurnEnd(Entity a_turnTaker, World a_world)
        {
            if (a_turnTaker.isNullOrDisposed(a_world)) { return UniTask.CompletedTask; }

            var stash_turnTaker = a_world.GetStash<CurrentTurnTakerTag>();

            if (stash_turnTaker.Has(a_turnTaker))
            {
                stash_turnTaker.Remove(a_turnTaker);
            }

            return UniTask.CompletedTask;
        }
    }
}
