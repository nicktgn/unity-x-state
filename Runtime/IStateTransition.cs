namespace LobstersUnited.UnityXState {
    
    public interface IStateTransition<T> where T : IStateContext {

        public IState<T> From { get; }

        public IState<T> To { get; }

        public IStateSwitchCondition<T> Condition { get; }
    }
}
