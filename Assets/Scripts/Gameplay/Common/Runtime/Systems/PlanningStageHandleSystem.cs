using System.Collections;
using Gameplay.Common.Requests;
using Gameplay.Features.BattleField.Components;
using Gameplay.Features.BattleField.Requests;
using Gameplay.Features.DragAndDrop.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityUtilities;

namespace Gameplay.Common.Systems{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlanningStageHandleSystem : ISystem
    {
        public World World { get; set; }

        private Request<PlanningStageEnterRequest> _enterRequest;

        private Filter _monsterSpawnCells;
        private Stash<DropTargetComponent> stash_dropTarget;

        public void OnAwake()
        {
            _enterRequest = World.GetRequest<PlanningStageEnterRequest>();

            _monsterSpawnCells = World.Filter
                .With<TagMonsterSpawnCell>()
                .Build();

            stash_dropTarget = World.GetStash<DropTargetComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var req in _enterRequest.Consume())
            {
                EnterHandle();
            }
        }

        public void Dispose()
        {

        }

        private void EnterHandle()
        {
            RellayCoroutiner.RellayCoroutine(EnterHandleCoroutine());
        }

        private void ExitHandle()
        {

        }


        private IEnumerator EnterHandleCoroutine()
        {

            /* ############################################## */
            /*                     Notify                     */
            /* ############################################## */

            // var notifier = Object.Instantiate(UnityResources.NotifyController);
            // notifier.ShowMessage("Стадия подготовки!");
            // yield return new WaitForSeconds(1.5f);
            // notifier.HideMessage();
            yield return null;

            /* ############################################## */
            /*           Pre-spawn monsters request           */
            /* ############################################## */

            var genMonsterReq = World.Default.GetRequest<GenerateMonstersRequest>();

            genMonsterReq.Publish(new GenerateMonstersRequest
            {
                MosntersCount = 2
            }, true);

            /* ############################################## */
            /*         Hightlight monster spawn cells         */
            /* ############################################## */

            var highlightMonsterSpawnCellsReq = World.Default.GetRequest<EnableMonsterSpawnCellsHighlightRequest>();

            highlightMonsterSpawnCellsReq.Publish(new EnableMonsterSpawnCellsHighlightRequest { }, true);
            
            var req_markMonsterSpawnCellsAsDropTargets = 
                World.Default.GetRequest<MarkMonsterSpawnCellsAsDropTargetRequest>();

            req_markMonsterSpawnCellsAsDropTargets.Publish(
                new MarkMonsterSpawnCellsAsDropTargetRequest { DropRadius = 1.0f }, true);

        }
    }
}


