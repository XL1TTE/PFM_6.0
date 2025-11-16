using System.Threading.Tasks;
using Core.Utilities;
using Cysharp.Threading.Tasks;
using Domain.BattleField.Components;
using Domain.BattleField.Events;
using Domain.TurnSystem.Tags;
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

    // public sealed class FixZFighting : BaseInteraction, IOnEntityCellPositionChanged
    // {
    //     public UniTask OnPositionChanged(Entity a_pCell, Entity a_nCell, Entity a_subject, World a_world)
    //     {
    //         ref var transform = ref GU.GetTransform(a_subject, a_world);
    //         transform.position += new UnityEngine.Vector3(0, 0, transform.position.y * 0.01f);
    //         return UniTask.CompletedTask;
    //     }
    // }

    public sealed class UpdateTurnTakerPointer : BaseInteraction, IOnEntityCellPositionChanged
    {
        public async UniTask OnPositionChanged(Entity a_pCell, Entity a_nCell, Entity a_subject, World a_world)
        {
            if (a_world.GetStash<CurrentTurnTakerTag>().Has(a_subject) == false) { return; }

            var stash_cellView = a_world.GetStash<CellViewComponent>();
            if (stash_cellView.Has(a_pCell))
            {
                stash_cellView.Get(a_pCell).m_Value.DisablePointerLayer();
            }
            if (stash_cellView.Has(a_nCell))
            {
                stash_cellView.Get(a_nCell).m_Value.EnablePointerLayer();
            }
            await UniTask.CompletedTask;
        }
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

