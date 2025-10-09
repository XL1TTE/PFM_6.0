using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.HealthBars.Requests
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct ChangeHealthBarRendererRequest : IRequestData
    {
        public Entity Owner;
        public Entity NewRenderer;
    }
}
