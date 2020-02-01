using GGJ.Core;
using GGJ.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ.Mechanics {
    [RequireComponent(typeof(Portable))]
    public class Tool : Transmitter, IReceiverListener {
        
        public Receiver receiver;
        public bool startDisabled = false;
        public List<string> activateActions = new List<string>() { "LeftHand", "RightHand" };
        public string interaction = "Action";
        
        private Portable portable;
                
        #region Mono Behaviour
        private void Awake() {
            StoreProperties();
            Init();
        }

        private void Start() {
            StoreReferences();
            SetUp();
            enabled = !startDisabled;
        }

        protected virtual void OnDestroy() {
            receiver?.Remove(this);
        }
        #endregion

        #region Init
        protected virtual void StoreProperties() {
            receiver = receiver ?? GetComponentInParent<Receiver>();
            portable = GetComponent<Portable>();
        }

        protected virtual void Init() {

        }

        protected virtual void StoreReferences() {

        }

        protected virtual void SetUp() {
            receiver?.Register(this);
        }
        #endregion

        #region Control
        public void Receive(Transmitter source, string action) {
            if (!enabled || !IsGrabbed() || !activateActions.Contains(action)) {
                return;
            }

            Receiver target = portable.Player.CurrentReceiver;
            SendAction(interaction, target);
        }
        #endregion

        #region Actions

        #endregion

        #region Queries
        public bool IsGrabbed() {
            return portable.Location == Portable.Locations.Fixed && portable.Player != null;
        }

        #endregion

        #region Debug

        #endregion
    }
}