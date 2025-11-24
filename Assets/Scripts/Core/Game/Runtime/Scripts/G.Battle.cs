using Core.Utilities;
using Cysharp.Threading.Tasks;
using Domain.Abilities.Components;
using Domain.UI.Mono;
using Interactions;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game
{

    public static partial class G
    {
        public static class Battle
        {
            public static void PlayerWon(World a_world)
            {
                Debug.Log("WIN");
                Interactor.CallAll<IOnPlayerWonBattle>(async h => await h.OnPlayerWon(a_world)).Forget();
            }
            public static void PlayerLost(World a_world)
            {
                Interactor.CallAll<IOnPlayerLostBattle>(async h => await h.OnPlayerLost(a_world)).Forget();
            }

            public static bool IsHoverPagePinned() => BattleUiRefs.Instance.BookWidget.m_IsPinned;

            public static void PinHoverActorPage(Entity actor, bool isPinned)
            {
                var t_book = BattleUiRefs.Instance.BookWidget;
                t_book.m_IsPinned = isPinned;
                if (isPinned)
                {
                    t_book.m_PinnedEntity = actor;
                }
            }

            public static void UpdateTurnTakerPageInBook(Entity entity, World a_world)
            {
                var t_maxHealth = GU.GetMaxHealth(entity, a_world);
                var t_health = GU.GetHealth(entity, a_world);
                var t_speed = GU.GetSpeed(entity, a_world);

                var t_book = BattleUiRefs.Instance.BookWidget;

                t_book.SetHealth(t_maxHealth, t_health);
                t_book.SetSpeed(t_speed);

                t_book.SetFireResSprite(GUI.GetFireResistanceSprite(entity, a_world));
                t_book.SetPoisonResSprite(GUI.GetPoisonResistanceSprite(entity, a_world));
                t_book.SetBleedResSprite(GUI.GetBleedResistanceSprite(entity, a_world));

                if (F.HasName(entity, a_world))
                {
                    t_book.SetTurnTakerName(F.GetName(entity, a_world));
                }
                else { t_book.SetTurnTakerName("Unknown"); }

                if (F.HasAvatar(entity, a_world)) { t_book.m_Avatar.sprite = F.GetAvatar(entity, a_world); }
                else { t_book.m_Avatar.sprite = null; }
            }
            public static void UpdateHoveredActorPageInBook(Entity entity, World a_world)
            {
                var t_maxHealth = GU.GetMaxHealth(entity, a_world);
                var t_health = GU.GetHealth(entity, a_world);
                var t_speed = GU.GetSpeed(entity, a_world);

                var t_book = BattleUiRefs.Instance.BookWidget;

                t_book.SetRpHealth(t_maxHealth, t_health);
                t_book.SetRpSpeed(t_speed);

                t_book.SetRpFireResSprite(GUI.GetFireResistanceSprite(entity, a_world));
                t_book.SetRpPoisonResSprite(GUI.GetPoisonResistanceSprite(entity, a_world));
                t_book.SetRpBleedResSprite(GUI.GetBleedResistanceSprite(entity, a_world));

                if (F.HasName(entity, a_world))
                {
                    t_book.SetRpName(F.GetName(entity, a_world));
                }
                else { t_book.SetRpName("Unknown"); }

                if (F.HasAvatar(entity, a_world)) { t_book.m_RpAvatar.sprite = F.GetAvatar(entity, a_world); }
                else { t_book.m_RpAvatar.sprite = null; }

                var stash_Abilities = a_world.GetStash<AbilitiesComponent>();

                if (stash_Abilities.Has(entity))
                {
                    var t_abilities = stash_Abilities.Get(entity);

                    t_book.m_HeadAbilityViewer.m_Icon.sprite = t_abilities.m_HeadAbility.m_Icon;
                    t_book.m_RightHandAbilityViewer.m_Icon.sprite = t_abilities.m_RightHandAbility.m_Icon;
                    t_book.m_MoveAbilityViewer.m_Icon.sprite = t_abilities.m_LegsAbility.m_Icon;
                    t_book.m_LeftHandAbilityViewer.m_Icon.sprite = t_abilities.m_LeftHandAbility.m_Icon;

                    t_book.m_HeadAbilityViewer.m_AbilityData = t_abilities.m_HeadAbility;
                    t_book.m_RightHandAbilityViewer.m_AbilityData = t_abilities.m_RightHandAbility;
                    t_book.m_MoveAbilityViewer.m_AbilityData = t_abilities.m_LegsAbility;
                    t_book.m_LeftHandAbilityViewer.m_AbilityData = t_abilities.m_LeftHandAbility;
                }
            }

            public static void HideTurnTakerPageInBook()
            {
                BattleUiRefs.Instance.BookWidget.HideTurnTakerInfo();
            }
            public static void HideHoveredActorPageInBook()
            {
                BattleUiRefs.Instance.BookWidget.HideHoveredEntityInfo();
            }
            public static void ShowTurnTakerPageInBook()
            {
                BattleUiRefs.Instance.BookWidget.ShowTurnTakerInfo();
            }
            public static void ShowHoveredActorPageInBook()
            {
                BattleUiRefs.Instance.BookWidget.ShowHoveredEntityInfo();
            }
        }

    }

}
