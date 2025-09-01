using System;
using System.Collections.Generic;
using Domain.Abilities.Components;
using Domain.Abilities.Tags;
using Domain.BattleField.Components;
using Domain.BattleField.Tags;
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

        private Filter f_currentTurnTaker;
        private Filter f_occupiedCells;
        
        private Request<TargetSelectionRequest> req_targetSelection;

        private Event<ButtonClickedEvent> evt_ButtonClicked;
        private Event<TargetSelectionCompletedEvent> evt_selectionCompleted;

        private Stash<AttackAbilityBtnTag> stash_attackAbilityBtn;
        private Stash<AttackAbility> stash_attackAbility;
        private Stash<CellPositionComponent> stash_cellPosition;
        private Stash<GridPosition> stash_gridPosition;
        private Stash<TagOccupiedCell> stash_occupiedCells;
        private Stash<Health> stash_health;
        private Stash<TagEnemy> stash_enemyTag;


        private Entity CurrentExecuter;


        public void OnAwake()
        {
            f_currentTurnTaker = World.Filter
                .With<TagMonster>()
                .With<CurrentTurnTakerTag>()
                .Build();

            f_occupiedCells = World.Filter 
                .With<CellTag>()
                .With<CellPositionComponent>()
                .With<TagOccupiedCell>()
                .Build();

            req_targetSelection = World.GetRequest<TargetSelectionRequest>();


            evt_ButtonClicked = World.GetEvent<ButtonClickedEvent>();
            evt_selectionCompleted = World.GetEvent<TargetSelectionCompletedEvent>();

            stash_attackAbilityBtn = World.GetStash<AttackAbilityBtnTag>();
            stash_attackAbility = World.GetStash<AttackAbility>();
            stash_cellPosition = World.GetStash<CellPositionComponent>();
            stash_gridPosition = World.GetStash<GridPosition>();
            stash_occupiedCells = World.GetStash<TagOccupiedCell>();
            stash_health = World.GetStash<Health>();
            stash_enemyTag = World.GetStash<TagEnemy>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach(var evt in evt_ButtonClicked.publishedChanges){
                if(Validate(evt)){ // if attack button clicked
                    CurrentExecuter = f_currentTurnTaker.FirstOrDefault();
                    Execute(CurrentExecuter);
                }
            }
            
            foreach(var evt in evt_selectionCompleted.publishedChanges){
                if (evt.CompletedRequestID != SystemID) { return; }
                if(evt.SelectedTargets.Count < 1){return; }
                
                foreach(var t in evt.SelectedTargets){
                    if(stash_occupiedCells.Has(t) == false){continue;}
                    var target = stash_occupiedCells.Get(t).Occupier;

                    // Attack logic (now just health--)
                    if(stash_health.Has(target)){
                        ref var c_health = ref stash_health.Get(target);
                        c_health.Value--;
                        Debug.Log($"Target {t.Id} now have {c_health.Value} hp.");
                    }
                }
            }
        }

        public void Dispose()
        {
            CurrentExecuter = default;
        }

        private void Execute(Entity executer)
        {
            Debug.Log("ATTACK ABILITY");
            if (executer.IsExist())
            {
                List<Entity> allowedCells = FindAttackCells(executer);
                if (allowedCells.Count > 0)
                {
                    Debug.Log("SEND TARGET SELECTION REQUEST");
                    req_targetSelection.Publish(new TargetSelectionRequest
                    {
                        RequestID = SystemID,
                        AllowedTargets = allowedCells,
                        TargetCount = 1,
                        Type = TargetSelectionRequest.SelectionType.Cell
                    });
                }
            }
        }

        private List<Entity> FindAttackCells(Entity executer)
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
            foreach (var cell in f_occupiedCells){
                if(stash_enemyTag.Has(stash_occupiedCells.Get(cell).Occupier) == false){continue;} 
                var c_cellPos = stash_cellPosition.Get(cell);
                var cellPos = new Vector2Int(c_cellPos.grid_x, c_cellPos.grid_y);
                if (cellPositions.Contains(cellPos))
                {
                    result.Add(cell);
                }
            }
            return result;
        }

        private bool Validate(ButtonClickedEvent evt)
        {
            if(stash_attackAbilityBtn.Has(evt.ClickedButton)){return true;}
            return false;
        }
    }
}


