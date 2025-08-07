using Scellecs.Morpeh;
using UI.Requests;
using UI.Widgets;
using UnityEngine;

namespace UI
{
    public class UI_RefsProvider : MonoBehaviour
    {
        
        [SerializeField] public UI_FullscreenNotification ui_fullScreenNotification;
        
        void Awake()
        {
            var _request = World.Default.GetRequest<SharedUILinkRequest>();
            _request.Publish(new SharedUILinkRequest{
               ref_FullScreenNotification = ui_fullScreenNotification
            }, true);
        }
    }
}
