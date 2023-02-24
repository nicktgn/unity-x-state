using System;
using UnityEngine;

namespace LobstersUnited.UnityXState {
    
    [Serializable]
    public class StateTransitionSO<T> : ScriptableObject, IStateTransition<T> where T : IStateContext {
        
        IState<T> from;
        IState<T> to;
        IStateSwitchCondition<T> condition;
        
        public void Construct(IState<T> from, IState<T> to, IStateSwitchCondition<T> condition) {
            this.from = from;
            this.to = to;
            this.condition = condition;
        }

        public IState<T> From => from;

        public IState<T> To => to;

        public IStateSwitchCondition<T> Condition => condition;
    }
    
}
