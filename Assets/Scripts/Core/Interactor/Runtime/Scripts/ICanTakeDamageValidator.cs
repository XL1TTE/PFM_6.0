using System.Collections;
using Cysharp.Threading.Tasks;
using Domain.Stats.Components;
using Scellecs.Morpeh;

namespace Interactions
{
    public interface ICanTakeDamageValidator
    {
        UniTask<bool> Validate(Entity a_Target, World a_World);
    }

    public class IHaveHealthValidator : BaseInteraction, ICanTakeDamageValidator
    {
        public UniTask<bool> Validate(Entity a_Target, World a_World)
        {
            if (a_World.GetStash<Health>().Has(a_Target) == false)
            {
                return UniTask.FromResult(false);
            }

            return UniTask.FromResult(true);
        }
    }
}

