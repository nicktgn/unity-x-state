namespace LobstersUnited.UnityXState {
    
    public partial class AbsStateMachine<T> : IManagedStateMachine<T> where T : IStateContext {
        
        IState<T> IManagedStateMachine<T>.CurrentState { get => currentState; set => currentState = value; }
        
    }
}
