using System;
using UnityEngine;


namespace LobstersUnited.UnityXState {

    [Serializable]
    public class StateTransition<T> : IStateTransition<T> where T : IStateContext {
        IState<T> from;
        IState<T> to;

        IStateSwitchCondition<T> condition;

        readonly int hashCode;

        public StateTransition(IState<T> from, IState<T> to, IStateSwitchCondition<T> condition) {
            this.from = from;
            this.to = to;
            this.condition = condition;

            hashCode = Tuple.Create(From, To, Condition).GetHashCode();
        }

        public IState<T> From => from;

        public IState<T> To => to;

        public IStateSwitchCondition<T> Condition => condition;

        public override bool Equals(object obj) {
            var t = obj as IStateTransition<T>;
            if (t == null)
                return false;

            return t.From == From && t.To == To && t.Condition == Condition;
        }

        public override int GetHashCode() {
            return hashCode;
        }

        public override string ToString() {
            return $"StateTransition {{ From = \"{From.GetType().Name}\", To = \"{To.GetType().Name}\", Condition = \"{Condition.GetType().Name}\" }}";
        }
    }
}
