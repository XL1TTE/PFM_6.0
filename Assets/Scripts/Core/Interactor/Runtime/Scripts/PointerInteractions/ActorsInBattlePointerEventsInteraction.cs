
using Core.Utilities;
using Cysharp.Threading.Tasks;
using Domain.StateMachine.Components;
using Domain.StateMachine.Mono;
using Game;
using Scellecs.Morpeh;

namespace Interactions
{
    public sealed class ActorsInBattlePointerEventsInteraction
        : BaseInteraction, IOnPointerEnter, IOnPointerExit, IOnPointerClick
    {

        private bool isPined = false;

        public UniTask OnPointerClick(Entity entity, World a_world)
        {
            if (ValidateExecution(entity, a_world) == false) { return UniTask.CompletedTask; }

            G.Battle.PinHoverActorPage(GU.GetCellOccupier(entity, a_world), !G.Battle.IsHoverPagePinned());

            return UniTask.CompletedTask;
        }

        public UniTask OnPointerEnter(Entity entity, World a_world)
        {
            if (ValidateExecution(entity, a_world) == false) { return UniTask.CompletedTask; }

            if (G.Battle.IsHoverPagePinned())
            {
                G.Battle.PinHoverActorPage(GU.GetCellOccupier(entity, a_world), false);
            }

            var occupier = GU.GetCellOccupier(entity, a_world);

            G.Battle.ShowHoveredActorPageInBook();
            G.Battle.UpdateHoveredActorPageInBook(occupier, a_world);

            return UniTask.CompletedTask;
        }

        public UniTask OnPointerExit(Entity entity, World a_world)
        {
            if (ValidateExecution(entity, a_world) == false) { return UniTask.CompletedTask; }
            if (G.Battle.IsHoverPagePinned()) { return UniTask.CompletedTask; }

            G.Battle.HideHoveredActorPageInBook();
            return UniTask.CompletedTask;
        }

        private bool ValidateExecution(Entity entity, World a_world)
        {
            if (SM.IsIt<BattleState>(out _) == false) { return false; }
            if (F.IsOccupiedCell(entity, a_world) == false) { return false; }
            var occupier = GU.GetCellOccupier(entity, a_world);
            if ((F.IsEnemy(occupier, a_world) | F.IsMonster(occupier, a_world)) == false) { return false; }
            return true;
        }
    }
}
