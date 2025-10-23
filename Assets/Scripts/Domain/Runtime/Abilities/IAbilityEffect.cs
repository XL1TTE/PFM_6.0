using System.Collections;
using Cysharp.Threading.Tasks;

namespace Domain.Ability
{
    public interface IAbilityEffect
    {
        UniTask Execute(AbilityContext context);
    }
}
