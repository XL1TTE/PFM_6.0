using System;
using System.Collections.Generic;
using Core.Utilities;
using Cysharp.Threading.Tasks;
using Domain.Abilities;
using Domain.Extentions;
using Domain.FloatingDamage;
using Domain.Stats.Components;
using Domain.UI.Widgets;
using Gameplay.FloatingDamage.Systems;
using Scellecs.Morpeh;
using UI.Elements;
using UnityEngine;

namespace Game
{
    public static partial class GUI
    {
        public static void NotifyFullScreen(string a_message, UniTask a_hideWhen, Color a_bgColor, string a_tip = "")
        {
            NotifyFullScreenAsync(a_message, a_hideWhen, a_bgColor, a_tip).Forget();
        }

        public static async UniTask NotifyFullScreenAsync(string a_message, UniTask a_hideWhen, Color a_bgColor, string a_tip = "")
        {
            if (FullscreenNotification.IsInstantiated() == false)
            {
                return;
            }

            FullscreenNotification.SetBgColor(a_bgColor);
            FullscreenNotification.ShowMessage(a_message, a_tip);

            await a_hideWhen;

            FullscreenNotification.HideMessage();
        }

        public static void ShowFloatingDamage(Entity a_target, int a_value, DamageType a_damageType, World a_world, List<OnDamageTags> a_tags)
        {
            ShowFloatingDamageAsync(a_target, a_value, a_damageType, a_world, a_tags).Forget();
        }

        private static async UniTask ShowFloatingDamageAsync(Entity a_target, int a_value, DamageType a_damageType, World a_world, List<OnDamageTags> a_tags)
        {
            if (FloatingGui.IsInstantiated())
            {
                List<Text> t_tagsInText = new(2);

                var spawnPoint = GU.GetTransform(a_target, a_world).position;
                var randomShift = new Vector3(
                    UnityEngine.Random.Range(-20f, 20f),
                    UnityEngine.Random.Range(-5f, 5f),
                    0
                );

                foreach (var tag in a_tags)
                {
                    var text = TextPool.I()
                        .WarmupElement()
                        .SetText(tag.ToString())
                        .AlignCenter()
                        .FitContent(true)
                        .FontSize(T.TEXT_SIZE_H1)
                        .SetColor(a_damageType.ToColor());

                    t_tagsInText.Add(text);
                }

                var floatingDamage = TextPool.I()
                    .WarmupElement()
                    .FitContent(true)
                    .SetText(a_value.ToString())
                    .AlignCenter()
                    .FontSize(T.TEXT_SIZE_H1);

                switch (a_damageType)
                {
                    case DamageType.BLEED_DAMAGE:
                        floatingDamage.SetColor(C.COLOR_BLEED);
                        break;
                    case DamageType.PHYSICAL_DAMAGE:
                        floatingDamage.SetColor(Color.white);
                        break;
                    case DamageType.FIRE_DAMAGE:
                        floatingDamage.SetColor(C.COLOR_BURNING);
                        break;
                    case DamageType.POISON_DAMAGE:
                        floatingDamage.SetColor(C.COLOR_POISON);
                        break;
                }

                var t_spawnIn = spawnPoint + randomShift;
                foreach (var text in t_tagsInText)
                {
                    FloatingGui.Show(t_spawnIn, text);
                    await UniTask.WaitForSeconds(0.25f);
                }

                if (a_tags.Contains(OnDamageTags.IMMUNED)) { return; }

                FloatingGui.Show(t_spawnIn + new Vector3(0, -5f, 0), floatingDamage);
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
                    .FitContent(true)
                    .AlignCenter()
                    .FontSize(T.TEXT_SIZE_H1);

                FloatingGui.Show(targetPos.position, text);
            }
        }


        public static void NotifyUnderCursor(string a_message, Color a_color)
        {
            var text = TextPool.I()
                .WarmupElement()
                .SetText(a_message)
                .FontSize(T.TEXT_SIZE_H1)
                .FitContent(true)
                .SetColor(a_color);

            FloatingGui.Show(Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, -20, 0), text);
        }

        public static Sprite GetFireResistanceSprite(Entity a_entity, World a_world)
        {
            var stage = F.GetResistance<BurningResistanceModiffier>(a_entity, a_world);

            switch (stage)
            {
                case IResistanceModiffier.Stage.NONE:
                    return GR.SPR_UI_FIRE_RES_NONE;
                case IResistanceModiffier.Stage.RESISTANT:
                    return GR.SPR_UI_FIRE_RES_RESISTANT;
                case IResistanceModiffier.Stage.IMMUNE:
                    return GR.SPR_UI_FIRE_RES_IMMUNED;
            }
            return GR.SPR_UI_FIRE_RES_NONE;
        }
        public static Sprite GetPoisonResistanceSprite(Entity a_entity, World a_world)
        {
            var stage = F.GetResistance<PoisonResistanceModiffier>(a_entity, a_world);

            switch (stage)
            {
                case IResistanceModiffier.Stage.NONE:
                    return GR.SPR_UI_POISON_RES_NONE;
                case IResistanceModiffier.Stage.RESISTANT:
                    return GR.SPR_UI_POISON_RES_RESISTANT;
                case IResistanceModiffier.Stage.IMMUNE:
                    return GR.SPR_UI_POISON_RES_IMMUNED;
            }
            return GR.SPR_UI_POISON_RES_NONE;
        }
        public static Sprite GetBleedResistanceSprite(Entity a_entity, World a_world)
        {
            var stage = F.GetResistance<BleedResistanceModiffier>(a_entity, a_world);

            switch (stage)
            {
                case IResistanceModiffier.Stage.NONE:
                    return GR.SPR_UI_BLEED_RES_NONE;
                case IResistanceModiffier.Stage.RESISTANT:
                    return GR.SPR_UI_BLEED_RES_RESISTANT;
                case IResistanceModiffier.Stage.IMMUNE:
                    return GR.SPR_UI_BLEED_RES_IMMUNED;
            }
            return GR.SPR_UI_BLEED_RES_NONE;
        }
    }
}
