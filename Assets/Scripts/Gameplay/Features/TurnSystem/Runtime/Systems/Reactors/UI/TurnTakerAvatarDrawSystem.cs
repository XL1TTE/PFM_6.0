
using Domain.Monster.Components;
using Domain.Monster.Tags;
using Domain.TurnSystem.Components;
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
    public sealed class TurnTakerAvatarDrawSystem : ISystem
    {
        public World World { get; set; }

        private Filter filter_currentTurnTaker;

        private Event<TurnSystemInitializedEvent> evt_turnSystemInitialized;
        private Event<NextTurnStartedEvent> evt_nextTurnStarted;
        
        private Stash<TurnQueueAvatar> stash_turnQueueAvatar;

        public void OnAwake()
        {
            filter_currentTurnTaker = World.Filter
                .With<TurnQueueAvatar>()
                .With<CurrentTurnTakerTag>()
                .Build();

            evt_turnSystemInitialized = World.GetEvent<TurnSystemInitializedEvent>();
            evt_nextTurnStarted = World.GetEvent<NextTurnStartedEvent>();

            stash_turnQueueAvatar = World.GetStash<TurnQueueAvatar>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach(var envt in evt_turnSystemInitialized.publishedChanges){
                ShowAvatar();
            }

            foreach (var evt in evt_nextTurnStarted.publishedChanges)
            {
                DrawCurrentTurnTakerAvatar();
            }
        }

        public void Dispose()
        {

        }
        
        private void ShowAvatar(){
            BattleFieldUIRefs.Instance.BookWidget.TurnTakerAvatar.gameObject.SetActive(true);
        }

        private void HideAvatar(){
            BattleFieldUIRefs.Instance.BookWidget.TurnTakerAvatar.gameObject.SetActive(false);
        }

        private void DrawCurrentTurnTakerAvatar(){
            if(filter_currentTurnTaker.IsEmpty()){return;}
            
            var turnTaker = filter_currentTurnTaker.First();
            var avatar = stash_turnQueueAvatar.Get(turnTaker).Value;
            if (avatar != null)
            {
                BattleFieldUIRefs.Instance.BookWidget.TurnTakerAvatar.sprite = avatar;
            }
        }

    }
}


