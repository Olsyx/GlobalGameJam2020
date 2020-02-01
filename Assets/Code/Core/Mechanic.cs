using GGJ.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GGJ.Core {
    [Serializable]
    public class MechanicEvents {
        public UnityEvent OnEnabled = new UnityEvent();
        public UnityEvent OnDisabled = new UnityEvent();

        public void RemoveAllListeners() {
            OnEnabled.RemoveAllListeners();
            OnDisabled.RemoveAllListeners();
        }
    }

    public abstract class Mechanic : MonoBehaviour, IReceiverListener {

        public Receiver receiver;
        public bool startDisabled = false;
        public List<string> enableActions = new List<string>() { "Enable" };
        public List<string> disableActions = new List<string>() { "Disable" };
        public MechanicEvents ablement = new MechanicEvents();

        private bool initFlag = true;

        #region Mono Behaviour
        private void OnEnable() {
            ablement.OnEnabled?.Invoke();
        }

        private void OnDisable() {
            if (initFlag) {
                return;
            }

            ablement.OnDisabled?.Invoke();
            initFlag = false;
        }

        private void Awake() {
            StoreProperties();
            Init();
        }

        private void Start() {
            StoreReferences();
            SetUp();
            enabled = !startDisabled;
            initFlag = startDisabled;
        }

        protected virtual void OnDestroy() {
            receiver?.Remove(this);
            ablement.RemoveAllListeners();
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
            receiver?.Register(this);
        }
        #endregion

        #region Control
        public void Receive(Transmitter source, string action) {
            if (!enabled) {
                if (enableActions.Contains(action)) {
                    Enable();
                }
                return;
            }

            if (disableActions.Contains(action)) {
                Disable();
                return; 
            }

            React(source, action);
        }

        protected abstract void React(Transmitter source, string action);
        #endregion

        #region Actions
        public void Enable() {
            enabled = true;
        }

        public void Disable() {
            enabled = false;
        }
        #endregion

        #region Queries

        #endregion

        #region Debug

        #endregion
    }
}