using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;

namespace Domain.TargetSelection.Events
{

    public enum TargetSelectionStatus : byte { Success, Failed, Interrupted, }

    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct TargetSelectionResult : IComponent
    {
        /// <summary>
        /// Status with which target selection was completed.
        /// </summary>
        public TargetSelectionStatus m_Status;

        /// <summary>
        /// Targets selected by player.
        /// </summary>
        public List<Entity> m_SelectedTargets;

        /// <summary>
        /// Flag that shows if result was processed.
        /// </summary>
        public bool m_IsProcessed;

        /// <summary>
        /// Time in sec given to process result, until it expire;
        /// </summary>
        public float m_ExpireIn;
    }
}


