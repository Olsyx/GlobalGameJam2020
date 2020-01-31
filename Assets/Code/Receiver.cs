using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GGJ.IO {
    public class Receiver : MonoBehaviour {

        [SerializeField] protected string id;
        [SerializeField] protected bool acceptAnySource;
        [SerializeField] protected List<string> acceptedTransmitters = new List<string>() { "Player" };

        public string Id { get => id; }

        public void Receive(string action) {
            Debug.Log(action);
        }


    }
}