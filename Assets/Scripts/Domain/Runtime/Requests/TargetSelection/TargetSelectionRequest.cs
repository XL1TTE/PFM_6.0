using System;
using System.Collections.Generic;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Domain.TargetSelection.Requests
{

    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct TargetSelectionRequest : IRequestData
    {

        /// <summary>
        /// Request sender.
        /// </summary>
        public Entity m_Sender;
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
    }
}


