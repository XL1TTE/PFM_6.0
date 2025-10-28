
using Domain.DragAndDrop.Components;
using Domain.Monster.Tags;
using Domain.StateMachine.Components;
using Domain.StateMachine.Mono;
using Domain.StateMachine.Requests;
using Domain.UI.Requests;
using Domain.UI.Tags;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.EcsButtons.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ExitPlanningStageButtonSystem : ISystem
    {
        public World World { get; set; }

        private Filter f_dragedMonsters;

        public Event<ButtonClickedEvent> _evt;
        public Request<ChangeStateRequest> req_changeState;


        public Stash<StartBattleButtonTag> stash_myBtn;


        public void OnAwake()
        {
            f_dragedMonsters = World.Filter.With<DragStateComponent>().With<TagMonster>().Build();

            _evt = World.GetEvent<ButtonClickedEvent>();
            req_changeState = World.GetRequest<ChangeStateRequest>();

            stash_myBtn = World.GetStash<StartBattleButtonTag>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var evt in _evt.publishedChanges)
            {
                if (Validate(evt) == false) { return; }
                SM.ExitState<BattlePlanningState>();
                SM.EnterState<PreBattleNotificationState>();
            }
        }

        public void Dispose()
        {

        }

        private bool Validate(ButtonClickedEvent request)
        {
            if (stash_myBtn.Has(request.ClickedButton) == false) { return false; }

            if (f_dragedMonsters.IsEmpty() == false) { return false; }
            return true;
        }
    }
}


