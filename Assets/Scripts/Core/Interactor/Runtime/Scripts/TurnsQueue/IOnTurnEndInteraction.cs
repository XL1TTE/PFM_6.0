

using System.Collections.Generic;
using Core.Utilities;
using Cysharp.Threading.Tasks;
using Domain.BattleField.Components;
using Domain.Extentions;
using Domain.GameEffects;
using Domain.TurnSystem.Tags;
using Game;
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

    public sealed class DisableCellPointerView : BaseInteraction, IOnTurnEndInteraction
    {
        public async UniTask OnTurnEnd(Entity a_turnTaker, World a_world)
        {
            var stash_cellView = a_world.GetStash<CellViewComponent>();
            var t_cell = GU.GetOccupiedCell(a_turnTaker, a_world);

            if (t_cell.isNullOrDisposed(a_world)) { return; }
            if (stash_cellView.Has(t_cell) == false) { return; }

            stash_cellView.Get(t_cell).m_Value.DisablePointerLayer();

            await UniTask.CompletedTask;
        }

    }
}
