using System;
using System.Collections.Generic;
using Core.Utilities;
using Cysharp.Threading.Tasks;
using Domain.Abilities;
using Domain.Extentions;
using Domain.FloatingDamage;
using Domain.UI.Widgets;
using Gameplay.FloatingDamage.Systems;
using Scellecs.Morpeh;
using UI.Elements;
using UnityEngine;

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
            if (FloatingGui.IsInstantiated())
            {

                var floatingDamage = TextPool.I()
                    .WarmupElement()
                    .SetText(a_value.ToString())
                    .AlignCenter()
                    .FontSize(T.TEXT_SIZE_H1);

                switch (a_damageType)
                {
                    case DamageType.BLEED_DAMAGE:
                        floatingDamage.SetColor(Color.red);
                        break;
                    case DamageType.PHYSICAL_DAMAGE:
                        floatingDamage.SetColor(Color.white);
                        break;
                    case DamageType.FIRE_DAMAGE:
                        floatingDamage.SetColor(Color.yellow);
                        break;
                    case DamageType.POISON_DAMAGE:
                        floatingDamage.SetColor(Color.green);
                        break;
                }

                FloatingGui.Show(GU.GetTransform(a_target, a_world).position, floatingDamage);
            }
        }

        public static void ShowHealNumber(Entity a_target, int a_value, World a_world)
        {
            if (FloatingGui.IsInstantiated())
            {
                ref var targetPos = ref GU.GetTransform(a_target, a_world);

                var text = TextPool.I()
                    .WarmupElement()
                    .SetText(a_value.ToString())
                    .SetColor("#9dff7a".ToColor())
                    .AlignCenter()
                    .FontSize(T.TEXT_SIZE_B1);

                FloatingGui.Show(targetPos.position, text);
            }
        }
    }
}
