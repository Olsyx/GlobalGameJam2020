using GGJ.Core;
using GGJ.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ.Mechanics {
    [RequireComponent(typeof(Portable))]
    public class Tool : Transmitter, IReceiverListener {
        
        public Receiver receiver;
        public List<string> activateActions = new List<string>() { "LeftHand", "RightHand" };
        public string interaction = "Action";
        
        private Portable portable;

        #region Mono Behaviour
        protected virtual void OnDestroy() {
            receiver?.Remove(this);
        }
        #endregion

        #region Init
        protected override void StoreProperties() {
            base.StoreProperties();
            receiver = receiver ?? GetComponentInParent<Receiver>();
            portable = GetComponent<Portable>();
        }

        protected override void Init() {
            base.Init();

        }

        protected override void StoreReferences() {
            base.StoreReferences();

        }

        protected override void SetUp() {
            base.SetUp();
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