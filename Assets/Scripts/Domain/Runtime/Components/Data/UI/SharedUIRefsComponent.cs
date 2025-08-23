using Domain.UI.Widgets;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.UI.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct SharedUIRefsComponent : IComponent
    {
        public UI_FullscreenNotification FullScreenNotification;
        public FpsCounter FpsCounter;
    }
}

