using Cysharp.Threading.Tasks;
using Domain.GameEffects;
using Scellecs.Morpeh;

namespace Interactions
{
    public interface IOnStunApplied
    {
        UniTask OnStunApplied(Entity a_source, Entity a_target, IStatusEffectComponent.Stack a_stack, World a_world);
    }

public interface IOnStunRemoved
    {
        UniTask OnStunRemoved(Entity a_target, IStatusEffectComponent.Stack a_stack, World a_world);
    }
}

