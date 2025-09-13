using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.Stats.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct Actions : IComponent
    {
        public int MaxMoveActions;
        public int AvaibleMoveActions;
        public int MaxInteractionActions;
        public int AvaibleInteractionActions;
        
        public void ResetActions(){
            this.AvaibleMoveActions = this.MaxMoveActions;
            this.AvaibleInteractionActions = this.MaxInteractionActions;
        }
    }
}


