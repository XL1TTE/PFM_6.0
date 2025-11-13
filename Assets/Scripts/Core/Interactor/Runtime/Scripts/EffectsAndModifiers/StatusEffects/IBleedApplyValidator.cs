using Cysharp.Threading.Tasks;
using Domain.GameEffects;
using Scellecs.Morpeh;

namespace Interactions
{
    public interface IBleedApplyValidator
    {
        UniTask<bool> TestOneCriteria(Entity a_source, Entity a_target, int a_duration, int a_damagePerTick, World a_world);
    }

    public sealed class DeclineIfHavePoison : BaseInteraction, IBleedApplyValidator
    {
        public UniTask<bool> TestOneCriteria(Entity a_source, Entity a_target, int a_duration, int a_damagePerTick, World a_world)
        {
            if (a_world.GetStash<PoisonStatusComponent>().Has(a_target)) { return UniTask.FromResult(false); }
            return UniTask.FromResult(true);
        }
    }

}

