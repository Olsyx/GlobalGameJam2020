using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GGJ.IO {
    public class PlayerInput : Transmitter {

        [Header("Behaviour")]
        public Transform aim;
        public float maxDistance = 2.5f;

        [Header("Buttons")]
        public string leftHandButton = "LeftClick";
        public string leftHandInteraction = "LeftHand";
        public string rightHandButton = "RightClick";
        public string rightHandInteraction = "RightHand";
        public string upButton = "Up";
        public string downButton = "Down";

        Receiver cachedReceiver;

        #region Mono Behaviour
        private void FixedUpdate() {
            FindTarget();
            ManageUnityInput();
        }
        #endregion

        #region Control
        private void FindTarget() {
            RaycastHit hit;
            if (!Physics.Raycast(transform.position, GetAimVector(), out hit, maxDistance)) {
                return;
            }

            Receiver newReceiver = hit.collider.GetComponentInParent<Receiver>(); ;

            if (cachedReceiver == newReceiver) {
                SendAction(hoverActions, cachedReceiver);
                return;
            }

            Remove(cachedReceiver);
            SendAction(exitActions, cachedReceiver);
            cachedReceiver = newReceiver;
            Register(cachedReceiver);
            SendAction(enterActions, cachedReceiver);
        }

        private void ManageUnityInput() {
            if (Input.GetButtonUp(rightHandButton)) {
                SendAction(rightHandInteraction);
            }

            if (Input.GetButtonUp(leftHandButton)) {
                SendAction(leftHandInteraction);
            }

            float vertical = Input.GetAxis("Vertical");
            float horizontal = Input.GetAxis("Horizontal");

            if (vertical > 0) {
                SendAction("Forward");
            } else if (vertical < 0) {
                SendAction("Backwards");
            }

            if (horizontal > 0) {
                SendAction("Right");
            } else if (horizontal < 0) {
                SendAction("Left");
            }

            if (Input.GetButton(upButton)) {
                SendAction("Up");
            } else if (Input.GetButton(downButton)) {
                SendAction("Down");
            }
        }
        #endregion

        #region Queries
        private Vector3 GetAimVector() {
            return  aim.position - transform.position;
        }
        #endregion

        #region Debug
        private void OnDrawGizmos() {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + GetAimVector() * maxDistance);
        }
        #endregion


    }
}