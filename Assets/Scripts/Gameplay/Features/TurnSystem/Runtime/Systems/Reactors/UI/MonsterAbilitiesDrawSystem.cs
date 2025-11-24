using System.Collections.Generic;
using Core.Utilities;
using Cysharp.Threading.Tasks;
using Domain.Abilities.Components;
using Domain.Abilities.Mono;
using Domain.Abilities.Providers;
using Domain.Extentions;

using Domain.UI.Mono;
using Game;
using Interactions;

using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Gameplay.TurnSystem.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MonsterAbilitiesDrawSystem : BaseInteraction, IOnTurnStartInteraction
    {
        private List<AbilityButtonView> m_abilityBtnsCache = new();


        public UniTask OnTurnStart(Entity a_turnTaker, World a_world)
        {
            if (F.IsTakingTurn(a_turnTaker, a_world))
            {
                ClearAbilityViews();
                DrawAbilities(a_turnTaker, a_world);
                G.UpdateAbilityButtonsState(a_world);
            }
            else
            {
                ClearAbilityViews();
            }
            return UniTask.CompletedTask;
        }

        private void DrawAbilities(Entity abilityOwner, World a_world)
        {
            var stash_Abilities = a_world.GetStash<AbilitiesComponent>();

            var m_AbilityButtonPrefab = GR.p_AbilityButton;

            if (stash_Abilities.Has(abilityOwner) == false) { return; }
            var t_abilities = stash_Abilities.Get(abilityOwner);

            if (t_abilities.m_LeftHandAbility?.m_Value != null)
            {
                var slot = BattleUiRefs.Instance.BookWidget.m_FirstHandAbilitySlot;
                var ability_btn = Object.Instantiate(m_AbilityButtonPrefab, slot);
                SetAbilityIcon(ability_btn, t_abilities.m_LeftHandAbility);
                AttachAbilityOwnerToView(ability_btn, abilityOwner);
                AttachAbilityToView(ability_btn, t_abilities.m_LeftHandAbility);
                m_abilityBtnsCache.Add(ability_btn);
            }
            if (t_abilities.m_RightHandAbility?.m_Value != null)
            {
                var slot = BattleUiRefs.Instance.BookWidget.m_SecondHandAbilitySlot;
                var ability_btn = Object.Instantiate(m_AbilityButtonPrefab, slot);
                SetAbilityIcon(ability_btn, t_abilities.m_RightHandAbility);

                AttachAbilityOwnerToView(ability_btn, abilityOwner);
                AttachAbilityToView(ability_btn, t_abilities.m_RightHandAbility);
                m_abilityBtnsCache.Add(ability_btn);
            }
            if (t_abilities.m_HeadAbility?.m_Value != null)
            {
                var slot = BattleUiRefs.Instance.BookWidget.m_HeadAbilitySlot;
                var ability_btn = Object.Instantiate(m_AbilityButtonPrefab, slot);
                SetAbilityIcon(ability_btn, t_abilities.m_HeadAbility);

                AttachAbilityOwnerToView(ability_btn, abilityOwner);
                AttachAbilityToView(ability_btn, t_abilities.m_HeadAbility);
                m_abilityBtnsCache.Add(ability_btn);
            }
            if (t_abilities.m_LegsAbility?.m_Value != null)
            {
                var slot = BattleUiRefs.Instance.BookWidget.m_MoveButtonSlot;
                var ability_btn = Object.Instantiate(m_AbilityButtonPrefab, slot);
                SetAbilityIcon(ability_btn, t_abilities.m_LegsAbility);

                AttachAbilityOwnerToView(ability_btn, abilityOwner);
                AttachAbilityToView(ability_btn, t_abilities.m_LegsAbility);
                m_abilityBtnsCache.Add(ability_btn);
            }
            if (t_abilities.m_TurnAroundAbility?.m_Value != null)
            {
                var slot = BattleUiRefs.Instance.BookWidget.m_TurnAroundButtonSlot;
                var ability_btn = Object.Instantiate(m_AbilityButtonPrefab, slot);
                SetAbilityIcon(ability_btn, t_abilities.m_TurnAroundAbility);

                AttachAbilityOwnerToView(ability_btn, abilityOwner);
                AttachAbilityToView(ability_btn, t_abilities.m_TurnAroundAbility);
                m_abilityBtnsCache.Add(ability_btn);
            }
        }

        private void SetAbilityIcon(AbilityButtonView a_AbilityBtn, AbilityData a_AbilityData)
        {
            a_AbilityBtn.SetIcon(a_AbilityData.m_Icon);
        }

        private void AttachAbilityOwnerToView(AbilityButtonView view, Entity owner)
        {
            if (view.gameObject.TryFindComponent<AbilityButtonTagProvider>(out var ability))
            {
                ability.GetData().m_ButtonOwner = owner;
            }
        }
        private void AttachAbilityToView(AbilityButtonView view, AbilityData ability)
        {
            if (view.gameObject.TryFindComponent<AbilityButtonTagProvider>(out var abilityTag))
            {
                abilityTag.GetData().m_Ability = ability;
            }
        }

        private void ClearAbilityViews()
        {
            foreach (var item in m_abilityBtnsCache)
            {
                Object.Destroy(item.gameObject);
            }
            m_abilityBtnsCache.Clear();
        }
    }
}


