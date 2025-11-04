using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;
using System;

namespace Domain.TargetSelection.Tags
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct SelectionOptionTag : IComponent
    {
        /// <summary>
        /// Show if target was already selected.
        /// </summary>
        public bool m_IsSelected;
    }

    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct TargetSelectionSession : IComponent
    {

        /// <summary>
        /// Amount of targets to pick.
        /// </summary>
        public UInt16 m_TargetsAmount;

        /// <summary>
        /// Options that should be rendered as posible option, 
        /// but which player can't realy select. 
        /// </summary>
        public List<Entity> m_UnavaibleOptions;

        /// <summary>
        /// Options that will be rendered as as posible options, 
        /// and which player will be able to pick.
        /// </summary>
        public List<Entity> m_AwaibleOptions;

        /// <summary>
        /// Options which have been selected by player.
        /// </summary>
        public List<Entity> m_SelectedOptions;
    }
}

