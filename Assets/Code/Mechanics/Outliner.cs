using GGJ.Core;
using GGJ.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ.Mechanics {
    public class Outliner : Mechanic {
        public Shader outline;
        public float width = 0.1f;
        public Color color = Color.cyan;
        public List<MeshRenderer> renderers = new List<MeshRenderer>();
        public List<string> onActions = new List<string>() { "Enter" };
        public List<string> toggleActions = new List<string>() { "Toggle" };
        public List<string> offActions = new List<string>() { "Exit" };

        List<Material> cachedMaterials = new List<Material>();
        List<Shader> cachedShaders = new List<Shader>();

        private void OnDisable() {
            ResetShaders();
        }

        private void Awake() {
            CacheRenderers();
        }

        #region Control
        protected override void React(Transmitter source, string action) {
            if (onActions.Contains(action)) {
                On();
            } else if (offActions.Contains(action)) {
                Off();
            }
        }

        protected void CacheRenderers() {
            cachedMaterials = new List<Material>();
            for (int i = 0; i < renderers.Count; i++) {
                cachedMaterials.AddRange(renderers[i].materials);
            }

            cachedShaders = new List<Shader>();
            for (int i = 0; i < cachedMaterials.Count; i++) {
                cachedShaders.Add(cachedMaterials[i].shader);
            }
        }

        protected void ResetShaders() {
            for (int i = 0; i < cachedMaterials.Count; i++) {
                cachedMaterials[i].shader = cachedShaders[i];
            }
        }
        #endregion

        #region Actions
        public void On() {
            if (!enabled) {
                return;
            }

            for (int i = 0; i < cachedMaterials.Count; i++) {
                cachedMaterials[i].shader = outline;
                cachedMaterials[i].SetColor("_OutlineColor", color);
                cachedMaterials[i].SetFloat("_Outline", width);
            }
        }

        public void Off() {
            if (!enabled) {
                return;
            }
            ResetShaders();
        }
        #endregion
    }
}