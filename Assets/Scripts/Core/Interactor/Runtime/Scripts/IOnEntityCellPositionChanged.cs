using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Domain.BattleField.Events;
using Scellecs.Morpeh;
using Unity.VisualScripting;

namespace Interactions
{
    public interface IOnEntityCellPositionChanged
    {
        /// <summary>
        /// Call this when entity's cell position had changed. 
        /// </summary>
        /// <param name="a_pCell">Previous cell.</param>
        /// <param name="a_nCell">New cell.</param>
        /// <param name="a_subject">Subject.</param>
        UniTask OnPositionChanged(Entity a_pCell, Entity a_nCell, Entity a_subject, World a_world);
    }

    public sealed class NotifyEcsSystemsCellPositionChangedInteraction : BaseInteraction, IOnEntityCellPositionChanged
    {
        public async UniTask OnPositionChanged(Entity a_pCell, Entity a_nCell, Entity a_subject, World a_world)
        {
            var evt_cellPositionChanged = a_world.GetEvent<EntityCellPositionChangedEvent>();
            evt_cellPositionChanged.NextFrame(new EntityCellPositionChangedEvent
            {
                m_Subject = a_subject,
                m_pCell = a_pCell,
                m_nCell = a_nCell
            });
            await UniTask.CompletedTask;
        }
    }
}

