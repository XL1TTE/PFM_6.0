using System.Collections;
using Cysharp.Threading.Tasks;

namespace Domain.Abilities
{
    public interface IAbilityEffect
    {
        UniTask Execute(AbilityContext context);
    }
}
