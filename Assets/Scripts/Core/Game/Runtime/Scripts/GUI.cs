using System;
using Core.Utilities;
using Cysharp.Threading.Tasks;
using Domain.UI.Widgets;

namespace Game
{
    public static partial class GUI
    {
        public static void NotifyFullScreen(string a_message, Func<bool> a_hideWhen, string a_tip = "")
        {
            NotifyFullScreenAsync(a_message, a_hideWhen, a_tip).Forget();
        }

        private static async UniTask NotifyFullScreenAsync(string a_message, Func<bool> a_hideWhen, string a_tip = "")
        {
            if (FullscreenNotification.IsInstantiated() == false)
            {
                return;
            }

            FullscreenNotification.ShowMessage(a_message, a_tip);

            await UniTask.WaitUntil(() => a_hideWhen.Invoke());

            FullscreenNotification.HideMessage();
        }
    }
}
