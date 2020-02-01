using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GGJ.IO {
    public interface IReceiverListener {
        void Receive(Transmitter source, string action);
    }

    public class Receiver : MonoBehaviour {

        [SerializeField] protected string id;
        [SerializeField] protected bool acceptAnySource;
        [SerializeField] protected List<string> acceptedTransmitters = new List<string>() { "LeftHand", "RightHand" };

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
        
        public virtual void Receive(Transmitter source, string action) {
            if (!enabled || !IsValidTransmitter(source)) {
                return;
            }

            for (int i = 0; i < listeners.Count; i++) {
                listeners[i].Receive(source, action);
            }
        }

        public bool IsValidTransmitter(Transmitter source) {
            if (acceptAnySource) {
                return true;
            }

            return source != null && acceptedTransmitters.Contains(source.Id);
        }

    }
}