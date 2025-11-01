using Cysharp.Threading.Tasks;
using Domain.Abilities;

namespace Gameplay.Abilities
{
    public class DoNothing : IAbilityNode
    {
        public IAbilityNode Clone() => new DoNothing();

        public async UniTask Execute(AbilityContext context)
        {
            await UniTask.CompletedTask;
        }
    }

}
