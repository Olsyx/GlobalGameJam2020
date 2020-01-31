using GGJ.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ.Player { 
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    public class PlayerMovement : Receiver {
        public enum Direction {
            Forward, Backwards, Left, Right, Up, Down
        }

        public Transmitter source;
        public float movementSpeed = 1f;
        public float mouseSensitivity = 1f;

        private Rigidbody body;
        private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)

        #region Mono Behaviour
        private void Awake() {
            StoreProperties();
            Init();
        }

        private void Start() {
            StoreReferences();
            SetUp();
        }

        private void Update() {
            FollowMouse();
        }

        private void FixedUpdate() {
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
            lastMouse = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            lastMouse = new Vector3(-lastMouse.y * mouseSensitivity, lastMouse.x * mouseSensitivity, 0);
            lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x, transform.eulerAngles.y + lastMouse.y, 0);
            transform.eulerAngles = lastMouse;
            lastMouse = Input.mousePosition;
        }

        protected void Move(Direction direction) {
            Vector3 vector = GetDirectionVector(direction);
            body.AddForce(vector * movementSpeed * 100f);
        }
        #endregion

        #region Actions
        public override void Receive(string action) {
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
        #endregion

        #region Debug
        #endregion
    }
}