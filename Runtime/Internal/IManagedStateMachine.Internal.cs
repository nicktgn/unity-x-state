using System.Collections.Generic;

namespace LobstersUnited.UnityXState {
    
    internal interface IManagedStateMachine<T> where T : IStateContext {
        
        public IState<T> InitialState { get; }

        public IState<T> CurrentState { get; set; }

        public void SwitchState(IState<T> state, T ctx);

        public List<IStateTransition<T>> SetupStates(T ctx);

        public void Update(T ctx);
        
    }
}
