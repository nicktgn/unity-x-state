namespace LobstersUnited.UnityXState {
    
    public interface IStateSwitchCondition<T> where T : IStateContext {

        public bool Check(T ctx);

    }
    
}
