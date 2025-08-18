using Core.Utilities.Extentions;
using Gameplay.Common.Components;
using Gameplay.Common.Events;
using Gameplay.Features.Monster.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;


namespace Gameplay.Common.Systems{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class TurnSystemInitializer : ISystem
    {
        public World World { get; set; }
        
        private Event<OnStateEnterEvent> evt_onStateEnter;
        private Event<OnStateExitEvent> evt_onStateExit;
        
        private Stash<BattleState> stash_battleState;
        private Stash<CurrentTurnTakerTag> stash_curTurnTaker;

        public void OnAwake()
        {
            evt_onStateEnter = World.GetEvent<OnStateEnterEvent>();
            evt_onStateExit = World.GetEvent<OnStateExitEvent>();

            stash_battleState = World.GetStash<BattleState>();
            stash_curTurnTaker = World.GetStash<CurrentTurnTakerTag>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach(var evt in evt_onStateEnter.publishedChanges){
                if(IsStateValid(evt.StateEntity)){
                    MarkFirstTurnTaker();
                }
            }
            foreach(var evt in evt_onStateExit.publishedChanges){
                if(IsStateValid(evt.StateEntity)){
                    Cleanup();
                }
            }
        }

        public void Dispose()
        {

        }
        
        private void MarkFirstTurnTaker(){
            var monsters = World.Filter
                .With<TagMonster>().Build();
            var monster = monsters.FirstOrDefault();
            if (monster.IsExist()){
                stash_curTurnTaker.Add(monster);
            }

        }
        
        private void Cleanup(){
            var turnTakers = World.Filter.With<CurrentTurnTakerTag>().Build();
            foreach(var e in turnTakers){
                stash_curTurnTaker.Remove(e);
            }
        }



        private bool IsStateValid(Entity state){
            if(stash_battleState.Has(state) == false){return false;}
            return true;
        }
    }
}


