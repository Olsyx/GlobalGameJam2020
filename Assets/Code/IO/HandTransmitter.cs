using GGJ.Mechanics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GGJ.IO {
    public class HandTransmitter : Transmitter {
        public string button = "Click";
        public string interaction = "Hand";
        public float longClick = 0.5f;
        public Holder slot;

        private float count;

        public void UpdateInput(Receiver target) {
            if (Input.GetButtonDown(button)) {
                StartCount();
            } else if (Input.GetButton(button)) {
                UpdateCount();
            }

            if (Input.GetButtonUp(button)) {
                Interact(target);
            }
        }

        protected void StartCount() {
            count = longClick;
        }

        protected void UpdateCount() {
            if (count <= 0) {
                return;
            }

            count -= Time.deltaTime;
            if (count > longClick) {
                return;
            }
            Interact(null, true);
        }

        public void Interact(Receiver target, bool isLong = false) {
            string action = interaction;
            if (isLong) {
                action += "_Long";
            }

            SendAction(action);

            if (target != null && slot.State == Holder.States.Empty) {
                SendAction(action, target);
            }
        }
    }
}