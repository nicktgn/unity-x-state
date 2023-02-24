namespace LobstersUnited.UnityXState {
    
    public interface IState<in T> where T : IStateContext {
        
        public bool NeedsUpdate { get; }
        
        public void EnterState(T ctx);

        public void UpdateState(T ctx);

        public void ExitState(T ctx);
        
    }
}
