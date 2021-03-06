﻿using GGJ.Cameras;
using GGJ.Utils;
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
        public GameObject controls;
        public GameObject credits;

        [Header("Player Settings")]
        public Transform player;
        public CameraEffects cameraControl;
        public PlayerInput input;
        public PlayerMovement playerMovement;
        public MovementTransmitter movementTransmitter;

        [Header("Game Settings")]
        public Transform startPoint;
        public Transformer moveAsteroidField;
        public Transformer rotateAsteroidField;

        [Header("Audio Settings")]
        public AudioMixer mixer;
        public AudioClip ambient;
        public AudioClip mainTheme;

        private float alpha;
        private bool finished;

        #region Mono Behaviour
        private void Awake() {
            startPoint = startPoint ?? this.transform;
        }

        private void Start() {
            mixer.Select(0);
            mixer.SetLoop(true);
            mixer.Play(mainTheme);
            Open();
        }

        private void FixedUpdate() {
            if (finished || startButton.activeInHierarchy) {
                return;
            }

            if (!Input.GetButtonUp(openButton)) {
                return;
            }

            if (menu.activeInHierarchy) {
                Close();
            } else {
                Open();
            }
        }
        #endregion

        #region Control
        private void Open() {
            menu.SetActive(true);
            StopPlayer();
        }

        private void Close() {
            menu.SetActive(false);
            ResumePlayer();
        }

        private void StopPlayer() {
            aim.SetActive(false);
            input.enabled = false;
            playerMovement.enabled = false;
            movementTransmitter.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void ResumePlayer() {
            aim.SetActive(true);
            input.enabled = true;
            playerMovement.enabled = true;
            movementTransmitter.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void SetUpPlayer() {
            player.position = startPoint.position;
            player.rotation = startPoint.rotation;

            ResumePlayer();
            cameraControl.FadeIn();
        }
        #endregion

        #region Actions
        public void StartGame() {
            startButton.SetActive(false);
            menu.SetActive(false);
            cameraControl.FadeOut();
            Invoke("SetUpPlayer", cameraControl.fadeTime * 2f);

            mixer.Select(1);
            mixer.Set(ambient);
            mixer.Select(0);
            mixer.SetLoop(false);
            mixer.SetTransitionTime(5f);
            mixer.SetPivot(0.95f);
            mixer.MixWith(1);
        }

        public void EndGame() {
            finished = true;
            movementTransmitter.enabled = false;
            input.enabled = false;
            StartCoroutine(StartEndGame());
        }

        IEnumerator StartEndGame() {
            rotateAsteroidField.SetLocalRotation(0);
            moveAsteroidField.SetLocalPosition(0);
            yield return new WaitForSeconds(5f);
            cameraControl.FadeOut();
            Open();
            controls.SetActive(false);
            credits.SetActive(true);
        }

        
        public void Exit() {
            Application.Quit();
        }
        #endregion
    }
}