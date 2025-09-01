using System;
using System.Collections.Generic;
using Domain.BattleField.Requests;
using Domain.CursorDetection.Components;
using Domain.Extentions;
using Domain.StateMachine.Components;
using Domain.StateMachine.Events;
using Domain.StateMachine.Mono;
using Domain.TargetSelection.Events;
using Domain.TargetSelection.Requests;
using Domain.TargetSelection.Tags;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEditorInternal;
using UnityEngine;

namespace Gameplay.TargetSelection.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class TargetSelectionSystem : ISystem
    {
        private enum SystemState:byte{None, SelectingEnemies, SelectingCells}
        
        public World World { get; set; }
        
        private Filter _selectablesUnderCursor;
        
        private Filter f_state;
        
        private Request<TargetSelectionRequest> req_targetSelection;
        private Request<ChangeCellViewToSelectRequest> req_selectCellsView;
        private Event<TargetSelectionCompletedEvent> evt_selectionComplited;
                
        private Stash<TagAwaibleToSelect> stash_awaibleToSelect;
        private Stash<TargetSelectionState> stash_state;
        
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

            f_state = StateMachineWorld.Value.Filter.With<TargetSelectionState>().Build();

            req_targetSelection = World.GetRequest<TargetSelectionRequest>();
            
            req_selectCellsView = World.GetRequest<ChangeCellViewToSelectRequest>();
            
            evt_selectionComplited = World.GetEvent<TargetSelectionCompletedEvent>();
            
            stash_state = StateMachineWorld.Value.GetStash<TargetSelectionState>();

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
            
            if(!StateMachineWorld.IsStateActiveOptimized(f_state, stash_state, out var state)){
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
            
            
            StateMachineWorld.ExitState<TargetSelectionState>();
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
            StateMachineWorld.EnterState<TargetSelectionState>();
        }
    
        private void PrepareAwaibleTargets(TargetSelectionRequest req){
            foreach(var target in req.AllowedTargets){
                stash_awaibleToSelect.Add(target);
                AwaibleTargets.Add(target);
            }
            HighlightCells();
        }
        
        private void HighlightCells(){

            req_selectCellsView.Publish(new ChangeCellViewToSelectRequest{
                Cells = new List<Entity>(AwaibleTargets), 
                State = ChangeCellViewToSelectRequest.SelectState.Enabled
            });
        }
        
        private void ReturnDefaultColors(){
            req_selectCellsView.Publish(new ChangeCellViewToSelectRequest
            {
                Cells = new List<Entity>(AwaibleTargets),
                State = ChangeCellViewToSelectRequest.SelectState.Disabled
            });
        }
    }
}
