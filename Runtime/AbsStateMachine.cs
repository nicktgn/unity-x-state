using System;
using System.Collections.Generic;
using UnityEngine;


namespace LobstersUnited.UnityXState {

    [Serializable]
    public abstract partial class AbsStateMachine<T> : IStateMachine<T> where T : IStateContext {

        [SerializeField] StateMachineEditorHelper editorHelper;
        
        StateManager<T> stateManager;

        IState<T> currentState;

        public bool HasStarted => stateManager.HasStarted;
        
        public IState<T> CurrentState { get => currentState; internal set => currentState = value; }

        /// <summary>
        /// Override this property to assign initial state
        /// </summary>
        public abstract IState<T> InitialState { get; }
        
        public abstract List<IStateTransition<T>> SetupStates(T ctx);

        public void SwitchState(IState<T> state, T ctx) {
            stateManager.SwitchState(state, ctx);
        }
        
        public virtual void Start(T ctx) {
            editorHelper = new StateMachineEditorHelper();
            editorHelper.Init(this);
            
            stateManager = new StateManager<T>(this, ctx);
            stateManager.Start(ctx);
        }
        
        public void Update(T ctx) {
            stateManager.Update(ctx);
        }
    }
}
