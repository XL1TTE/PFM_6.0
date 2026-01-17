using Unity.IL2CPP.CompilerServices;

namespace Domain.StateMachine.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct PlayerTurnState : IState
    {
    }
}


