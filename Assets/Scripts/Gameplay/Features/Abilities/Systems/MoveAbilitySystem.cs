
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Domain.Abilities.Components;
using Domain.Abilities.Tags;
using Domain.BattleField.Components;
using Domain.BattleField.Events;
using Domain.BattleField.Tags;
using Domain.Components;
using Domain.CursorDetection.Components;
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
        
        
        private Filter f_moveAbiltiyBtns;
        private Filter f_currentTurnTaker;
        private Filter f_cells;
        
        private Request<TargetSelectionRequest> req_targetSelection;
        private Event<CellOccupiedEvent> evt_cellOccupied;
        private Event<ButtonClickedEvent> evt_btnClicked;
        private Event<TargetSelectionCompletedEvent> evt_selectionCompleted;
        private Event<TargetSelectionCanceledEvent> evt_selectionCanceled;
        
        private Stash<MoveAbilityBtnTag> stash_moveAbilityBtn;
        private Stash<MovementAbility> stash_moveAbility;
        private Stash<Moving> stash_movingTag;
        private Stash<TagCursorDetector> stash_cursorDetector;
        private Stash<PerfomingActionTag> stash_performingAction;
        private Stash<CellPositionComponent> stash_cellPosition;
        private Stash<TagOccupiedCell> stash_cellOccupied;
        private Stash<GridPosition> stash_gridPosition;
        private Stash<TransformRefComponent> stash_transformRef;
        private Stash<UnderCursorComponent> stash_underCursor;
        
        private Entity CurrentCaster;
        private Entity CurrentMoveButton;
        
        private Dictionary<int, Sequence> ActiveMoveTweensMap = new();
        
        private bool IsAbilityAwaible = false;

        public void OnAwake()
        {
            f_moveAbiltiyBtns = World.Filter
                .With<MoveAbilityBtnTag>()
                .Build();

            f_currentTurnTaker = World.Filter
                .With<TagMonster>()
                .With<CurrentTurnTakerTag>()
                .Build();

            f_cells = World.Filter
                .With<CellTag>()
                .With<CellPositionComponent>()
                .Build();

            req_targetSelection = World.GetRequest<TargetSelectionRequest>();
            evt_btnClicked = World.GetEvent<ButtonClickedEvent>();
            evt_selectionCompleted = World.GetEvent<TargetSelectionCompletedEvent>();
            evt_selectionCanceled = World.GetEvent<TargetSelectionCanceledEvent>();
            evt_cellOccupied = World.GetEvent<CellOccupiedEvent>();

            stash_moveAbilityBtn = World.GetStash<MoveAbilityBtnTag>();
            stash_moveAbility = World.GetStash<MovementAbility>();
            stash_movingTag = World.GetStash<Moving>();
            stash_cursorDetector = World.GetStash<TagCursorDetector>();
            stash_performingAction = World.GetStash<PerfomingActionTag>();
            stash_cellOccupied = World.GetStash<TagOccupiedCell>();
            stash_cellPosition = World.GetStash<CellPositionComponent>();
            stash_gridPosition = World.GetStash<GridPosition>();
            stash_transformRef = World.GetStash<TransformRefComponent>();
            stash_underCursor = World.GetStash<UnderCursorComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            // Validation
            var _currentUsability = ValidateSkillUsability();
            if (_currentUsability != IsAbilityAwaible)
            {
                IsAbilityAwaible = _currentUsability;
                UpdateSkillView(_currentUsability);
            }
            
            #region Abiltiy Logic
            
            // Move ability use entry point
            foreach (var evt in evt_btnClicked.publishedChanges)
            {
                if (IsMoveAbilityButtonClicked(evt.ClickedButton) == false){continue;}
                
                CurrentCaster = f_currentTurnTaker.FirstOrDefault();
                CurrentMoveButton = evt.ClickedButton;
                
                var MoveOptions = GetMoveOptions(CurrentCaster);
                if (ValidateMoveOptions(MoveOptions) == false){continue;}
                
                StartMoveOptionSelection(MoveOptions);
                EnableAbilityButtonSelectedView(evt.ClickedButton);
            }

            // When player succsefuly selected targets
            foreach (var evt in evt_selectionCompleted.publishedChanges)
            {
                if (evt.CompletedRequestID != SystemID) { continue; }

                var SelectedCell = evt.SelectedTargets.FirstOrDefault();

                DisableAbilityButtonSelectedView(CurrentMoveButton); // We do this here, because we want to remove skill highlight before skill starts executing.
                MoveCasterToSelectedCell(SelectedCell);
            }
            
            // When player cancels target selection
            foreach (var evt in evt_selectionCanceled.publishedChanges)
            {
                if (evt.CanceledRequestID != SystemID) { continue; }
                OnSkillUseCanceled();
            }
            #endregion

            #region Visuals

            ProcessSkillButtonHover();

            #endregion

        }


        public void Dispose()
        {

        }

        private void ProcessSkillButtonHover(){
            foreach (var e in f_moveAbiltiyBtns){
                ref var btn = ref stash_moveAbilityBtn.Get(e);
                if (stash_underCursor.Has(e))
                {
                    btn.View.EnableHoverView();
                }
                else
                {
                    btn.View.DisableHoverView();
                }
            }
        }

        private void UpdateSkillView(bool isUsable){            
            if(isUsable){
                EnableSkillViewUsability();
            }
            else{ DisableSkillViewUsability(); }
        }

        private void EnableSkillViewUsability(){
            foreach (var btn in f_moveAbiltiyBtns)
            {
                stash_moveAbilityBtn.Get(btn).View.DisableUnavaibleView();

                if (stash_cursorDetector.Has(btn) == false)
                {
                    stash_cursorDetector.Add(btn);
                }
            }
        }
        private void DisableSkillViewUsability(){
            foreach (var btn in f_moveAbiltiyBtns)
            {
                stash_moveAbilityBtn.Get(btn).View.EnableUnavaibleView();
                stash_cursorDetector.Remove(btn);
            }
        }

        private bool ValidateSkillUsability(){
            if (f_currentTurnTaker.IsEmpty()) {return false;}
            
            var user = f_currentTurnTaker.FirstOrDefault();
            if (user.IsExist() == false) { return false; }

            if (stash_performingAction.Has(user)){return false;}
                
            return true;
        }

        private void AddMovingTagToCaster(Entity user){
            stash_movingTag.Add(user);
        }
        
        private void RemoveMovingTagFromCaster(Entity user){
            if(stash_movingTag.Has(user)){
                stash_movingTag.Remove(user);
            }
        }


        private void StartMoveOptionSelection(List<Entity> cellOptions){
            var allowedCells = FilterAllowedOptions(cellOptions);
            var forbiddenCells = FilterForbiddenOptions(cellOptions);

            SendTargetSelectionRequest(allowedCells, forbiddenCells);
        }

        private void ResetSkillState(){
            CurrentCaster = default;
            CurrentMoveButton = default;
        }
        
        private void OnSkillUseCanceled(){
            DisableAbilityButtonSelectedView(CurrentMoveButton);
            ResetSkillState();
        }
        
        private void OnSkillUseCompleted(){
            RemoveMovingTagFromCaster(CurrentCaster);
            ResetSkillState();
        }

        private void EnableAbilityButtonSelectedView(Entity button){
            if(stash_moveAbilityBtn.Has(button)==false){return;}
            
            stash_moveAbilityBtn.Get(button).View.EnableSelectedView();
        }
        private void DisableAbilityButtonSelectedView(Entity button){
            if(stash_moveAbilityBtn.Has(button)==false){return;}
            
            stash_moveAbilityBtn.Get(button).View.DisableSelectiedView();
        }

        private void SendTargetSelectionRequest(List<Entity> allowedCells, List<Entity> forbiddenCells)
        {
            req_targetSelection.Publish(new TargetSelectionRequest
            {
                RequestID = SystemID,
                AllowedTargets = allowedCells,
                ForbiddenTargets = forbiddenCells,
                TargetCount = 1,
                Type = TargetSelectionRequest.SelectionType.Cell
            });
        }

        private void MoveCasterToSelectedCell(Entity cell)
        {
            evt_cellOccupied.NextFrame(new CellOccupiedEvent{
                OccupiedBy = CurrentCaster,
                CellEntity = cell 
            });
            var cellPos = stash_cellPosition.Get(cell);
            ref var executerTransformRef = ref stash_transformRef.Get(CurrentCaster).Value;
            var targetPos = new Vector3(cellPos.global_x, cellPos.global_y, cellPos.global_y * 0.01f);

            
            if(ActiveMoveTweensMap.ContainsKey(CurrentCaster.Id)){
                ActiveMoveTweensMap[CurrentCaster.Id].Kill(true);
                ActiveMoveTweensMap.Remove(CurrentCaster.Id);
            }
            var moveSequence = GetMoveSequence(executerTransformRef, targetPos);
            
            AddMovingTagToCaster(CurrentCaster);
            
            ActiveMoveTweensMap.Add(CurrentCaster.Id, moveSequence);
            moveSequence.OnComplete(
                () => OnSkillUseCompleted()
            );
        }

        private Sequence GetMoveSequence(Transform executerTransform, Vector3 targetPosition)
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
            
            return seq;
        }

        #region MoveOptionsFilters
        private List<Entity> GetMoveOptions(Entity monsterEntity)
        {
            List<Entity> allowedCells = new();
            List<Vector2Int> cellPositions = new();

            var c_gridPos = stash_gridPosition.Get(monsterEntity);
            var moves = stash_moveAbility.Get(monsterEntity).Movements;

            var gridPos = new Vector2Int(c_gridPos.grid_x, c_gridPos.grid_y);

            foreach (var move in moves)
            {
                cellPositions.Add(gridPos + move);
            }

            // pick cells with allowed positions
            foreach (var cell in f_cells)
            {
                var c_cellPos = stash_cellPosition.Get(cell);
                var cellPos = new Vector2Int(c_cellPos.grid_x, c_cellPos.grid_y);
                if (cellPositions.Contains(cellPos))
                {
                    allowedCells.Add(cell);
                }
            }

            return allowedCells;
        }
        private List<Entity> FilterAllowedOptions(List<Entity> options)
        {
            List<Entity> filter = new();
            foreach (var opt in options)
            {
                if (stash_cellOccupied.Has(opt) == false)
                {
                    filter.Add(opt);
                }
            }
            return filter;
        }
        private List<Entity> FilterForbiddenOptions(List<Entity> options)
        {
            List<Entity> filter = new();
            foreach (var opt in options)
            {
                if (stash_cellOccupied.Has(opt))
                {
                    filter.Add(opt);
                }
            }
            return filter;
        }
        #endregion

        #region Validations
        private bool ValidateMoveOptions(List<Entity> options)
        {
            if (options.Count < 1) { return false; }
            return true;
        }


        private bool IsMoveAbilityButtonClicked(Entity clickedBtn)
        {
            if (stash_moveAbilityBtn.Has(clickedBtn) == false) { return false; }
            return true;
        }
        #endregion

    }
}


