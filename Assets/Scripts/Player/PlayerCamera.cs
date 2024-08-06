using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.InputSystem;
using UnityEngine;

namespace Scripts.Player
{
    public class PlayerCamera : MonoBehaviour
    {
        [Header("Camera Settings")]
        [SerializeField] private float _sensitivityX = 1f;
        [SerializeField] private float _sensitivityY = 1f;

        [Header("References")]
        [SerializeField] private Transform _orientation;
        [SerializeField] private PlayerFreeze _playerFreeze;

        private float _xRotation = 0f;
        private float _yRotation = 0f;

        public float SensitivityX { get => _sensitivityX; set => _sensitivityX = value; }
        public float SensitivityY { get => _sensitivityY; set => _sensitivityY = value; }

        private void Start() {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
        }

        private void Update() {
            HandleRotation();

        }

        private void HandleRotation() {
            float mouseX = InputManager.Instance.LookInput.x * _sensitivityX * Time.unscaledDeltaTime;
            float mouseY = InputManager.Instance.LookInput.y * _sensitivityY * Time.unscaledDeltaTime;

            _yRotation += mouseX;

            _xRotation -= mouseY;
            
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

            transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0f);;

            _orientation.rotation = Quaternion.Euler(0f, _yRotation, 0f);
            
        }
    }
}
