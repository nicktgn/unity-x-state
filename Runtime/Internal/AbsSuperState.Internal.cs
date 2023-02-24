namespace LobstersUnited.UnityXState {
    
    public partial class AbsSuperState<T> : IManagedStateMachine<T> where T : IStateContext {
        
        IState<T> IManagedStateMachine<T>.CurrentState { get; set; }
        
    }
}
