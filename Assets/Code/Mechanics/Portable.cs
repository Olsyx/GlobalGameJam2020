using GGJ.Core;
using GGJ.IO;
using GGJ.Player;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GGJ.Mechanics {
    [Serializable]
    public class PortableEvents {
        public UnityEvent OnFixed = new UnityEvent();
        public UnityEvent OnFreed = new UnityEvent();

        public void RemoveAllListeners() {
            OnFixed.RemoveAllListeners();
            OnFreed.RemoveAllListeners();
        }
    }

    [RequireComponent(typeof(Rigidbody))]
    public class Portable : Mechanic {
        public enum Locations {
            Free, Fixed
        }

        public bool useGravity = true;
        public Transform placingPoint;
        public List<string> fixActions = new List<string>() { "LeftHand", "RightHand" };
        public List<string> freeActions = new List<string>() { "LeftHand_Long", "RightHand_Long" };
        public PortableEvents events = new PortableEvents();

        public Holder Holder { get => holder; }
        public Transform Self { get; protected set; }
        public Locations Location { get; protected set; }
        public PlayerInput Player { get; protected set; }

        private Rigidbody body;
        private Holder holder;
        private Transmitter holderTransmitter;

        #region Mono Behaviour

        protected void Update() {
            if (useGravity || Location == Locations.Fixed) {
                return;
            }

            body.velocity -= body.velocity * Time.deltaTime;
        }

        protected override void OnDestroy() {
            base.OnDestroy();
            events.RemoveAllListeners();
        }
        #endregion

        #region Init
        protected override void StoreProperties() {
            base.StoreProperties();
            body = GetComponent<Rigidbody>();
            placingPoint = placingPoint ?? this.transform;
        }

        protected override void StoreReferences() {
            base.StoreReferences();
            StoreSelf();
        }
        public void StoreSelf() {
            Self = receiver != null ? receiver.transform : this.transform;
        }
        #endregion

        #region Control
        protected override void React(Transmitter source, string action) {
            if (freeActions.Contains(action)) {
                Free();
                return;
            }

            if (fixActions.Contains(action)) {
                FixTo(source);
            }
            
        }

        protected void CleanReferences() {
            holderTransmitter?.Remove(this.receiver);

            Holder lastHolder = holder;
            holder = null;
            Player = null;
            holderTransmitter = null;

            lastHolder?.Empty();
        }

        protected void StoreIn(Holder target, bool silently = false) {
            holder = target;
            Player = holder.GetComponentInParent<PlayerInput>();

            holderTransmitter = holder.GetComponentInParent<Transmitter>();
            holderTransmitter?.Register(this.receiver);

            body.isKinematic = true;
            body.useGravity = false;

            holder.Store(this, silently);
        }
        #endregion

        #region Actions
        public void FixTo(Transmitter source, bool silently = false) {
            if (!enabled || source == null) {
                return;
            }

            Holder sourceHolder = null;
            if (Player != null) {
                sourceHolder = Player.CurrentTarget?.GetComponentInChildren<Holder>();
            } else {
                sourceHolder = source.GetComponentInChildren<Holder>();
            }
            FixTo(sourceHolder, silently);
        }

        public void FixTo(Holder target, bool silently = false) {
            if (!enabled || target == null || !target.CanStore(this)) {
                return;
            }

            if (holder != null && !holder.CanRelease()) {
                return;
            }

            CleanReferences();

            Location = Locations.Fixed;
            Self.parent = target.transform;
            StoreIn(target, silently);

            if (!silently) {
                events.OnFixed?.Invoke();
            }
        }

        public void Free(bool silently = false) {
            if (!enabled) {
                return;
            }

            if (holder != null && !holder.CanRelease()) {
                return;
            }

            CleanReferences();

            Self.parent = null;
            body.isKinematic = false;
            body.useGravity = useGravity;
            Location = Locations.Free;

            if (!silently) {
                events.OnFreed?.Invoke();
            }
        }
        #endregion

        #region Queries
        #endregion

        #region Debug

        #endregion
    }
}