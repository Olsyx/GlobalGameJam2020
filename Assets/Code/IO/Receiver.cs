using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GGJ.IO {
    public interface IReceiverListener {
        void Receive(string action);
    }

    public class Receiver : MonoBehaviour {

        [SerializeField] protected string id;
        [SerializeField] protected bool acceptAnySource;
        [SerializeField] protected List<string> acceptedTransmitters = new List<string>() { "Player" };

        public string Id { get => id; }

        protected List<IReceiverListener> listeners = new List<IReceiverListener>();

        public void Register(IReceiverListener listener) {
            if (listeners.Contains(listener)) {
                return;
            }
            listeners.Add(listener);
        }

        public void Remove(IReceiverListener listener) {
            listeners.Remove(listener);
        }
        
        public virtual void Receive(string action) {
            if (!enabled) {
                return;
            }

            for (int i = 0; i < listeners.Count; i++) {
                listeners[i].Receive(action);
            }
        }


    }
}