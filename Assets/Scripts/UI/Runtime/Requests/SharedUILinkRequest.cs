using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using UI.Widgets;

namespace UI.Requests{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct SharedUILinkRequest : IRequestData
    {
        public UI_FullscreenNotification ref_FullScreenNotification;
    }
}

