
using Core.Utilities;
using Cysharp.Threading.Tasks;
using Domain.Abilities.Tags;
using Domain.UI.Mono;
using Domain.UI.Tags;
using Game;
using Scellecs.Morpeh;

namespace Interactions
{
    public interface IOnInteractionCountChanged
    {

        UniTask OnChange(Entity a_subject, int a_count, World a_world);
    }


    public sealed class UpdateAbilityButtonsState : BaseInteraction, IOnInteractionCountChanged
    {
        public override Priority m_Priority => Priority.HIGH;
        public UniTask OnChange(Entity a_subject, int a_count, World a_world)
        {
            G.UpdateAbilityButtonsState(a_world);
            return UniTask.CompletedTask;
        }
    }
}
