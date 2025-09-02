using System;
using Domain.Enemies.Tags;
using Domain.Monster.Tags;
using Domain.TurnSystem.Events;
using Domain.TurnSystem.Tags;
using Domain.UI.Mono;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.TurnSystem.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class InformationBoardViewSystem : ISystem
    {
        public World World { get; set; }
        
        private Filter f_turnTaker;
        
        private Event<NextTurnStartedEvent> evt_nextTurnStarted;
        
        private Stash<TagMonster> stash_monsterTag;
        private Stash<TagEnemy> stash_enemyTag;

        public void OnAwake()
        {
            f_turnTaker = World.Filter.With<CurrentTurnTakerTag>().Build();

            evt_nextTurnStarted = World.GetEvent<NextTurnStartedEvent>();

            stash_monsterTag = World.GetStash<TagMonster>();
            stash_enemyTag = World.GetStash<TagEnemy>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach(var evt in evt_nextTurnStarted.publishedChanges){
                UpdateInformation(evt);
            }
        }

        private void UpdateInformation(NextTurnStartedEvent evt)
        {
            if(f_turnTaker.IsEmpty()){return;}
            
            var turnTaker = f_turnTaker.First();
            
            if(stash_monsterTag.Has(turnTaker)){
                BattleFieldUIRefs.Instance.InformationBoardWidget.ChangeText("Player's turn");
            }
            else if(stash_enemyTag.Has(turnTaker)){
                BattleFieldUIRefs.Instance.InformationBoardWidget.ChangeText("Enemy's turn");
            }
        }

        public void Dispose()
        {
        }
    }
}


