﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GGJ.IO {
    public abstract class Transmitter : MonoBehaviour {

        [SerializeField] protected string id;
        public List<string> enterActions = new List<string>() { "Enter" };
        public List<string> hoverActions = new List<string>() { "Hover" };
        public List<string> exitActions = new List<string>() { "Exit" };

        public string Id { get => id; }
        protected List<Receiver> receivers = new List<Receiver>();
             
        public void Register(Receiver receiver) {
            if (receivers.Contains(receiver)) {
                return;
            }
            receivers.Add(receiver);
        }

        public void Remove(Receiver receiver) {
            receivers.Remove(receiver);
        }

        public void SendAction(List<string> actions) {
            if (!enabled) {
                return;
            }
            for (int i = 0; i < receivers.Count; i++) {
                SendAction(actions, receivers[i]);
            }
        }

        public void SendAction(string action) {
            if (!enabled) {
                return;
            }
            for (int i = 0; i < receivers.Count; i++) {
                SendAction(action, receivers[i]);
            }
        }

        public void SendAction(List<string> actions, Receiver target) {
            if (!enabled || target == null) {
                return;
            }
            for (int i = 0; i < actions.Count; i++) {
                SendAction(actions[i], target);
            }
        }
        public void SendAction(string action, Receiver target) {
            if (!enabled || target == null) {
                return;
            }
            target.Receive(this, action);
        }
    }
}