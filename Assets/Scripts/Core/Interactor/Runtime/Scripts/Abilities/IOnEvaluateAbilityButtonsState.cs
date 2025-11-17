using System.Reflection;
using Core.Utilities;
using Cysharp.Threading.Tasks;
using Domain.Abilities.Components;
using Domain.Abilities.Tags;
using Scellecs.Morpeh;

namespace Interactions
{
    public interface IOnEvaluateAbilityButtonsState
    {
        /// <summary>
        /// Make shure that a_result parameter is True when passed.
        /// </summary>
        /// <param name="a_button"></param>
        /// <param name="a_result">Should be true by default.</param>
        /// <param name="a_world"></param>
        /// <returns></returns>
        UniTask OnEvaluate(Entity a_button, ref bool a_result, World a_world);
    }

    public sealed class CheckOwnerBusyCondition : BaseInteraction, IOnEvaluateAbilityButtonsState
    {
        public UniTask OnEvaluate(Entity a_button, ref bool a_result, World a_world)
        {
            var owner = F.GetAbilityButtonOwner(a_button, a_world);
            a_result &= !V.IsActorBusy(owner, a_world);
            return UniTask.CompletedTask;
        }
    }
    public sealed class CheckInteractionCountMeetCondition : BaseInteraction, IOnEvaluateAbilityButtonsState
    {
        public UniTask OnEvaluate(Entity a_button, ref bool a_result, World a_world)
        {
            var stash_interactions = a_world.GetStash<InteractionsComponent>();

            if (F.IsAbilityButton(a_button, a_world) == false) { return UniTask.CompletedTask; }

            var owner = F.GetAbilityButtonOwner(a_button, a_world);

            if (F.GetAbilityType(a_button, a_world) != AbilityType.INTERACTION) { return UniTask.CompletedTask; }

            if (stash_interactions.Has(owner) == false) { return UniTask.CompletedTask; }

            if (stash_interactions.Get(owner).m_InteractionsLeft < 1)
            {
                a_result &= false;
            }
            return UniTask.CompletedTask;
        }
    }
    public sealed class CheckMoveInteractionCountMeetCondition : BaseInteraction, IOnEvaluateAbilityButtonsState
    {
        public UniTask OnEvaluate(Entity a_button, ref bool a_result, World a_world)
        {
            var stash_interactions = a_world.GetStash<InteractionsComponent>();

            if (F.IsAbilityButton(a_button, a_world) == false) { return UniTask.CompletedTask; }

            var owner = F.GetAbilityButtonOwner(a_button, a_world);
            var t_abilityType = F.GetAbilityType(a_button, a_world);

            if (t_abilityType != AbilityType.MOVEMENT && t_abilityType != AbilityType.ROTATE) { return UniTask.CompletedTask; }

            if (stash_interactions.Has(owner) == false) { return UniTask.CompletedTask; }

            if (stash_interactions.Get(owner).m_MoveInteractionsLeft < 1)
            {
                a_result &= false;
            }
            return UniTask.CompletedTask;
        }
    }



}
