using System.Linq;
using Domain.BattleField.Requests;
using Domain.BattleField.Tags;
using Domain.CursorDetection.Components;
using Domain.Extentions;
using Domain.StateMachine.Components;
using Domain.StateMachine.Events;
using Domain.StateMachine.Mono;
using Domain.UI.Mono;
using Domain.UI.Tags;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
namespace Gameplay.StateMachine.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class BattlePlanningStateExitSystem : ISystem
    {
        public World World { get; set; }

        private Event<OnStateExitEvent> evt_onStateExit;

        private Stash<BattlePlanningState> stash_state;

        public void OnAwake()
        {
            evt_onStateExit = SM.Value.GetEvent<OnStateExitEvent>();

            stash_state = SM.Value.GetStash<BattlePlanningState>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var evt in evt_onStateExit.publishedChanges)
            {
                if (IsValid(evt.StateEntity))
                {
                    Exit(evt.StateEntity);
                }
            }
        }

        public void Dispose()
        {

        }

        private void Exit(Entity stateEntity)
        {

            Filter spawnCellsFilter = World.Filter.With<TagMonsterSpawnCell>().Build();

            var highlightMonsterSpawnCellsReq = World.GetRequest<ChangeCellViewToSelectedRequest>();

            highlightMonsterSpawnCellsReq.Publish(
                    new ChangeCellViewToSelectedRequest
                    {
                        Cells = spawnCellsFilter.AsEnumerable(),
                        State = ChangeCellViewToSelectedRequest.SelectState.Disabled
                    }
                    , true);

            var req_markMonsterSpawnCellsAsDropTargets =
                World.GetRequest<MarkMonsterSpawnCellsAsDropTargetRequest>();

            req_markMonsterSpawnCellsAsDropTargets.Publish(
                new MarkMonsterSpawnCellsAsDropTargetRequest
                {
                    state = MarkMonsterSpawnCellsAsDropTargetRequest.State.Disable
                }, true);

            /* ########################################## */
            /*          Remove start battle button         */
            /* ########################################## */
            foreach (var btn in World.Filter.With<StartBattleButtonTag>().Build())
            {
                if (World.TryGetComponent<StartBattleButtonTag>(btn, out var t_btn))
                {
                    World.GetStash<TagCursorDetector>().Remove(btn); // Disable ability to click
                    t_btn.View.DestroySelf();
                }
            }
        }

        private bool IsValid(Entity stateEntity)
        {
            if (stash_state.Has(stateEntity))
            {
                return true;
            }
            return false;
        }
    }
}

