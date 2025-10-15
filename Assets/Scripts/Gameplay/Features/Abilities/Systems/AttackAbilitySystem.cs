using System.Collections.Generic;
using System.Linq;
using Core.Utilities;
using DG.Tweening;
using Domain.Abilities.Components;
using Domain.Abilities.Tags;
using Domain.AbilityGraph;
using Domain.BattleField.Components;
using Domain.BattleField.Tags;
using Domain.Components;
using Domain.CursorDetection.Components;
using Domain.Enemies.Tags;
using Domain.Extentions;
using Domain.Monster.Tags;
using Domain.TargetSelection.Events;
using Domain.TargetSelection.Requests;
using Domain.TargetSelection.Tags;
using Domain.TurnSystem.Tags;
using Domain.UI.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Gameplay.Abilities.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AttackAbilitySystem : ISystem
    {
        public World World { get; set; }

        private Filter f_targetSelectionResults;
        private Filter f_attackAbiltiyBtns;

        private Filter f_currentTurnTaker;
        private Filter f_Cells;

        private Request<TargetSelectionRequest> req_targetSelection;
        private Request<DealDamageRequest> req_dealDamage;

        private Event<ButtonClickedEvent> evt_ButtonClicked;
        private Stash<TargetSelectionResult> stash_selectionResult;
        private Stash<TargetSelectionSession> stash_targetSelectionSession;
        private Stash<AttackAbilityButtonTag> stash_attackAbilityBtn;
        private Stash<Attacking> stash_attackingTag;
        private Stash<PerfomingActionTag> stash_performingAction;
        private Stash<TagOccupiedCell> stash_occupiedCells;
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
            f_targetSelectionResults = World.Filter
                .With<AttackAbilityButtonTag>()
                .With<TargetSelectionResult>()
                .Build();

            f_attackAbiltiyBtns = World.Filter
                .With<AttackAbilityButtonTag>()
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
            req_dealDamage = World.GetRequest<DealDamageRequest>();

            evt_ButtonClicked = World.GetEvent<ButtonClickedEvent>();

            stash_selectionResult = World.GetStash<TargetSelectionResult>();
            stash_targetSelectionSession = World.GetStash<TargetSelectionSession>();

            stash_attackAbilityBtn = World.GetStash<AttackAbilityButtonTag>();
            stash_performingAction = World.GetStash<PerfomingActionTag>();
            stash_attackingTag = World.GetStash<Attacking>();
            stash_occupiedCells = World.GetStash<TagOccupiedCell>();
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
                if (IsAttackAbilityButtonClicked(evt) == false) { continue; }
                {
                    CurrentCaster = f_currentTurnTaker.FirstOrDefault();
                    CurrentAttackButton = evt.ClickedButton;

                    var AttackOptions = GU.FindAttackOptionsCellsFor(CurrentCaster, World);

                    if (ValidateAttackOptions(AttackOptions) == false) { continue; }

                    StartAttackOptionSelection(evt.ClickedButton, AttackOptions);
                    EnableAbilityButtonSelectedView(evt.ClickedButton);
                }
            }

            // When player succsefuly selected targets
            foreach (var e in f_targetSelectionResults)
            {
                ref var selectionResult = ref stash_selectionResult.Get(e);
                var session = stash_targetSelectionSession.Get(e);

                // When player cancels target selection
                if (selectionResult.m_Status == TargetSelectionStatus.Failed)
                {
                    OnSkillUseCanceled();
                    selectionResult.m_IsProcessed = true;
                    return;
                }

                var SelectedCell = session.m_SelectedOptions.FirstOrDefault();

                DisableAbilityButtonSelectedView(CurrentAttackButton); // We do this here, because we want to remove skill highlight before skill starts executing.
                AttackTargetOnCell(SelectedCell);
                selectionResult.m_IsProcessed = true;
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

        private void StartAttackOptionSelection(Entity sender, List<Entity> cellOptions)
        {
            var allowedCells = FilterAlowedOptions(cellOptions);
            var forbiddenCells = FilterForbiddenOptions(cellOptions);

            SendTargetSelectionRequest(sender, allowedCells, forbiddenCells);
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
            if (button.isNullOrDisposed(World)) { return; }
            if (stash_attackAbilityBtn.Has(button) == false) { return; }

            stash_attackAbilityBtn.Get(button).View.EnableSelectedView();
        }
        private void DisableAbilityButtonSelectedView(Entity button)
        {
            if (button.isNullOrDisposed(World)) { return; }
            if (stash_attackAbilityBtn.Has(button) == false) { return; }

            stash_attackAbilityBtn.Get(button).View.DisableSelectiedView();
        }

        private void SendTargetSelectionRequest(Entity sender, List<Entity> allowedCells, List<Entity> forbiddenCells)
        {
            req_targetSelection.Publish(new TargetSelectionRequest
            {
                m_Sender = sender,
                m_AwaibleOptions = allowedCells,
                m_UnavaibleOptions = forbiddenCells,
                m_TargetsAmount = 1,
            });
        }

        private void AttackTargetOnCell(Entity cell)
        {

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

        private Sequence GetAttackSequence(Entity target, Transform attackerTransform, Transform targetTransform)
        {

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

        private void DoDamageToTarget(Entity target)
        {
            req_dealDamage.Publish(new DealDamageRequest
            {
                Source = CurrentCaster,
                Target = target,
                MinBaseDamage = 1,
                MaxBaseDamage = 5,
                DamageType = DamageType.Physical
            }, true);
        }


        private List<Entity> FilterAlowedOptions(List<Entity> options)
        {
            List<Entity> filter = new();
            foreach (var opt in options)
            {
                if (stash_occupiedCells.Has(opt))
                {
                    if (stash_enemyTag.Has(stash_occupiedCells.Get(opt).Occupier))
                    {
                        filter.Add(opt);
                    }
                }
            }
            return filter;
        }


        // Need to move methods like this in centralized place,
        // and give them a name in "GetAllWithEnemies", "GetEmpty" e.t.c style. 
        private List<Entity> FilterForbiddenOptions(List<Entity> options)
        {
            List<Entity> filter = new();
            foreach (var opt in options)
            {
                if (stash_occupiedCells.Has(opt) == false)
                {
                    filter.Add(opt);
                }
                else
                {
                    if (stash_enemyTag.Has(stash_occupiedCells.Get(opt).Occupier) == false)
                    {
                        filter.Add(opt);
                    }
                }
            }
            return filter;
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
            if (stash_attackAbilityBtn.Has(evt.ClickedButton)) { return true; }
            return false;
        }
    }
}


