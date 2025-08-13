using System.Collections.Concurrent;
using Core.Utilities.Extentions;
using Gameplay.Common.Components;
using Gameplay.Common.Requests;
using Scellecs.Morpeh;
using UI.Components;
using UI.Requests;
using Unity.IL2CPP.CompilerServices;
using UnityEditorInternal;

namespace Gameplay.Common.Systems{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ExitPlanningStageButtonSystem : ISystem
    {
        public World World { get; set; }


        public Request<ButtonClickedRequest> _request;
        public Request<ChangeStateRequest> req_changeState;


        public Stash<ExitPlanningStageButtonTag> stash_myBtn;


        public void OnAwake()
        {
            _request = World.GetRequest<ButtonClickedRequest>();
            req_changeState = World.GetRequest<ChangeStateRequest>();

            stash_myBtn = World.GetStash<ExitPlanningStageButtonTag>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var req in _request.Consume())
            {
                if (Validate(req) == false) { return; }

                var state = World.Filter.With<BattleState>().Build().FirstOrDefault();
                
                if(state.IsExist()){
                    req_changeState.Publish(new ChangeStateRequest
                    {
                        NextState = state
                    }, true);
                }
            }
        }

        public void Dispose()
        {

        }

        private bool Validate(ButtonClickedRequest request)
        {
            if (stash_myBtn.Has(request.ClickedButton) == false) { return false; }
            return true;
        }
    }
}


