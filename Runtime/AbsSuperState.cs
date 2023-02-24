using System;
using System.Collections.Generic;

namespace LobstersUnited.UnityXState {
    
    /// <summary>
    /// State that can have nested State Machine with internal states in it
    /// </summary>
    [Serializable]
    public abstract partial class AbsSuperState<T> : IState<T>, IStateMachine<T> where T : IStateContext {

        StateManager<T> stateManager;

        public bool HasStarted => stateManager.HasStarted;
        
        public IState<T> CurrentState { get; internal set; } = null;

        /// <summary>
        /// Override this property to assign initial sub-state
        /// </summary>
        public abstract IState<T> InitialState { get; }
        
        public abstract bool NeedsUpdate { get; }
        
        public abstract void EnterState(T ctx);
        
        public abstract void UpdateState(T ctx);

        public abstract void ExitState(T ctx);
        
        /// <summary>
        /// Override this method to setup sub-state transitions
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual List<IStateTransition<T>> SetupStates(T context) {
            return null;
        }
        
        public void Start(T ctx) {
            stateManager = new StateManager<T>(this, ctx);
            stateManager.Start(ctx);
        }

        public void SwitchState(IState<T> state, T ctx) {
            stateManager.SwitchState(state, ctx);
        }
        
        public void Update(T ctx) {
            stateManager.Update(ctx);
        }
    }
    
}
