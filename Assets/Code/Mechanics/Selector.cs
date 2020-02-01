using GGJ.Core;
using GGJ.IO;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GGJ.Mechanics {
    public class Selector : Mechanic {
        [Serializable]
        public class State {
            public string id;
            public List<string> enterActions = new List<string>();
            public UnityEvent OnEntered = new UnityEvent();
            public UnityEvent OnExited = new UnityEvent();
        }
        public int startState = 0;
        public bool cycle = true;
        public List<State> states = new List<State>();
        public List<string> nextActions = new List<string>();
        public List<string> previousActions = new List<string>();

        protected int currentState = 0;

        #region Mono Behaviour
        #endregion

        #region Init
        protected override void StoreProperties() {
            base.StoreProperties();
            currentState = startState;
        }
        #endregion

        #region Control
        protected override void React(Transmitter source, string action) {
            if (nextActions.Contains(action)) {
                Next();
                return;
            }

            if (previousActions.Contains(action)) {
                Previous();
                return;
            }

            int newState = FindMatchingState(action);
            if (newState < 0) {
                return;
            }

            SelectState(newState);
        }

        protected void SelectState(int newState) {
            if (currentState == newState) {
                return;
            }

            states[currentState].OnExited?.Invoke();
            currentState = newState;
            states[currentState].OnEntered?.Invoke();
        }
        #endregion

        #region Actions
        public void Set(string state) {
            int target = states.FindIndex(s => s.id == state);
            SelectState(target);
        }

        public void Set(int state) {
            if (!enabled || state < 0 || state >= states.Count) {
                return;
            }

            SelectState(state);
        }

        public void Next() {
            if (!enabled) {
                return;
            }

            int newState = currentState + 1;
            if (cycle) {
                newState = newState < states.Count ? newState : 0;
            } else {
                newState = Mathf.Max(states.Count, newState);
            }

            SelectState(newState);
        }

        public void Previous() {
            if (!enabled) {
                return;
            }

            int newState = currentState - 1;
            if (cycle) {
                newState = newState >= 0 ? newState : states.Count - 1;
            } else {
                newState = Mathf.Min(0, newState);
            }

            SelectState(newState);
        }

        #endregion

        #region Queries
        protected int FindMatchingState(string action) {
            int i = 0;
            while (i < states.Count && !states[i].enterActions.Contains(action)) {
                i++;
            }

            return i < states.Count ? i : -1;
        }

        #endregion

        #region Debug

        #endregion
    }
}