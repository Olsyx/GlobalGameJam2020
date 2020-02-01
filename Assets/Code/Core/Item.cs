using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ.Core {
    public class Item : MonoBehaviour {
        [SerializeField] protected string id = "Item";
        public string Id { get => id; }
    }

}