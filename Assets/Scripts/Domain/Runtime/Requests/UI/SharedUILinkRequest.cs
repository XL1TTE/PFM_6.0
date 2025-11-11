using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Domain.UI.Widgets;

namespace Domain.UI.Requests
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct SharedUILinkRequest : IRequestData
    {
        public FullscreenNotification ref_FullScreenNotification;
        public FpsCounter ref_FpsCounter;
    }
}

