using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ.Utils {
    public class FakeChild : MonoBehaviour {
        public Transform fakeParent;
        public Vector3 localPosition;
        public Vector3 localRotation;

        private void Update() {
            this.transform.position = fakeParent.TransformPoint(localPosition);
            this.transform.rotation = fakeParent.rotation * Quaternion.Euler(localRotation);
        }
    }
}