using System.Collections;
using System.Collections.Generic;
using Core.Utilities;
using Cysharp.Threading.Tasks;
using Domain.Abilities;
using Domain.Stats.Components;
using Scellecs.Morpeh;

namespace Interactions
{
    public interface ICalculateDamageInteraction
    {
        UniTask Execute(
            Entity a_Source,
            Entity a_Target,
            World a_world,
            DamageType a_damageType,
            ref int a_damage,
            List<OnDamageTags> a_tags);
    }

    public sealed class ResistPoisonInteraction : BaseInteraction, ICalculateDamageInteraction
    {
        public UniTask Execute(Entity a_Source, Entity a_Target, World a_world, DamageType a_damageType, ref int a_damage, List<OnDamageTags> a_tags = null)
        {
            if (a_damageType != DamageType.POISON_DAMAGE) { return UniTask.CompletedTask; }

            switch (F.GetResistance<PoisonResistanceModiffier>(a_Target, a_world))
            {
                case IResistanceModiffier.Stage.NONE:
                    break;
                case IResistanceModiffier.Stage.WEAKNESS:
                    a_tags.Add(OnDamageTags.WEAKNESS);
                    a_damage *= 2;
                    break;
                case IResistanceModiffier.Stage.RESISTANT:
                    a_tags.Add(OnDamageTags.RESISTANT);
                    a_damage /= 2;
                    break;
                case IResistanceModiffier.Stage.IMMUNE:
                    a_tags.Add(OnDamageTags.IMMUNED);
                    a_damage = 0;
                    break;
            }

            return UniTask.CompletedTask;
        }
    }
    public sealed class ResistBleedingInteraction : BaseInteraction, ICalculateDamageInteraction
    {
        public UniTask Execute(Entity a_Source, Entity a_Target, World a_world, DamageType a_damageType, ref int a_damage, List<OnDamageTags> a_tags = null)
        {
            if (a_damageType != DamageType.BLEED_DAMAGE) { return UniTask.CompletedTask; }

            switch (F.GetResistance<BleedResistanceModiffier>(a_Target, a_world))
            {
                case IResistanceModiffier.Stage.NONE:
                    break;
                case IResistanceModiffier.Stage.WEAKNESS:
                    a_tags.Add(OnDamageTags.WEAKNESS);
                    a_damage *= 2;
                    break;
                case IResistanceModiffier.Stage.RESISTANT:
                    a_tags.Add(OnDamageTags.RESISTANT);
                    a_damage /= 2;
                    break;
                case IResistanceModiffier.Stage.IMMUNE:
                    a_tags.Add(OnDamageTags.IMMUNED);
                    a_damage = 0;
                    break;
            }

            return UniTask.CompletedTask;
        }
    }
    public sealed class ResistBurningInteraction : BaseInteraction, ICalculateDamageInteraction
    {
        public UniTask Execute(Entity a_Source, Entity a_Target, World a_world, DamageType a_damageType, ref int a_damage, List<OnDamageTags> a_tags = null)
        {
            if (a_damageType != DamageType.FIRE_DAMAGE) { return UniTask.CompletedTask; }

            switch (F.GetResistance<BurningResistanceModiffier>(a_Target, a_world))
            {
                case IResistanceModiffier.Stage.NONE:
                    break;
                case IResistanceModiffier.Stage.WEAKNESS:
                    a_tags.Add(OnDamageTags.WEAKNESS);
                    a_damage *= 2;
                    break;
                case IResistanceModiffier.Stage.RESISTANT:
                    a_tags.Add(OnDamageTags.RESISTANT);
                    a_damage /= 2;
                    break;
                case IResistanceModiffier.Stage.IMMUNE:
                    a_tags.Add(OnDamageTags.IMMUNED);
                    a_damage = 0;
                    break;
            }

            return UniTask.CompletedTask;
        }
    }
}

