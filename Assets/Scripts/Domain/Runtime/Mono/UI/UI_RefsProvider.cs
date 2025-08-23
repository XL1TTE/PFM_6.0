using Domain.UI.Requests;
using Domain.UI.Widgets;
using Scellecs.Morpeh;
using UnityEngine;

namespace Domain.UI.Mono
{
    public class UI_RefsProvider : MonoBehaviour
    {
        
        [SerializeField] public UI_FullscreenNotification ui_fullScreenNotification;
        [SerializeField] public FpsCounter ui_fpsCounter;
        
        void Awake()
        {
            var _request = World.Default.GetRequest<SharedUILinkRequest>();
            _request.Publish(new SharedUILinkRequest{
               ref_FullScreenNotification = ui_fullScreenNotification,
               ref_FpsCounter = ui_fpsCounter
            }, true);
        }
    }
}
