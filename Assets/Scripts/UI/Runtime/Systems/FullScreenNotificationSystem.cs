using Core.Utilities.Extantions;
using Scellecs.Morpeh;
using UI.Components;
using UI.Requests;
using UI.Widgets;
using Unity.IL2CPP.CompilerServices;

namespace UI.Systems{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class FullScreenNotificationSystem : ISystem
    {
        public World World { get; set; }
                
        private Request<FullScreenNotificationRequest> request;
        
        private UI_FullscreenNotification UIControlRef;

        public void OnAwake()
        {
            var _sharedUI = World.Filter.With<SharedUIRefsComponent>().Build().FirstOrDefault();
            request = World.GetRequest<FullScreenNotificationRequest>();


            if (!_sharedUI.IsExist()){
                throw new System.Exception("Entity with SharedUIRefsComponent was not found.");
            }
            var uiRefs = World.GetStash<SharedUIRefsComponent>().Get(_sharedUI);
            UIControlRef = uiRefs.FullScreenNotification;
        }

        public void OnUpdate(float deltaTime)
        {
            foreach(var req in request.Consume()){
                
                switch(req.state){
                    case FullScreenNotificationRequest.State.Enable:
                        EnableUI(req.Message, req.Tip);
                        break;
                    case FullScreenNotificationRequest.State.Disable:
                        DisableUI();
                        break;
                    default:
                        return;
                }
                
            }
        }

        public void Dispose()
        {

        }
        
        private void EnableUI(string Message, string Tip = ""){
            UIControlRef.ShowMessage(Message, Tip);
        }
        
        private void DisableUI(){
            UIControlRef.HideMessage();
        }
    }

}

