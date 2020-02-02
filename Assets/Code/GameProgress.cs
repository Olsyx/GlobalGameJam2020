using GGJ.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GGJ.Behaviours {
    public class GameProgress : MonoBehaviour {
        public Selector lightSwitch;
        public MotorControl motor;

        public UnityEvent OnReadyForEnd = new UnityEvent();
        public UnityEvent OnEndPrevented = new UnityEvent();

        bool motorEnabled = false;

        private void Awake() {
            lightSwitch.OnStateChanged.AddListener(LightSwitchChanged);
            motor.OnFinished.AddListener(OnMotorFinished);
        }

        void LightSwitchChanged() {
            bool lightsOn = lightSwitch.CurrentState == "On";
            if (lightsOn && motorEnabled) {
                OnReadyForEnd?.Invoke();
            } else {
                OnEndPrevented?.Invoke();
            }
        }

        void OnMotorFinished() {
            motorEnabled = true;
        }
    }
}
