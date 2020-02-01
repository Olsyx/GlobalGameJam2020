using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GGJ.Utils { 
    public class AudioMixer : MonoBehaviour {
        public GameObject sourcesObject;

        private List<AudioSource> sources = new List<AudioSource>();
        private float transitionTime;
        private float pivot;
        private int selected = 0;
        private int mixedTo = 0;

        private void Awake() {
            sources = sourcesObject.GetComponentsInChildren<AudioSource>().ToList();
        }

        public void Select(int index) {
            selected = index;
        }

        public void MixWith(int index) {
            mixedTo = index;
            StartCoroutine(ChangeTrack());
        }

        IEnumerator ChangeTrack() {
            AudioSource selectedSource = sources[selected];
            AudioSource mixedToSource = sources[mixedTo];

            mixedToSource.volume = 0f;
            mixedToSource.Play();

            while (selectedSource.time / selectedSource.clip.length < pivot) {
                yield return new WaitForFixedUpdate();
            }

            float count = transitionTime;
            while (count > 0) {
                count -= Time.deltaTime;
                float percentage = 1 - count / transitionTime;
                selectedSource.volume = 1 - percentage;
                mixedToSource.volume = percentage;
                yield return new WaitForFixedUpdate();
            }

            while (mixedToSource.volume < 1) {
                mixedToSource.volume += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }

            selectedSource.Stop();
        }

        public void SetTransitionTime(float time) {
            transitionTime = time;
        }

        public void SetPivot(float pivot) {
            this.pivot = pivot;
        }

        public void SetLoop(bool value) {
            sources[selected].loop = value;
        }

        public void Play(AudioClip clip) {
            Set(clip);
            sources[selected].Play();
        }

        public void Stop(int index) {
            sources[index].Stop();
        }

        public void Set(AudioClip clip) {
            sources[selected].clip = clip;
        }

    }
}