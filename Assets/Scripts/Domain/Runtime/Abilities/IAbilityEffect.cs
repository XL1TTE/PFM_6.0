using System.Collections;
using Cysharp.Threading.Tasks;

namespace Domain.Abilities
{
    public interface IAbilityNode
    {
        UniTask Execute(AbilityContext context);

        IAbilityNode Clone();
    }
}
