using Cysharp.Threading.Tasks;
using Domain.Stats.Components;
using Scellecs.Morpeh;

namespace Interactions.ICanBeHealedValidator
{
    public interface ICanBeHealedValidator
    {
        UniTask<bool> Validate(Entity a_target, World a_world, string a_log = "");
    }

    public sealed class IHaveHealValidator : BaseInteraction, ICanBeHealedValidator
    {
        public UniTask<bool> Validate(Entity a_target, World a_world, string a_log = "")
        {
            if (a_world.GetStash<Health>().Has(a_target) == false)
            {
                a_log += $"Nothing to heal!";
                return UniTask.FromResult(false);
            }
            return UniTask.FromResult(true);
        }
    }
}

