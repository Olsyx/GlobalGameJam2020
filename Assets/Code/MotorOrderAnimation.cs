using GGJ.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GGJ.Behaviours {

    public class MotorOrderAnimation : MonoBehaviour {
        public List<MaterialChanger> slots = new List<MaterialChanger>();
        public List<AudioSource> sounds = new List<AudioSource>();
        public List<float> delays = new List<float>() { 1, 1, 1, 1, 1 };
        public List<int> order = new List<int>() { 0, 4, 1, 3, 2 };
        public UnityEvent OnFinished = new UnityEvent();

        public void Play() {
            float delay = 0;
            for (int i = 0; i < order.Count; i++) {
                StartCoroutine(Show(order[i], delay));
                delay += delays[order[i]];
            }
            Invoke("Finished", delay);
        }

        IEnumerator Show(int index, float addedDelay) {
            yield return new WaitForSeconds(addedDelay);
            slots[index].SetEmissive(1);
            sounds[index].Play();
            yield return new WaitForSeconds(delays[index]);
            slots[index].SetEmissive(0);
        }

        void Finished() {
            OnFinished?.Invoke();
        }
    }

}