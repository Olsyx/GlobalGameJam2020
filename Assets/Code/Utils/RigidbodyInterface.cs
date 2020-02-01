using GGJ.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ.Utils {

    public class RigidbodyInterface : MonoBehaviour {
        public Rigidbody body;

        private void Awake() {
            body = body ?? GetComponent<Rigidbody>();
        }

        public void AddForceFromPlayer(float force) {
            Vector3 vector = PlayerInput.Instance.GetAimVector().normalized;
            AddForce(vector * force);
        }

        public void AddForce(Vector3 force) {
            body.AddForce(force);
        }

        public void AddTorque(Vector3 force) {
            body.AddTorque(force);
        }

        public void AddRandomTorque(float force) {
            Vector3 vector = new Vector3(
                Random.Range(-1, 1),
                Random.Range(-1, 1),
                Random.Range(-1, 1)
            );
            body.AddTorque(vector * force);
        }
    }

}