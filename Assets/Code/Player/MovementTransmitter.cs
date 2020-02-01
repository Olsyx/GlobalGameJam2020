using GGJ.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ.Player {
    public class MovementTransmitter : Transmitter {
        public string horizontalAxis = "Horizontal";
        public string verticalAxis = "Vertical";
        public string upButton = "Up";
        public string downButton = "Down";

        public void Forward() {
            SendAction("Forward");
        }

        public void Backwards() {
            SendAction("Backwards");
        }

        public void Right() {
            SendAction("Right");
        }

        public void Left() {
            SendAction("Left");
        }

        public void Up() {
            SendAction("Up");
        }

        public void Down() {
            SendAction("Down");
        }
    }
}