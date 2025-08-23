
using Domain.Monster.Components;
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
    public sealed class MonsterAvatarDrawSystem : ISystem
    {
        public World World { get; set; }

        private Filter filter_monsterTurnTaker;

        private Event<NextTurnStartedEvent> evt_nextTurnStarted;
        
        private Stash<MonsterDammyRefComponent> stash_monsterDammy;

        public void OnAwake()
        {
            filter_monsterTurnTaker = World.Filter
                .With<TagMonster>()
                .With<CurrentTurnTakerTag>()
                .Build();

            evt_nextTurnStarted = World.GetEvent<NextTurnStartedEvent>();
            
            stash_monsterDammy = World.GetStash<MonsterDammyRefComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var evt in evt_nextTurnStarted.publishedChanges)
            {
                if (filter_monsterTurnTaker.IsEmpty()) { return; }

                DrawAvatar();
            }
        }

        public void Dispose()
        {

        }

        private void DrawAvatar()
        {
            if(filter_monsterTurnTaker.IsEmpty()){return;}
            
            var monster = filter_monsterTurnTaker.First();
            var avatar = stash_monsterDammy.Get(monster).MonsterDammy.MonsterAvatar;
            if(avatar != null){
                BattleFieldUIRefs.Instance.BookWidget.TurnTakerAvatar.sprite = avatar;
            }
        }
    }
}


