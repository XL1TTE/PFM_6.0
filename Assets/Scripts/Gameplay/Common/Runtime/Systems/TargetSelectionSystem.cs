using System;
using System.Collections;
using System.Collections.Generic;
using Core.Components;
using Core.Utilities.Extentions;
using Gameplay.Common.Components;
using Gameplay.Common.Requests;
using Gameplay.Features.BattleField.Components;
using Gameplay.Features.BattleField.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Gameplay.Common.Systems{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class TargetSelectionSystem : ISystem
    {
        private enum SystemState:byte{None, SelectingEnemies, SelectingCells}
        
        public World World { get; set; }
        
        private Filter _selectablesUnderCursor;
        
        private Request<TargetSelectionRequest> req_targetSelection;
        private Request<CellSpriteChangeRequest> req_changeCellSprite;
        private Event<TargetSelectionCompletedEvent> evt_selectionComplited;
        
        private Stash<TagAwaibleToSelect> stash_awaibleToSelect;
        
        private SystemState CurrentState = SystemState.None;
        
        private List<Entity> AwaibleTargets = new();
        
        private List<Entity> SelectedTargets = new();
        
        private UInt16 MaxTargets = 0;
        
        private int ProcessingRequestID = -1;

        public void OnAwake()
        {
            _selectablesUnderCursor = World.Filter
                .With<TagAwaibleToSelect>()
                .With<UnderCursorComponent>()
                .Build();

            req_targetSelection = World.GetRequest<TargetSelectionRequest>();
            req_changeCellSprite = World.GetRequest<CellSpriteChangeRequest>();
            evt_selectionComplited = World.GetEvent<TargetSelectionCompletedEvent>();

            stash_awaibleToSelect = World.GetStash<TagAwaibleToSelect>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach(var req in req_targetSelection.Consume()){
                Reset();
                SetSystemState(req);
                PrepareAwaibleTargets(req);
            }
            if(CurrentState != SystemState.None){
                ProcessSelection();
            }
        }

        public void Dispose()
        {

        }
        
        private void ProcessSelection(){
            if(Input.GetMouseButtonDown(0) && !_selectablesUnderCursor.IsEmpty()){
                Debug.Log("SELECT CELL");
                SelectTarget(_selectablesUnderCursor.FirstOrDefault());
                if(IsSelectionOver()){
                    evt_selectionComplited.NextFrame(new TargetSelectionCompletedEvent{
                       CompletedRequestID = ProcessingRequestID,
                       SelectedTargets = new List<Entity>(this.SelectedTargets) 
                    });
                    ExitSelection();
                }
            }
            if(Input.GetMouseButtonDown(1)){
                ExitSelection();
            }
        }
        
        private void SelectTarget(Entity target){
            if(target.IsExist() == false){return;}
            
            ref var TargetState = ref stash_awaibleToSelect.Get(target);

            if (TargetState.IsSelected){
                SelectedTargets.Remove(target);
                TargetState.IsSelected = false;
            }
            else{
                SelectedTargets.Add(target);
                TargetState.IsSelected = true;
            }
        }
        
        private bool IsSelectionOver(){
            if(SelectedTargets.Count == Math.Min(MaxTargets, AwaibleTargets.Count)){
                return true;
            }
            return false;
        }
        
        private void ExitSelection(){
            ReturnDefaultColors();
            Reset();
            Debug.Log("EXIT TARGET SELECTING");
        }
        
        private void Reset(){
            foreach(var e in AwaibleTargets){
                stash_awaibleToSelect.Remove(e);
            }
            CurrentState = SystemState.None;
            SelectedTargets.Clear();
            AwaibleTargets.Clear();
            MaxTargets = 0;
            ProcessingRequestID = -1;
        }
        
        private void SetSystemState(TargetSelectionRequest req)
        {
            MaxTargets = req.TargetCount;
            ProcessingRequestID = req.RequestID;
            
            switch (req.Type)
            {
                case TargetSelectionRequest.SelectionType.Cell:
                    CurrentState = SystemState.SelectingCells;
                    break;
                case TargetSelectionRequest.SelectionType.Enemy:
                    CurrentState = SystemState.SelectingEnemies;
                    break;
            }
        }
    
        private void PrepareAwaibleTargets(TargetSelectionRequest req){
            foreach(var target in req.AllowedTargets){
                stash_awaibleToSelect.Add(target);
                AwaibleTargets.Add(target);
            }
            HighlightCells();
        }
        
        private void HighlightCells(){
                
            req_changeCellSprite.Publish(new CellSpriteChangeRequest{
                Cells = new List<Entity>(AwaibleTargets), 
                Sprite = CellSpriteChangeRequest.SpriteType.Highlighted
            });
        }
        
        private void ReturnDefaultColors(){
            req_changeCellSprite.Publish(new CellSpriteChangeRequest
            {
                Cells = new List<Entity>(AwaibleTargets),
                Sprite = CellSpriteChangeRequest.SpriteType.Default
            });
        }
    }
}
