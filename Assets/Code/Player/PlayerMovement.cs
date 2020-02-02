using GGJ.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ.Player { 
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class PlayerMovement : Receiver {
        public enum Direction {
            Forward, Backwards, Left, Right, Up, Down
        }

        public Transmitter source;
        public float movementSpeed = 1f;
        public float mouseSensitivity = 1f;

        private Rigidbody body;
        private float rotationY = 0f;

        #region Mono Behaviour
        private void Awake() {
            StoreProperties();
            Init();
        }

        private void Start() {
            StoreReferences();
            SetUp();
        }

        private void FixedUpdate() {
            FollowMouse();
            body.velocity -= body.velocity / 10f;
        }
        #endregion

        #region Init
        private void StoreProperties() {
            body = GetComponent<Rigidbody>();
        }

        private void Init() {

        }

        private void StoreReferences() {

        }

        private void SetUp() {
            source.Register(this);
        }
        #endregion

        #region Control
        protected void FollowMouse() {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;
            rotationY += Input.GetAxis("Mouse Y") * mouseSensitivity;
            rotationY = Mathf.Clamp(rotationY, -60, 60);
            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0.0f);
        }

        protected void Move(Direction direction) {
            Vector3 vector = GetDirectionVector(direction);
            body.AddForce(vector * movementSpeed * 100f);
        }
        #endregion

        #region Actions
        public override void Receive(Transmitter source, string action) {
            Direction direction;
            if (Enum.TryParse(action, out direction)) {
                Move(direction);
            }
        }
        #endregion

        #region Queries
        private Vector3 GetDirectionVector(Direction direction) {
            switch (direction) {
                case Direction.Forward:
                    return transform.forward;
                case Direction.Backwards:
                    return -transform.forward;
                case Direction.Left:
                    return -transform.right;
                case Direction.Right:
                    return transform.right;
                case Direction.Up:
                    return transform.up;
                case Direction.Down:
                    return -transform.up;
            }
            return Vector3.zero;
        }

        /// <summary>
        /// Clamps a given angle in degrees into the range [0, 360]
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public float ClampAngle(float degrees) {
            float correctAngle = degrees % 360f;

            if (correctAngle < 0) {
                correctAngle = correctAngle + 360f;
            }

            return correctAngle;
        }

        /// <summary>
        /// Clamps a given angle in degrees into the range [0, 360]
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public Vector3 ClampAngle(Vector3 angular) {
            return new Vector3(ClampAngle(angular.x),
                               ClampAngle(angular.y),
                               ClampAngle(angular.z));
        }
        #endregion

        #region Debug
        #endregion
    }
}