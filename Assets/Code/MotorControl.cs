using GGJ.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GGJ.Behaviours { 
    public class MotorControl : MonoBehaviour {
        public Selector lightSwitch;
        public List<Holder> holders = new List<Holder>();
        public List<int> order = new List<int>() { 0, 4, 1, 3, 2 };
        public UnityEvent OnLightsOn = new UnityEvent();
        public UnityEvent OnLightsOff = new UnityEvent();
        public UnityEvent OnFinished = new UnityEvent();

        int progress = 0;

        private void Awake() {
            lightSwitch.OnStateChanged.AddListener(LightSwitchChanged);
            for (int i = 0; i < holders.Count; i++) {
                holders[i].events.OnOccupied.AddListener(HolderOccupied);
            }
        }

        void LightSwitchChanged() {
            if (lightSwitch.CurrentState == "On") {
                if (progress < order.Count) {
                    EmptyAll();
                }
                OnLightsOn?.Invoke();
            } else {
                OnLightsOff?.Invoke();
            }
        }
        
        void HolderOccupied(Holder holder) {
            if (progress >= order.Count) {
                return;
            }

            if (lightSwitch.CurrentState == "On") {
                EmptyAll();
                return;
            }

            int index = holders.FindIndex(h => h == holder);
            if (index != order[progress]) {
                EmptyAll();
                return;
            }

            holder.enabled = false;
            progress++;
            if (progress >= order.Count) {
                OnFinished?.Invoke();
            }
        }

        void EmptyAll() {
            progress = 0;
            for (int i = 0; i < holders.Count; i++) {
                holders[i].enabled = true;
                holders[i].Empty();
            }
        }

    }
}