using Cysharp.Threading.Tasks;
using Game;
using Scellecs.Morpeh;

namespace Interactions
{
    public interface IOnEntityHealedInteraction
    {
        UniTask Execute(
            Entity a_Source,
            Entity a_Target,
            int a_amount,
            World a_world);
    }

    public sealed class ShowFloatingNumberInteraction : BaseInteraction, IOnEntityHealedInteraction
    {
        public override Priority m_Priority => base.m_Priority;

        public async UniTask Execute(Entity a_Source, Entity a_Target, int a_amount, World a_world)
        {
            GUI.ShowHealNumber(a_Target, a_amount, a_world);
            await UniTask.CompletedTask;
        }
    }

}

