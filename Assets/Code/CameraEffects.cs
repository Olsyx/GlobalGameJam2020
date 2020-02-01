using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GGJ.Cameras {

    [RequireComponent(typeof(Camera))]
    public class CameraEffects : MonoBehaviour {

        public Image fade;
        public float fadeTime = 1f;

        bool fading = false;
        float targetFade = 0f;
        float count;
        Camera camera;

        private void Awake() {
            camera = GetComponent<Camera>();
        }

        private void Update() {
            if (!fading) {
                return;
            }

            count -= Time.deltaTime;

            float step = count / fadeTime;
            Color color = fade.color;
            color.a = targetFade - step;
            fade.color = color;

            fading = count > 0f;
        }

        public void FadeIn() {
            fading = true;
            count = fadeTime;
            targetFade = 0f;
        }

        public void FadeOut() {
            fading = true;
            count = fadeTime;
            targetFade = 1f;
        }
    }

}