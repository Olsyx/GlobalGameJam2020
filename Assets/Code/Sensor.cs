using GGJ.Core;
using GGJ.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GGJ {

    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class Sensor : MonoBehaviour  {
        public enum States {
            Inactive, Active
        } 
        
        public enum Modes {
            None, Any, All
        }

        public Modes mode;
        public Collider collider;
        public List<string> detectableItems = new List<string>();
        public UnityEvent OnActivated = new UnityEvent();
        public UnityEvent OnDeactivated = new UnityEvent();

        public States State { get; protected set; }

        private List<Item> items = new List<Item>();

        #region Mono Behaviour
        private void FixedUpdate() {
            EvaluateState();
        }

        private void OnTriggerEnter(Collider other) {
            Item item = other.GetComponentInParent<Item>();
            if (item == null || !detectableItems.Contains(item.Id)) {
                return;
            }

            Store(item);
            EvaluateState();
        }

        private void OnTriggerExit(Collider other) {
            Item item = other.GetComponentInParent<Item>();
            if (item == null || !detectableItems.Contains(item.Id)) {
                return;
            }

            Remove(item);
        }
        #endregion

        #region Control
        private void Store(Item item) {
            if (items.Contains(item)) {
                return;
            }

            items.Add(item);
        }

        private void Remove(Item item) {
            items.Remove(item);
        }

        private void EvaluateState() {
            bool activate = (items.Count == 0 && mode == Modes.None)
                            || (items.Count > 0 && mode == Modes.Any)
                            || (items.Count == detectableItems.Count && mode == Modes.All);

            Set(activate ? States.Active : States.Inactive);
        }

        private void Set(States newState) {
            if (newState == State) {
                return;
            }

            State = newState;
            if (State == States.Active) {
                OnActivated?.Invoke();
            } else {
                OnDeactivated?.Invoke();
            }
        }
        #endregion

        #region Debug
        private void OnDrawGizmos() {
            if (collider == null) {
                return;
            }

            Color color = !enabled ? Color.red
                              : State == States.Inactive ? Color.black 
                                : Color.green;

            GizmosUtils.DrawCollider(this.transform, collider, color);
        }
        #endregion

    }

}