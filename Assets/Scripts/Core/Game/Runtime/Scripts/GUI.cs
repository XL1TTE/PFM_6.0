using System;
using System.Collections.Generic;
using Core.Utilities;
using Cysharp.Threading.Tasks;
using Domain.Abilities;
using Domain.FloatingDamage;
using Domain.UI.Widgets;
using Gameplay.FloatingDamage.Systems;
using Scellecs.Morpeh;

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

        public static void ShowFloatingDamage(Entity a_target, int a_value, DamageType a_damageType, World a_world)
        {
            if (FloatingDamage.IsInstantiated())
            {
                FloatingDamage.Show(a_target, a_value, a_damageType, a_world);
            }
        }

    }
}
