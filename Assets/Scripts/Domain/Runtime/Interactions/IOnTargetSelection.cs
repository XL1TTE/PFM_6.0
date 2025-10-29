using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Domain.Monster.Tags;
using Scellecs.Morpeh;

namespace Gameplay.TargetSelection
{

    public enum TargetSelectionTypes : byte
    {
        CELL_WITH_ENEMY,
        CELL_WITH_ALLY,
        CELL_EMPTY,
    }

    public class Result
    {
        public IEnumerable<Entity> m_Value;
    }

    public interface IOnTargetSelection
    {
        UniTask Execute(
            IEnumerable<Entity> a_cellOptions,
            TargetSelectionTypes a_type,
            uint a_maxTargets,
            World a_world,
            Result a_result);
    }
}


