using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using TMPro;

namespace Domain.UI.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct TextMeshProRefComponent : IComponent
    {
        public TextMeshProUGUI TMP;
    }
}

