using System.Collections.Generic;

namespace LobstersUnited.UnityXState {

    public interface IStateMachine<T> where T : IStateContext {

        public bool HasStarted { get; }

        public IState<T> InitialState { get; }

        public IState<T> CurrentState { get; }

        // TODO: Consider if this interface needed
        public void SwitchState(IState<T> state, T ctx);
        
        public List<IStateTransition<T>> SetupStates(T ctx);

        public void Start(T ctx);
        
        public void Update(T ctx);
    }
}
