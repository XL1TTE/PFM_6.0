
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Domain.Abilities.Components;
using Domain.Abilities.Tags;
using Domain.BattleField.Components;
using Domain.BattleField.Events;
using Domain.BattleField.Tags;
using Domain.Components;
using Domain.Extentions;
using Domain.Monster.Tags;
using Domain.TargetSelection.Events;
using Domain.TargetSelection.Requests;
using Domain.TurnSystem.Tags;
using Domain.UI.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Gameplay.Abilities.Systems{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MoveAbilitySystem : ISystem
    {
        private int SystemID = "MOVE_ABILITY".GetHashCode();
        
        public World World { get; set; }
        
        private Filter _currentTurnTaker;
        private Filter _nonOccupiedCells;
        
        private Request<TargetSelectionRequest> req_targetSelection;
        private Event<CellOccupiedEvent> evt_cellOccupied;
        private Event<ButtonClickedEvent> evt_btnClicked;
        private Event<TargetSelectionCompletedEvent> evt_selectionCompleted;
        
        private Stash<MoveAbilityBtnTag> stash_moveAbilityBtn;
        private Stash<MovementAbility> stash_moveAbility;
        private Stash<CellPositionComponent> stash_cellPosition;
        private Stash<GridPosition> stash_gridPosition;
        private Stash<TransformRefComponent> stash_transformRef;
        
        private Entity CurrentExecuter;
        
        private Dictionary<int, Sequence> ActiveMoveTweensMap = new();

        public void OnAwake()
        {
            _currentTurnTaker = World.Filter
                .With<TagMonster>()
                .With<CurrentTurnTakerTag>()
                .Build();

            _nonOccupiedCells = World.Filter
                .With<CellTag>()
                .With<CellPositionComponent>()
                .Without<TagOccupiedCell>()
                .Build();

            req_targetSelection = World.GetRequest<TargetSelectionRequest>();
            evt_btnClicked = World.GetEvent<ButtonClickedEvent>();
            evt_selectionCompleted = World.GetEvent<TargetSelectionCompletedEvent>();
            evt_cellOccupied = World.GetEvent<CellOccupiedEvent>();

            stash_moveAbilityBtn = World.GetStash<MoveAbilityBtnTag>();
            stash_moveAbility = World.GetStash<MovementAbility>();
            stash_cellPosition = World.GetStash<CellPositionComponent>();
            stash_gridPosition = World.GetStash<GridPosition>();
            stash_transformRef = World.GetStash<TransformRefComponent>();
        }

        public void OnUpdate(float deltaTime)
        {

            // Start executing and request cell selecting
            foreach (var evt in evt_btnClicked.publishedChanges){
                if(Validate(evt.ClickedButton)){
                    CurrentExecuter = _currentTurnTaker.FirstOrDefault();
                    Execute(CurrentExecuter);
                }
            }
            
            // Waiting for response from target selection system
            foreach(var evt in evt_selectionCompleted.publishedChanges){
                if(evt.CompletedRequestID != SystemID){return;}
                
                var cell = evt.SelectedTargets.FirstOrDefault();
                
                Debug.Log($"Cell with id ({cell.Id}) was selected.");
                
                MoveToSelectedCell(cell);
                Cleanup();
            }
            
        }

        private void Cleanup()
        {
            CurrentExecuter = default;
        }

        private void MoveToSelectedCell(Entity cell)
        {
            evt_cellOccupied.NextFrame(new CellOccupiedEvent{
                OccupiedBy = CurrentExecuter,
                CellEntity = cell 
            });
            var cellPos = stash_cellPosition.Get(cell);
            ref var executerTransformRef = ref stash_transformRef.Get(CurrentExecuter).Value;
            var targetPos = new Vector3(cellPos.global_x, cellPos.global_y, cellPos.global_y * 0.01f);

            
            if(ActiveMoveTweensMap.ContainsKey(CurrentExecuter.Id)){
                ActiveMoveTweensMap[CurrentExecuter.Id].Kill(true);
                ActiveMoveTweensMap.Remove(CurrentExecuter.Id);
            }
            MoveSequence(CurrentExecuter, executerTransformRef, targetPos);
        }

        private void MoveSequence(Entity executer, Transform executerTransform,
        Vector3 targetPosition)
        {

            var raiseHeight = 20;
            
            var targetPosWithHeight = 
                new Vector3(targetPosition.x, targetPosition.y + raiseHeight, targetPosition.z);

            var originalPosition = executerTransform.position;

            Sequence seq = DOTween.Sequence();
            

            seq.Append(executerTransform.DOMoveY(originalPosition.y
                + raiseHeight, 0.25f).SetEase(Ease.OutQuad));

            seq.Append(executerTransform.DOMove(targetPosWithHeight, 0.5f)
                .SetEase(Ease.OutQuad));

            seq.Append(executerTransform.DOMoveY(targetPosition.y, 0.25f)
                .SetEase(Ease.InQuad));

            ActiveMoveTweensMap.Add(executer.Id, seq);
            seq.Play();
        }

        private void Execute(Entity executer)
        {
            Debug.Log("MOVE ABILITY");
            if (executer.IsExist()){
                var allowedCells = FindAllowedToMoveCells(executer);
                if(allowedCells.Count > 0){
                    Debug.Log("SEND TARGET SELECTION REQUEST");
                    req_targetSelection.Publish(new TargetSelectionRequest{
                        RequestID = SystemID,
                        AllowedTargets = allowedCells,
                        TargetCount = 1,
                        Type = TargetSelectionRequest.SelectionType.Cell
                    });
                }
            }
        }

        public void Dispose()
        {

        }
        
        private List<Entity> FindAllowedToMoveCells(Entity monsterEntity){
            List<Entity> allowedCells = new();
            List<Vector2Int> cellPositions = new();
            
            var c_gridPos = stash_gridPosition.Get(monsterEntity);
            var moves = stash_moveAbility.Get(monsterEntity).Movements;
            
            var gridPos = new Vector2Int(c_gridPos.grid_x, c_gridPos.grid_y);
            
            foreach(var move in moves){
                cellPositions.Add(gridPos + move);
            }
            
            // pick cells with allowed positions
            foreach(var cell in _nonOccupiedCells){
                var c_cellPos = stash_cellPosition.Get(cell);
                var cellPos = new Vector2Int(c_cellPos.grid_x, c_cellPos.grid_y);
                if(cellPositions.Contains(cellPos)){
                    allowedCells.Add(cell);
                }
            }
        
            return allowedCells;
        }
        
        private bool Validate(Entity clickedBtn){
            if(stash_moveAbilityBtn.Has(clickedBtn) == false){return false;}
            return true;
        }
    }
}


