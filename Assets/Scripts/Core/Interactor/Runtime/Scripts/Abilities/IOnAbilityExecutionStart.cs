
using Cysharp.Threading.Tasks;
using Domain.Abilities;
using Domain.Abilities.Components;
using Game;
using Scellecs.Morpeh;

namespace Interactions
{

    public interface IOnAbiltiyExecutionStart
    {
        UniTask OnExecutionStart(AbilityData a_abiltiy, Entity a_owner, Entity a_target, World a_World);
    }


}
