using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GGJ.Utils {
    public class Transformer : MonoBehaviour {
        protected enum Modes {
            Position, LocalPosition, Rotation, LocalRotation, Scale
        }

        public Transform target;
        public float time = 0;
        public List<Vector3> options = new List<Vector3>();
        public UnityEvent OnFinished = new UnityEvent();

        protected float count;
        protected Modes mode;
        protected Vector3 from;
        protected Vector3 to;
        protected Quaternion fromRotation;
        protected Quaternion toRotation;

        #region Mono Behaviour
        private void Update() {
            if (!enabled || count <= 0) {
                return;
            }

            count -= Time.deltaTime;
            Move();

            if (count <= 0) {
                Finish();
            }
        }
        #endregion

        #region Control
        private void Move() {
            float step =  1 - count / time;
            switch (mode) {
                case Modes.Position:
                    target.position = Vector3.Lerp(from, to, step);
                    break;
                case Modes.LocalPosition:
                    target.localPosition = Vector3.Lerp(from, to, step);
                    break;
                case Modes.Rotation:
                    target.rotation = Quaternion.Lerp(fromRotation, toRotation, step);
                    break;
                case Modes.LocalRotation:
                    target.localRotation = Quaternion.Lerp(fromRotation, toRotation, step);
                    break;
                case Modes.Scale:
                    target.localScale = Vector3.Lerp(from, to, step);
                    break;
            }
        }

        private void Finish() {
            switch (mode) {
                case Modes.Position:
                    target.position = to;
                    break;
                case Modes.LocalPosition:
                    target.localPosition = to;
                    break;
                case Modes.Rotation:
                    target.rotation = toRotation;
                    break;
                case Modes.LocalRotation:
                    target.localRotation = toRotation;
                    break;
                case Modes.Scale:
                    target.localScale = to;
                    break;
            }
            OnFinished?.Invoke();
        }
        #endregion

        #region Actions
        public void SetPosition(int option) {
            if (!enabled) {
                return;
            }

            mode = Modes.Position;
            from = target.position;
            to = options[option];

            if (time > 0) {
                count = time;
                return;
            }

            target.position = to;
            OnFinished?.Invoke();
        }

        public void SetLocalPosition(int option) {
            if (!enabled) {
                return;
            }

            mode = Modes.LocalPosition;
            from = target.localPosition;
            to = options[option];

            if (time > 0) {
                count = time;
                return;
            }

            target.localPosition = to;
            OnFinished?.Invoke();
        }

        public void SetRotation(int option) {
            if (!enabled) {
                return;
            }

            mode = Modes.Rotation;
            fromRotation = target.rotation;
            toRotation = Quaternion.Euler(options[option]);

            if (time > 0) {
                count = time;
                return;
            }

            target.rotation = toRotation;
            OnFinished?.Invoke();
        }

        public void SetLocalRotation(int option) {
            if (!enabled) {
                return;
            }

            mode = Modes.LocalRotation;
            fromRotation = target.localRotation;
            toRotation = Quaternion.Euler(options[option]);

            if (time > 0) {
                count = time;
                return;
            }

            target.localRotation = toRotation;
            OnFinished?.Invoke();
        }

        public void SetScale(int option) {
            if (!enabled) {
                return;
            }

            mode = Modes.Scale;
            from = target.localScale;
            to = options[option];

            if (time > 0) {
                count = time;
                return;
            }

            target.localScale = to;
            OnFinished?.Invoke();
        }
        #endregion

        #region Queries
        #endregion

        #region Debug
        #endregion

    }
}