using System;

namespace LobstersUnited.UnityXState {
    
    public class StateSwitchCondition<T> : IStateSwitchCondition<T> where T : IStateContext {

        readonly Func<T, bool> checkFn;

        public StateSwitchCondition(Func<T, bool> checkFn) {
            this.checkFn = checkFn;
        }

        public bool Check(T ctx) {
            return checkFn.Invoke(ctx);
        }
    }
    
}
