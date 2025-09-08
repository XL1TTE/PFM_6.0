using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Domain.Abilities.Components;
using Domain.Abilities.Tags;
using Domain.BattleField.Components;
using Domain.BattleField.Tags;
using Domain.Components;
using Domain.CursorDetection.Components;
using Domain.Enemies.Tags;
using Domain.Extentions;
using Domain.Monster.Tags;
using Domain.Stats.Components;
using Domain.TargetSelection.Events;
using Domain.TargetSelection.Requests;
using Domain.TurnSystem.Tags;
using Domain.UI.Requests;
using NUnit.Framework;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Abilities.Systems{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AttackAbilitySystem : ISystem
    {
        private int SystemID = "ATTACK_ABILITY".GetHashCode();

        public World World { get; set; }
        
        private Filter f_attackAbiltiyBtns;
        private Filter f_currentTurnTaker;
        private Filter f_Cells;
        
        private Request<TargetSelectionRequest> req_targetSelection;

        private Event<ButtonClickedEvent> evt_ButtonClicked;
        private Event<TargetSelectionCompletedEvent> evt_selectionCompleted;
        private Event<TargetSelectionCanceledEvent> evt_selectionCanceled;

        private Stash<AttackAbilityBtnTag> stash_attackAbilityBtn;
        private Stash<AttackAbility> stash_attackAbility;
        private Stash<Attacking> stash_attackingTag;
        private Stash<PerfomingActionTag> stash_performingAction;
        private Stash<CellPositionComponent> stash_cellPosition;
        private Stash<GridPosition> stash_gridPosition;
        private Stash<TagOccupiedCell> stash_occupiedCells;
        private Stash<Health> stash_health;
        private Stash<TagEnemy> stash_enemyTag;
        private Stash<TransformRefComponent> stash_transformRef;
        private Stash<UnderCursorComponent> stash_underCursor;
        private Stash<TagCursorDetector> stash_cursorDetector;
        private Entity CurrentCaster;
        private Entity CurrentAttackButton;


        private Dictionary<int, Sequence> AttackSequenceMap = new();

        private bool IsAbilityAwaible = false;

        public void OnAwake()
        {
            f_attackAbiltiyBtns = World.Filter
                .With<AttackAbilityBtnTag>()
                .Build();
            f_currentTurnTaker = World.Filter
                .With<TagMonster>()
                .With<CurrentTurnTakerTag>()
                .Build();

            f_Cells = World.Filter 
                .With<CellTag>()
                .With<CellPositionComponent>()
                .Build();

            req_targetSelection = World.GetRequest<TargetSelectionRequest>();


            evt_ButtonClicked = World.GetEvent<ButtonClickedEvent>();
            evt_selectionCompleted = World.GetEvent<TargetSelectionCompletedEvent>();
            evt_selectionCanceled = World.GetEvent<TargetSelectionCanceledEvent>();

            stash_attackAbilityBtn = World.GetStash<AttackAbilityBtnTag>();
            stash_attackAbility = World.GetStash<AttackAbility>();
            stash_performingAction = World.GetStash<PerfomingActionTag>();
            stash_attackingTag = World.GetStash<Attacking>();
            stash_cellPosition = World.GetStash<CellPositionComponent>();
            stash_gridPosition = World.GetStash<GridPosition>();
            stash_occupiedCells = World.GetStash<TagOccupiedCell>();
            stash_health = World.GetStash<Health>();
            stash_enemyTag = World.GetStash<TagEnemy>();
            stash_transformRef = World.GetStash<TransformRefComponent>();
            stash_underCursor = World.GetStash<UnderCursorComponent>();
            stash_cursorDetector = World.GetStash<TagCursorDetector>();
        }

        public void OnUpdate(float deltaTime)
        {

            var _currentUsability = ValidateSkillUsability();
            if (_currentUsability != IsAbilityAwaible)
            {
                IsAbilityAwaible = _currentUsability;
                UpdateSkillView(_currentUsability);
            }

            #region Ability Logic
            // Attack ability use entry point
            foreach (var evt in evt_ButtonClicked.publishedChanges)
            {
                if (IsAttackAbilityButtonClicked(evt) == false){continue; }
                {
                    CurrentCaster = f_currentTurnTaker.FirstOrDefault();
                    CurrentAttackButton = evt.ClickedButton;
                    var AttackOptions = GetAttackOptions(CurrentCaster);
                    
                    if(ValidateAttackOptions(AttackOptions) == false){continue;}

                    StartAttackOptionSelection(AttackOptions);
                    EnableAbilityButtonSelectedView(evt.ClickedButton);
                }
            }
            // When player succsefuly selected targets
            foreach (var evt in evt_selectionCompleted.publishedChanges)
            {
                if (evt.CompletedRequestID != SystemID) { continue; }

                var SelectedCell = evt.SelectedTargets.FirstOrDefault();

                DisableAbilityButtonSelectedView(CurrentAttackButton); // We do this here, because we want to remove skill highlight before skill starts executing.
                AttackTargetOnCell(SelectedCell);
            }

            // When player cancels target selection
            foreach (var evt in evt_selectionCanceled.publishedChanges)
            {
                if (evt.CanceledRequestID != SystemID) { continue; }
                OnSkillUseCanceled();
            }

            #endregion

            ProcessSkillButtonHover();

        }

        public void Dispose()
        {

        }

        private void ProcessSkillButtonHover()
        {
            foreach (var e in f_attackAbiltiyBtns)
            {
                ref var btn = ref stash_attackAbilityBtn.Get(e);
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
        
        private void AddAttackingTagToCaster(Entity user)
        {
            stash_attackingTag.Add(user);
        }

        private void RemoveAttackingTagFromCaster(Entity user)
        {
            if (stash_attackingTag.Has(user))
            {
                stash_attackingTag.Remove(user);
            }
        }

        private void ResetSkillState()
        {
            CurrentCaster = default;
            CurrentAttackButton = default;
        }

        private void OnSkillUseCanceled()
        {
            DisableAbilityButtonSelectedView(CurrentAttackButton);
            ResetSkillState();
        }

        private void OnSkillUseCompleted()
        {
            RemoveAttackingTagFromCaster(CurrentCaster);
            ResetSkillState();
        }

        private void StartAttackOptionSelection(List<Entity> cellOptions)
        {
            var allowedCells = FilterAlowedOptions(cellOptions);
            var forbiddenCells = FilterForbiddenOptions(cellOptions);

            SendTargetSelectionRequest(allowedCells, forbiddenCells);
        }

        private void UpdateSkillView(bool isUsable)
        {
            if (isUsable)
            {
                EnableSkillViewUsability();
            }
            else { DisableSkillViewUsability(); }
        }


        private void EnableSkillViewUsability()
        {
            foreach (var btn in f_attackAbiltiyBtns)
            {
                stash_attackAbilityBtn.Get(btn).View.DisableUnavaibleView();

                if (stash_cursorDetector.Has(btn) == false)
                {
                    stash_cursorDetector.Add(btn);
                }
            }
        }
        private void DisableSkillViewUsability()
        {
            foreach (var btn in f_attackAbiltiyBtns)
            {
                stash_attackAbilityBtn.Get(btn).View.EnableUnavaibleView();
                stash_cursorDetector.Remove(btn);
            }
        }

        private void EnableAbilityButtonSelectedView(Entity button)
        {
            if (stash_attackAbilityBtn.Has(button) == false) { return; }

            stash_attackAbilityBtn.Get(button).View.EnableSelectedView();
        }
        private void DisableAbilityButtonSelectedView(Entity button)
        {
            if (stash_attackAbilityBtn.Has(button) == false) { return; }

            stash_attackAbilityBtn.Get(button).View.DisableSelectiedView();
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

        private void AttackTargetOnCell(Entity cell){

            var target = stash_occupiedCells.Get(cell).Occupier;

            ref var executerTransform = ref stash_transformRef.Get(CurrentCaster).Value;
            ref var targetTransform = ref stash_transformRef.Get(target).Value;

            if (AttackSequenceMap.ContainsKey(CurrentCaster.Id))
            {
                AttackSequenceMap[CurrentCaster.Id].Kill(true);
                AttackSequenceMap.Remove(CurrentCaster.Id);
            }

            var attackSequence = GetAttackSequence(target, executerTransform, targetTransform);
            
            AddAttackingTagToCaster(CurrentCaster);

            AttackSequenceMap.Add(CurrentCaster.Id, attackSequence);
            
            attackSequence.OnComplete(
                () => OnSkillUseCompleted()
            );
        }
        
        private Sequence GetAttackSequence(Entity target, Transform attackerTransform, Transform targetTransform){
            
            var raiseHeight = 20;
            
            Sequence seq = DOTween.Sequence();
            
            var originalPosition = attackerTransform.position;
            var targetPos = targetTransform.position;

            seq.Append(attackerTransform.DOMoveZ(originalPosition.z - 1.0f, 0.1f));

            seq.Append(attackerTransform.DOMoveY(originalPosition.y 
                + raiseHeight, 0.5f).SetEase(Ease.OutQuad));

            seq.Append(attackerTransform.DOMove(targetPos, 0.25f)
                .SetEase(Ease.InExpo).OnComplete(
                    () => DoDamageToTarget(target) // Do damage
                ));
                

            var attackPos = new Vector3(originalPosition.x, 
                originalPosition.y + raiseHeight, originalPosition.z - 1.0f);
                
            seq.Append(attackerTransform.DOMove(attackPos, 0.25f)
                .SetEase(Ease.OutQuad));

            seq.Append(attackerTransform.DOMove(originalPosition, 0.5f)
                .SetEase(Ease.InQuad));
                

            return seq;
        }
        
        private void DoDamageToTarget(Entity target){
            if (stash_health.Has(target))
            {
                ref var c_health = ref stash_health.Get(target);
                c_health.Value--;
                Debug.Log($"Target {target.Id} now have {c_health.Value} hp.");
            }
        }


        private List<Entity> FilterAlowedOptions(List<Entity> options){
            List<Entity> filter = new();
            foreach(var opt in options){
                if(stash_occupiedCells.Has(opt)){
                    if(stash_enemyTag.Has(stash_occupiedCells.Get(opt).Occupier)){
                        filter.Add(opt);
                    }
                }
            }
            return filter;
        }

        private List<Entity> FilterForbiddenOptions(List<Entity> options){
            List<Entity> filter = new();
            foreach (var opt in options)
            {
                if (stash_occupiedCells.Has(opt) == false)
                {
                    filter.Add(opt);
                }
                else{
                    if (stash_enemyTag.Has(stash_occupiedCells.Get(opt).Occupier) == false)
                    {
                        filter.Add(opt);
                    }
                }  
            }
            return filter;
        }
        private List<Entity> GetAttackOptions(Entity executer)
        {
            if(stash_attackAbility.Has(executer) == false){return new();}
            
            var result = new List<Entity>();
            var cellPositions = new List<Vector2Int>();
            var attacks = stash_attackAbility.Get(executer).Attacks;
            var c_gridPos = stash_gridPosition.Get(executer);
            var gridPosVector2 = new Vector2Int(c_gridPos.grid_x, c_gridPos.grid_y);

            foreach(var attack in attacks){
                cellPositions.Add(gridPosVector2 + attack);
            }
            foreach (var cell in f_Cells){
                var c_cellPos = stash_cellPosition.Get(cell);
                var cellPos = new Vector2Int(c_cellPos.grid_x, c_cellPos.grid_y);
                if (cellPositions.Contains(cellPos))
                {
                    result.Add(cell);
                }
            }
            return result;
        }

        private bool ValidateSkillUsability()
        {
            if (f_currentTurnTaker.IsEmpty()) { return false; }

            var user = f_currentTurnTaker.FirstOrDefault();
            if (user.IsExist() == false) { return false; }

            if (stash_performingAction.Has(user)) { return false; }

            return true;
        }
        private bool ValidateAttackOptions(List<Entity> options)
        {
            if (options.Count < 1) { return false; }
            return true;
        }
        private bool IsAttackAbilityButtonClicked(ButtonClickedEvent evt)
        {
            if(stash_attackAbilityBtn.Has(evt.ClickedButton)){return true;}
            return false;
        }
    }
}


