using System;
using Domain.Extentions;
using Domain.Monster.Tags;
using Domain.StateMachine.Components;
using Domain.StateMachine.Mono;
using Domain.TurnSystem.Events;
using Domain.TurnSystem.Requests;
using Domain.TurnSystem.Tags;
using Domain.UI.Requests;
using Domain.UI.Tags;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEditorInternal;

namespace Gameplay.EcsButtons.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class NextTurnButtonSystem : ISystem
    {
        public World World { get; set; }

        private Filter filter_currentTurnTaker;
        private Filter filter_myBtn;

        private Event<ButtonClickedEvent> evt_btnClicked;
        private Request<ProcessTurnRequest> req_processTurn;
        
        private Event<NextTurnStartedEvent> evt_nextTurnStarted;
        
        private Stash<NextTurnButtonTag> stash_myBtn;
        private Stash<ButtonTag> stash_btnTag;
        
        private Stash<TagMonster> stash_monsterTag;

        public void OnAwake()
        {
            filter_myBtn = World.Filter
                .With<ButtonTag>()
                .With<NextTurnButtonTag>()
                .Build();

            filter_currentTurnTaker = World.Filter
                .With<CurrentTurnTakerTag>()
                .Build();

            evt_btnClicked = World.GetEvent<ButtonClickedEvent>();
            evt_nextTurnStarted = World.GetEvent<NextTurnStartedEvent>();

            req_processTurn = World.GetRequest<ProcessTurnRequest>();

            stash_myBtn = World.GetStash<NextTurnButtonTag>();
            stash_monsterTag = World.GetStash<TagMonster>();
            stash_btnTag = World.GetStash<ButtonTag>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var evt in evt_btnClicked.publishedChanges)
            {
                if(Validate(evt)){
                    Execute(evt);
                    //BlockNextTurnButton();
                }
            }
            
            foreach(var evt in evt_nextTurnStarted.publishedChanges)
            {
                var turnTaker = filter_currentTurnTaker.FirstOrDefault();
                if(turnTaker.IsExist() == false){
                    HideNextTurnButton();
                    return;
                }
                
                if (stash_monsterTag.Has(turnTaker)){ // if monster take turn
                    ShowNextTurnButton();
                    //UnblockNextTurnButton();
                }
            }
        }

        private void BlockNextTurnButton()
        {
            throw new NotImplementedException();
        }

        private void UnblockNextTurnButton()
        {
            throw new NotImplementedException();
        }

        private void ShowNextTurnButton()
        {
            if (filter_myBtn.IsEmpty()) {return;}
            var btn = filter_myBtn.First();
            
            stash_myBtn.Get(btn).View.Show();
            stash_btnTag.Get(btn).state = ButtonTag.State.Enabled;
        }

        private void HideNextTurnButton()
        {
            if (filter_myBtn.IsEmpty()) { return; }
            var btn = filter_myBtn.First();

            stash_myBtn.Get(btn).View.Hide();
            stash_btnTag.Get(btn).state = ButtonTag.State.Disabled;
        }

        public void Dispose()
        {

        }

        private void Execute(ButtonClickedEvent evt)
        {
            req_processTurn.Publish(new ProcessTurnRequest{});
            
            StateMachineWorld.ExitState<TargetSelectionState>();
        }

        private bool Validate(ButtonClickedEvent evt)
        {
            if (stash_myBtn.Has(evt.ClickedButton) == false) { return false; }
            return true;
        }
    }
}


