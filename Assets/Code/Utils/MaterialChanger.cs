using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ.Utils { 
    public class MaterialChanger : MonoBehaviour {
        public MeshRenderer renderer;
        public List<Color> colors = new List<Color>();

        public void SetColor(int option) {
            renderer.material.SetColor("_TintColor", colors[option]);
        }
        public void SetEmissive(int option) {
            Debug.Log("Emissive color: " + option);

            renderer.material.SetColor("_EmissionColor", colors[option]);
        }
    }
}