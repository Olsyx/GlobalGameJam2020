using GGJ.Cameras;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GGJ.Player {
    public class Menu : MonoBehaviour {
        public string openButton = "Escape";

        [Header("Menu Settings")]
        public GameObject menu;
        public GameObject startButton;
        public GameObject aim;

        [Header("Player Settings")]
        public Transform player;
        public CameraEffects cameraControl;
        public PlayerMovement playerMovement;
        public MovementTransmitter movementTransmitter;

        [Header("Game Settings")]
        public Transform startPoint;

        private float alpha;

        private void Awake() {
            startPoint = startPoint ?? this.transform;
        }

        private void Start() {
            aim.SetActive(false);
            playerMovement.enabled = false;
            movementTransmitter.enabled = false;
        }

        private void FixedUpdate() {
            if (startButton.activeInHierarchy) {
                return;
            }

            if (!Input.GetButtonUp(openButton)) {
                return;
            }

            menu.SetActive(!menu.activeInHierarchy);
        }

        public void StartGame() {
            startButton.SetActive(false);
            menu.SetActive(false);
            cameraControl.FadeOut();
            Invoke("SetUpPlayer", cameraControl.fadeTime * 2f);
        }

        private void SetUpPlayer() {
            player.position = startPoint.position;
            player.rotation = startPoint.rotation;

            aim.SetActive(true);
            playerMovement.enabled = true;
            movementTransmitter.enabled = true;
            cameraControl.FadeIn();
        }
        
        public void Exit() {
            Application.Quit();
        }
    }
}