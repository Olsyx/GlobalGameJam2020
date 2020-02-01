using GGJ.Core;
using GGJ.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GGJ.Mechanics {
    public class HolderEvents {
        public UnityEvent OnOccupied = new UnityEvent();
        public UnityEvent OnEmptied = new UnityEvent();

        public void RemoveAllListeners() {
            OnOccupied.RemoveAllListeners();
            OnEmptied.RemoveAllListeners();
        }
    }

    [RequireComponent(typeof(Collider))]
    public class Holder : Mechanic {
        public enum States {
            Empty, Occupied
        }

        public Transform placingPoint;
        [SerializeField] protected Portable portable;
        public bool storeAnyPortable;
        public List<string> storablePortables = new List<string>();

        public List<string> emptyActions = new List<string>();
        public HolderEvents events = new HolderEvents();

        public Portable Portable { get => portable; }
        public States State { get; protected set; }

        #region Mono Behaviour
        protected override void OnDestroy() {
            base.OnDestroy();
            events.RemoveAllListeners();
        }
        #endregion

        #region Init
        protected override void SetUp() {
            base.SetUp();
            Portable target = portable;
            portable = null;
            target?.StoreSelf();
            target?.FixTo(this, true);
        }
        #endregion

        #region Control
        protected override void React(Transmitter source, string action) {
            if (!emptyActions.Contains(action)) {
                return;
            }
            Empty();
        }

        protected void Place(Portable target) {
            target.Self.rotation = this.placingPoint.rotation * Quaternion.Inverse(portable.Self.localRotation);
            Vector3 positionOffset = portable.placingPoint.position - portable.Self.position;
            target.Self.position = this.placingPoint.position - positionOffset;
        }
        #endregion

        #region Actions
        public void Store(Portable target, bool silently = false) {
            if (!enabled || target == null) {
                return;
            }

            if (target.Holder != this) {
                target.FixTo(this, silently);
                return;
            }

            this.portable = target;
            Place(target);
            State = States.Occupied;

            if (!silently) {
                events.OnOccupied?.Invoke();
            }
        }

        public void Empty(bool silently = false) {
            if (!enabled || portable == null) {
                return;
            }

            if (portable.Holder != null) {
                portable.Free(silently);
                return;
            }

            portable = null;
            State = States.Empty;
            if (!silently) {
                events.OnEmptied?.Invoke();
            }
        }
        #endregion

        #region Queries
        public bool CanStore(Portable target) {
            return enabled && State == States.Empty
                   && (storeAnyPortable || storablePortables.Contains(target.receiver.Id));
        }

        public bool CanRelease() {
            return enabled;
        }
        #endregion

        #region Debug
        private void OnDrawGizmos() {
            if (Application.isPlaying || portable == null) {
                return;
            }

            portable.StoreSelf();
            Place(portable);
        }
        #endregion
    }
}