using System.Linq;
using Domain.BattleField.Requests;
using Domain.BattleField.Tags;
using Domain.Extentions;
using Domain.StateMachine.Components;
using Domain.StateMachine.Events;
using Domain.StateMachine.Mono;
using Domain.UI.Mono;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
namespace Gameplay.StateMachine.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class BattlePlanningStateEnterSystem : ISystem
    {
        public World World { get; set; }

        private Event<OnStateEnterEvent> evt_onStateEnter;

        private Stash<BattlePlanningState> stash_state;

        public void OnAwake()
        {
            evt_onStateEnter = SM.Value.GetEvent<OnStateEnterEvent>();

            stash_state = SM.Value.GetStash<BattlePlanningState>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var evt in evt_onStateEnter.publishedChanges)
            {
                if (IsValid(evt.StateEntity))
                {
                    Enter(evt.StateEntity);
                }
            }
        }

        public void Dispose()
        {

        }

        private void Enter(Entity stateEntity)
        {

            /* ########################################## */
            /*              Change plate text             */
            /* ########################################## */

            BattleUiRefs.Instance.InformationBoardWidget.ChangeText("Preparation");


            /* ########################################## */
            /*          Spawn start battle button         */
            /* ########################################## */

            BattleUiRefs.Instance.BookWidget.SpawnStartBattleButton();

            /* ############################################## */
            /*         Hightlight monster spawn cells         */
            /* ############################################## */

            Filter spawnCellsFilter = World.Filter.With<TagMonsterSpawnCell>().Build();

            var highlightMonsterSpawnCellsReq = World.GetRequest<ChangeCellViewToSelectedRequest>();

            highlightMonsterSpawnCellsReq.Publish(
                    new ChangeCellViewToSelectedRequest
                    {
                        Cells = spawnCellsFilter.AsEnumerable(),
                        State = ChangeCellViewToSelectedRequest.SelectState.Enabled
                    }
                    , true);

            var req_markMonsterSpawnCellsAsDropTargets =
                World.GetRequest<MarkMonsterSpawnCellsAsDropTargetRequest>();

            req_markMonsterSpawnCellsAsDropTargets.Publish(
                new MarkMonsterSpawnCellsAsDropTargetRequest
                {
                    state = MarkMonsterSpawnCellsAsDropTargetRequest.State.Enable
                }, true);
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

