﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GGJ.IO {
    public class PlayerInput : Transmitter {

        [Header("Behaviour")]
        public Transform camera;
        public Transform aim;
        public float maxDistance = 2.5f;

        [Header("Transmitters")]
        public MovementTransmitter movement;
        public HandTransmitter leftHand;
        public HandTransmitter rightHand;

        public GameObject CurrentTarget { get; protected set; }
        public Receiver CurrentReceiver { get; protected set; }

        #region Mono Behaviour
        private void Update() {
            if (!enabled) {
                return;
            }
            FindTarget();
            ManageUnityInput();
        }
        #endregion

        #region Control
        private void FindTarget() {
            RaycastHit hit;
            if (!Physics.Raycast(camera.position, GetAimVector(), out hit, maxDistance)) {
                StoreReceiver(null);
                CurrentTarget = hit.collider?.gameObject;
                return;
            }

            CurrentTarget = hit.collider.gameObject;
            Receiver newReceiver = CurrentTarget.GetComponentInParent<Receiver>();
            StoreReceiver(newReceiver);
        }

        private void StoreReceiver(Receiver newReceiver) {
            if (CurrentReceiver == newReceiver) {
                SendAction(hoverActions, CurrentReceiver);
                return;
            }

            Remove(CurrentReceiver);
            SendAction(exitActions, CurrentReceiver);
            CurrentReceiver = newReceiver;

            if (CurrentReceiver == null) {
                return;
            }
            Register(CurrentReceiver);
            SendAction(enterActions, CurrentReceiver);
        }

        private void ManageUnityInput() {
            rightHand?.UpdateInput(CurrentReceiver);
            leftHand?.UpdateInput(CurrentReceiver);
            
            float vertical = Input.GetAxis(movement.verticalAxis);
            float horizontal = Input.GetAxis(movement.horizontalAxis);

            if (vertical > 0) {
                movement.Forward();
            } else if (vertical < 0) {
                movement.Backwards();
            }

            if (horizontal > 0) {
                movement.Right();
            } else if (horizontal < 0) {
                movement.Left();
            }

            if (Input.GetButton(movement.upButton)) {
                movement.Up();
            } else if (Input.GetButton(movement.downButton)) {
                movement.Down();
            }
        }
        #endregion

        #region Queries
        private Vector3 GetAimVector() {
            return  aim.position - camera.position;
        }
        #endregion

        #region Debug
        private void OnDrawGizmos() {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(camera.position, camera.position + GetAimVector() * maxDistance);
        }
        #endregion


    }
}