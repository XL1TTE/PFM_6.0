using System.Collections.Generic;
using Domain.BattleField.Events;
using Domain.BattleField.Requests;
using Domain.BattleField.Tags;
using Domain.Extentions;
using Domain.TurnSystem.Events;
using Domain.TurnSystem.Tags;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.TurnSystem.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class TurnTakerCellMarkSystem : ISystem
    {
        public World World { get; set; }
        
        private Filter filter_currentTurnTaker;
        private Filter filter_occupiedCells;
        
        private Request<ChangeCellViewToPointerRequest> req_CellColorChange;
        
        private Event<NextTurnStartedEvent> evt_turnStarted;
        private Event<EntityCellPositionChangedEvent> evt_cellPosChanged;
        private Stash<TagOccupiedCell> stash_occupiedCells;
        
        private Entity PreviousTurnTaker;

        public void OnAwake()
        {
            filter_currentTurnTaker = World.Filter.With<CurrentTurnTakerTag>().Build();

            filter_occupiedCells = World.Filter.With<TagOccupiedCell>().Build();

            req_CellColorChange = World.GetRequest<ChangeCellViewToPointerRequest>();

            evt_turnStarted = World.GetEvent<NextTurnStartedEvent>();
            evt_cellPosChanged = World.GetEvent<EntityCellPositionChangedEvent>();
            stash_occupiedCells = World.GetStash<TagOccupiedCell>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach(var evt in evt_turnStarted.publishedChanges){
                if(filter_currentTurnTaker.IsEmpty()){
                    DisableHighlightUnder(PreviousTurnTaker);
                    return;
                }
                var turnTaker = filter_currentTurnTaker.First();
                HighlighCellUnder(turnTaker);
                DisableHighlightUnder(PreviousTurnTaker);
                PreviousTurnTaker = turnTaker;
            }
            
            foreach(var evt in evt_cellPosChanged.publishedChanges){
                if (filter_currentTurnTaker.IsEmpty()){return;}
                if (evt.entity.Id == filter_currentTurnTaker.First().Id){
                    SendCellHighlightRequest(evt.newCell, ChangeCellViewToPointerRequest.PointerState.Enabled);
                    SendCellHighlightRequest(evt.oldCell, ChangeCellViewToPointerRequest.PointerState.Disabled);
                }
            }
        }

        private Entity FindOccupiedCell(Entity occupier){
            foreach(var cell in filter_occupiedCells){
                if(stash_occupiedCells.Get(cell).Occupier.Id == occupier.Id){
                    return cell;
                }
            }
            return default;
        }
        
        private void SendCellHighlightRequest(Entity cell, ChangeCellViewToPointerRequest.PointerState state)
        {
            req_CellColorChange.Publish(new ChangeCellViewToPointerRequest
            {
                Cells = new List<Entity> { cell },
                State = state
            });
        }
        
        private void HighlighCellUnder(Entity entity)
        {
            var cell = FindOccupiedCell(entity);
            if(cell.IsExist()){
                SendCellHighlightRequest(cell, ChangeCellViewToPointerRequest.PointerState.Enabled);
            }
        }

        private void DisableHighlightUnder(Entity entity)
        {
            var cell = FindOccupiedCell(entity);
            if (cell.IsExist())
            {
                SendCellHighlightRequest(cell, ChangeCellViewToPointerRequest.PointerState.Disabled);

            }
        }

        public void Dispose()
        {
        }
    }
}


