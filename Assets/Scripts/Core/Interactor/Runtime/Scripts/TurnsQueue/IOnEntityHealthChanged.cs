using Core.Utilities;
using Cysharp.Threading.Tasks;
using Domain.UI.Mono;
using Scellecs.Morpeh;

namespace Interactions
{
    public interface IOnEntityHealthChanged
    {
        UniTask OnChanged(Entity a_Entity, World a_world);
    }

    public sealed class UpdateStatsInBookIfTurnTaker : BaseInteraction, IOnEntityHealthChanged
    {
        public UniTask OnChanged(Entity a_entity, World a_world)
        {
            if (F.IsTakingTurn(a_entity, a_world) == false) { return UniTask.CompletedTask; }

            var t_maxHealth = GU.GetMaxHealth(a_entity, a_world);
            var t_health = GU.GetHealth(a_entity, a_world);

            var t_book = BattleUiRefs.Instance.BookWidget;

            t_book.SetHealth(t_maxHealth, t_health);

            return UniTask.CompletedTask;
        }
    }

}
