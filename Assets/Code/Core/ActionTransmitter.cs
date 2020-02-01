using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GGJ.IO {
    [RequireComponent(typeof(Rigidbody))]
    public class ActionTransmitter : Transmitter {

        public List<string> actions = new List<string>();

        #region Mono Behaviour
        private void OnTriggerEnter(Collider other) {
            Receiver newReceiver = other.GetComponentInParent<Receiver>();
            if (newReceiver == null) {
                return;
            }

            Register(newReceiver);
            SendAction(enterActions, newReceiver);
        }

        private void FixedUpdate() {
            SendActions(hoverActions);
        }

        private void OnTriggerExit(Collider other) {
            Receiver newReceiver = other.GetComponentInParent<Receiver>();
            if (newReceiver == null) {
                return;
            }

            Remove(newReceiver);
            SendAction(exitActions, newReceiver);
        }
        #endregion

        public void SendActions() {
            SendActions(this.actions);
        }

        public void SendDelayedActions(float delay) {
            Invoke("SendActions", delay);
        }
    }
}