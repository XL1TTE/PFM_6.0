
using Cysharp.Threading.Tasks;
using Domain.Abilities;
using Domain.Abilities.Components;
using Game;
using Scellecs.Morpeh;

namespace Interactions
{
    public interface IOnAbiltiyExecutionEnd
    {
        UniTask OnExecutionEnd(AbilityData a_abiltiy, Entity a_owner, Entity a_target, World a_World);
    }

    public sealed class ConsumeInteraction : BaseInteraction, IOnAbiltiyExecutionEnd
    {
        public UniTask OnExecutionEnd(AbilityData a_abiltiy, Entity a_owner, Entity a_target, World a_World)
        {
            if (a_abiltiy.m_AbilityType != AbilityType.INTERACTION) { return UniTask.CompletedTask; }
            G.ConsumeInteraction(a_owner, a_World);
            return UniTask.CompletedTask;
        }
    }
    public sealed class ConsumeMoveInteraction : BaseInteraction, IOnAbiltiyExecutionEnd
    {
        public UniTask OnExecutionEnd(AbilityData a_abiltiy, Entity a_owner, Entity a_target, World a_World)
        {
            if (a_abiltiy.m_AbilityType != AbilityType.MOVEMENT && a_abiltiy.m_AbilityType != AbilityType.ROTATE) { return UniTask.CompletedTask; }
            G.ConsumeMoveInteraction(a_owner, a_World);
            return UniTask.CompletedTask;
        }
    }
}
