using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace LobstersUnited.UnityXState {
    
    internal class StateMachineNotStartedException : Exception {

        public StateMachineNotStartedException() 
            : base("[StateMachine ERROR] Start() method of state machine was never called.") { }
    }
    
    internal class StateMachineTransitionException : Exception {

        public StateMachineTransitionException(MemberInfo conditionType, Tuple<Type, Type> original, Tuple<Type, Type> offender) 
            : base($"[StateMachine ERROR] Same condition {conditionType.Name} is encountered in two state transitions: {original} and {offender}. Each transition should have a unique condition.") { }
    }

    internal class StateManager<T> where T : IStateContext {

        bool hasThrownNotStaredError = false;

        readonly IManagedStateMachine<T> stateMachine;

        readonly List<IStateTransition<T>> stateTransitions;

        List<IStateTransition<T>> transitionsFromCurrentState;

        public StateManager(IManagedStateMachine<T> stateMachine, T ctx) {
            this.stateMachine = stateMachine;
            stateTransitions = stateMachine.SetupStates(ctx);
            VerifyStateTransitions();
        }
        
        public bool HasStarted { get; private set; } = false;

        public void SwitchState(IState<T> state, T ctx) {
            // can start with null state
            stateMachine.CurrentState?.ExitState(ctx);

            stateMachine.CurrentState = state;
            
            // if this state is a Super State, start it's internal state machine
            var superState = stateMachine.CurrentState as IStateMachine<T>;
            superState?.Start(ctx);
                
            transitionsFromCurrentState = FilterCurrentStateTransitions();
            stateMachine.CurrentState.EnterState(ctx);
        }

        public void Start(T ctx) {
            if (HasStarted)
                return;
            HasStarted = true;
            SwitchState(stateMachine.InitialState, ctx);
        }

        public void Update(T ctx) {
            if (hasThrownNotStaredError)
                return;
            if (!HasStarted || stateMachine.CurrentState == null) {
                hasThrownNotStaredError = true;
                throw new StateMachineNotStartedException();
            }
            
            // do own update
            var currentState = stateMachine.CurrentState; 
            if (currentState.NeedsUpdate) {
                stateMachine.CurrentState.UpdateState(ctx);
            }
            
            // if this state is a Super State, update it's nested state machine
            var nestedSM = currentState as IStateMachine<T>;
            nestedSM?.Update(ctx);
            
            // handle transitions
            TransitionStates(ctx);
        }

        void TransitionStates(T ctx) {
            foreach (var t in transitionsFromCurrentState) {
                // TODO: test that t.From is always == stateMachine.CurrentState
                if (t.Condition.Check(ctx)) {
                    SwitchState(t.To, ctx);
                    break;
                }
            }
        }

        void VerifyStateTransitions() {
            if (stateTransitions == null || stateTransitions.Count == 0)
                return;
            
            var statePairDict = new Dictionary<Tuple<Type, Type>, IStateTransition<T>>();
            var cDict = new Dictionary<IStateSwitchCondition<T>, Tuple<Type, Type>>();

            var i = stateTransitions.Count - 1;
            while (i >= 0) {
                var t = stateTransitions[i];
                
                var pair = Tuple.Create(t.From.GetType(), t.To.GetType());
                if (statePairDict.ContainsKey(pair)) {
                    Debug.LogWarning($"[StateMachine] Duplicate state transition found: {t}. Removing...");
                    stateTransitions.RemoveAt(i);
                } else {
                    statePairDict.Add(pair, t);
                    
                    var condPair = cDict.GetValueOrDefault(t.Condition);
                    if (condPair != null && pair.Equals(condPair)) {
                        throw new StateMachineTransitionException(t.Condition.GetType(), condPair, pair);
                    }
                }

                i--;
            }
        }

        List<IStateTransition<T>> FilterCurrentStateTransitions() {
            return stateTransitions
                .Where(t => t.From == stateMachine.CurrentState)
                .ToList();
        }
    }
}
