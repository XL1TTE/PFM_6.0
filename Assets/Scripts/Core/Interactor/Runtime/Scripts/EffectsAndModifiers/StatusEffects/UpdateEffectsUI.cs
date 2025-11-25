using Core.Utilities;
using Cysharp.Threading.Tasks;
using Domain.Extentions;
using Domain.GameEffects;
using Game;
using Scellecs.Morpeh;
using UnityEngine;

namespace Interactions
{
    public sealed class UpdateEffectsUIOnApply
    : BaseInteraction, IOnPoisonApplied, IOnBleedApplied, IOnBurningApplied, IOnStunApplied
    {
        public override Priority m_Priority => Priority.LOW;
        public UniTask OnBleedApplied(Entity a_source, Entity a_target, IStatusEffectComponent.Stack a_stack, World a_world)
        {
            G.gui.UpdateEntityEffectsUI(a_target, a_world);
            return UniTask.CompletedTask;
        }
        public UniTask OnBurningApplied(Entity a_source, Entity a_target, IStatusEffectComponent.Stack a_stack, World a_world)
        {
            G.gui.UpdateEntityEffectsUI(a_target, a_world);
            return UniTask.CompletedTask;
        }
        public UniTask OnPoisonApplied(Entity a_source, Entity a_target, IStatusEffectComponent.Stack a_stack, World a_world)
        {
            G.gui.UpdateEntityEffectsUI(a_target, a_world);
            return UniTask.CompletedTask;
        }

        public UniTask OnStunApplied(Entity a_source, Entity a_target, IStatusEffectComponent.Stack a_stack, World a_world)
        {
            G.gui.UpdateEntityEffectsUI(a_target, a_world);
            return UniTask.CompletedTask;
        }

        private async UniTask AddEffectInHealthBar(Entity a_entity, Sprite a_icon, IStatusEffectComponent.Stack a_stack, World a_world)
        {
            var healthBar = F.GetActiveHealthBarFor(a_entity, a_world);

            if (healthBar == null) { return; }

            await healthBar.AddStatusEffect(a_icon, a_stack);
        }
    }
    public sealed class UpdateEffectsUIOnRemove :
        BaseInteraction, IOnPoisonRemoved, IOnBleedRemoved, IOnBurningRemoved, IOnStunRemoved
    {
        public override Priority m_Priority => Priority.VERY_LOW;

        public UniTask OnOnBleedRemoved(Entity a_target, IStatusEffectComponent.Stack a_stack, World a_world)
        {
            G.gui.UpdateEntityEffectsUI(a_target, a_world);
            return UniTask.CompletedTask;
        }

        public UniTask OnBurningRemoved(Entity a_target, IStatusEffectComponent.Stack a_stack, World a_world)
        {
            G.gui.UpdateEntityEffectsUI(a_target, a_world);
            return UniTask.CompletedTask;
        }

        public UniTask OnPoisonRemoved(Entity a_target, IStatusEffectComponent.Stack a_stack, World a_world)
        {
            G.gui.UpdateEntityEffectsUI(a_target, a_world);
            return UniTask.CompletedTask;
        }

        public UniTask OnStunRemoved(Entity a_target, IStatusEffectComponent.Stack a_stack, World a_world)
        {
            G.gui.UpdateEntityEffectsUI(a_target, a_world);
            return UniTask.CompletedTask;
        }
    }

}

