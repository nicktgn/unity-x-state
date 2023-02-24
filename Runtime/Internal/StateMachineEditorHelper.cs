using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace LobstersUnited.UnityXState {

    // TODO: make internal and expose to editor assembly
    public struct StateInfo {
        public string stateMachineName;
        public string currentStateName;

        public StateInfo(string stateMachineName, string currentStateName) {
            this.stateMachineName = stateMachineName;
            this.currentStateName = currentStateName;
        }
    }

    // TODO: make internal and expose to editor assembly
    [Serializable]
    public class StateMachineEditorHelper {

        const string CURRENT_STATE_PROP = "CurrentState";

        const BindingFlags PUBLIC_PROPS_FLAGS = BindingFlags.Instance | BindingFlags.Public;
        

        object rootStateMachine;
        Type stateMachineType;
        PropertyInfo currentStateProp;

        public void Init(object stateMachine) {
            rootStateMachine = stateMachine;
            stateMachineType = stateMachine.GetType();
            currentStateProp = stateMachineType.GetProperty(CURRENT_STATE_PROP, PUBLIC_PROPS_FLAGS);
        }

        public string GetStateMachineName() {
            return stateMachineType.Name;
        }

        public string GetCurrentStateName() {
            var currentState = currentStateProp!.GetValue(rootStateMachine);
            return currentState == null ? string.Empty : currentState.GetType().Name;
        }

        public IEnumerable<StateInfo> GetCurrentStateHierarchyInfo() {
            var sm = rootStateMachine;
            while (sm != null) {
                var info = GetStateMachineInfo(sm, out var currentState);
                if (currentState == null)
                    break;
                
                yield return info;
                
                var stateType = currentState.GetType();
                var isSuperState = stateType.IsSubclassOf(typeof(IStateMachine<>));
                if (isSuperState) {
                    sm = currentState;
                } else {
                    break;
                }
            }
        }
        
        static StateInfo GetStateMachineInfo(object stateMachine, out object outCurrentState) {
            var stateMachineType = stateMachine.GetType();
            var smName = stateMachineType.Name;
            
            var currentStateProp = stateMachineType.GetProperty(CURRENT_STATE_PROP, PUBLIC_PROPS_FLAGS);
            if (currentStateProp == null) {
                outCurrentState = null;
                return new StateInfo();
            }
            outCurrentState = currentStateProp!.GetValue(stateMachine);
            var currentStateName = outCurrentState == null ? "None" : outCurrentState.GetType().Name;

            return new StateInfo(smName, currentStateName);
        }
    }
}
