using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Player;
using UnityEngine;
using UnityEngine.InputSystem;



namespace Scripts.InputSystem {
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        [SerializeField] private Transform _playerTransform;

        private PlayerMovement _playerMovement;
        private WallCheck _wallCheck;
        [SerializeField] private InputScheduler _inputScheduler;
        private PlayerInputActions _playerInputActions;

        private Vector2 _moveInput;
        private Vector2 _lookInput;

        public Vector2 MoveInput => _moveInput;
        public Vector2 LookInput => _lookInput;

        public event Action OnTimeFreeze;
        public event Action OnJump;
        public event Action OnPause;
        public event Action OnScreenCaptureEvent;

        private void Awake()
        {
            if (Instance != this && Instance != null) {
                Destroy(gameObject);
            } else {
                Instance = this;
            }

            _playerInputActions = new PlayerInputActions();

            _wallCheck = _playerTransform.GetComponent<WallCheck>();
            _playerMovement = _playerTransform.GetComponent<PlayerMovement>();
        }

        private void OnEnable() {
            _playerInputActions.Enable();

            _playerInputActions.Player.Move.performed += OnMovePerformed;
            _playerInputActions.Player.Move.canceled += OnMovePerformed;

            _playerInputActions.Player.Look.performed += OnLookPerformed;
            _playerInputActions.Player.Look.canceled += OnLookPerformed;

            _playerInputActions.Player.TimeFreeze.performed += OnTimeFreezePerformed;

            _playerInputActions.Player.Jump.performed += OnJumpPerformed;

            _playerInputActions.UI.PauseMenu.performed += OnPausePerformed;

            _playerInputActions.Screen.ScreenCapture.performed += OnScreenCapturePerformed;
        }

        private void OnScreenCapturePerformed(InputAction.CallbackContext context)
        {
            OnScreenCaptureEvent?.Invoke();
        }

        private void OnPausePerformed(InputAction.CallbackContext context)
        {
            OnPause?.Invoke();
        }

        private void OnJumpPerformed(InputAction.CallbackContext context)
        {
            PredicatedCommand jumpCommand = new(() => {
                return _playerMovement.CanJump || _wallCheck.ValidateCheck;
                }, () => OnJump?.Invoke(), 0.3f);
        
            _inputScheduler.AddCommand(jumpCommand);
        }

        private void OnTimeFreezePerformed(InputAction.CallbackContext context)
        {
            OnTimeFreeze?.Invoke();
        }

        private void OnLookPerformed(InputAction.CallbackContext context)
        {
            if (context.performed) {
                _lookInput = context.ReadValue<Vector2>();
            }
            else if (context.canceled) {
                _lookInput = Vector2.zero;
            }
        }

        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            if (context.performed) {
                _moveInput = context.ReadValue<Vector2>();
            }
            else if (context.canceled) {
                _moveInput = Vector2.zero;
            }
        }

        public void EnablePlayerInput() {
            _playerInputActions.Player.Enable();    
        }

        public void DisablePlayerInput() {
            _playerInputActions.Player.Disable();
        }
        private void OnDisable() {
            _playerInputActions.Disable();

            _playerInputActions.Player.Move.performed -= OnMovePerformed;
            _playerInputActions.Player.Move.canceled -= OnMovePerformed;
            
            _playerInputActions.Player.Look.performed -= OnLookPerformed;
            _playerInputActions.Player.Look.canceled -= OnLookPerformed;
        
            _playerInputActions.Player.TimeFreeze.performed -= OnTimeFreezePerformed;

            _playerInputActions.Player.Jump.performed -= OnJumpPerformed;

            _playerInputActions.UI.PauseMenu.performed -= OnPausePerformed;

            _playerInputActions.Screen.ScreenCapture.performed -= OnScreenCapturePerformed;
        }
    }
}