using GGJ.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ.Core {
    public abstract class Mechanic : MonoBehaviour, IReceiverListener {

        public Receiver receiver;

        #region Mono Behaviour
        private void Awake() {
            StoreProperties();
            Init();
        }

        private void Start() {
            StoreReferences();
            SetUp();
        }

        private void OnDestroy() {
            receiver.Remove(this);
        }
        #endregion

        #region Init
        protected virtual void StoreProperties() {
            receiver = receiver ?? GetComponentInParent<Receiver>();
        }

        protected virtual void Init() {

        }

        protected virtual void StoreReferences() {

        }

        protected virtual void SetUp() {
            receiver.Register(this);
        }
        #endregion

        #region Control
        public abstract void Receive(string action);
        #endregion

        #region Actions

        #endregion

        #region Queries

        #endregion

        #region Debug

        #endregion
    }
}