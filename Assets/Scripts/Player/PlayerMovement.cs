using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Scripts.InputSystem;
using UnityEngine;

namespace Scripts.Player {
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _maxMovementSpeed;

        [Header("Gravity Settings")]
        [SerializeField] private float _gravityAcceleration;

        [Header("Jump Settings")]
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _verticalWallJumpForce;
        [SerializeField] private float _horizontalWallJumpForce;
        [SerializeField] private float _kayoteTime;

        [Header("Wall Run Settings")]
        [SerializeField] private float _wallRunForce;

        [Header("References")]
        [SerializeField] private Transform _orientation;


        private float _dynamicGravityForce;
        private float _kayoteCounter;

        private Rigidbody _rigidbody;
        private GroundCheck _groundCheck;
        private WallCheck _wallCheck;

        private bool _canJump = false;
        private bool _isReset = false;

        private Vector3 _movementDirection;

        public bool CanJump { get => _canJump; }
        public bool StopGravity {get; private set; }

        private void Awake() {
            _rigidbody = GetComponent<Rigidbody>();
            _groundCheck = GetComponentInChildren<GroundCheck>();
            _wallCheck = GetComponentInChildren<WallCheck>();

            _dynamicGravityForce = 0;         
        }

        private void Start() {
            InputManager.Instance.OnJump += Jump;

            Task.Run ( () => CustomGravity());

        }

        private async void CustomGravity()
        {
            while (true) {
                HandleGravity();
                await Task.Delay((int)(Time.unscaledDeltaTime * 1000));
                
            }
        }

        private void Jump()
        {
            if (_canJump) {
                _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);

            } else if (_wallCheck.ValidateCheck) {
                
                Vector3 wallNormal = _wallCheck.GetNormal();

                Vector3 wallJumpDirection = wallNormal * _horizontalWallJumpForce + transform.up * _verticalWallJumpForce;

                _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
                _wallCheck.DisableCheckForTime(0.5f);
                _rigidbody.AddForce(wallJumpDirection, ForceMode.Impulse);
                
            }
        }

        private void Update() {

            if (_rigidbody.velocity.magnitude > _maxMovementSpeed) {
                _rigidbody.velocity = _rigidbody.velocity.normalized * _maxMovementSpeed;
            }

            if (!_groundCheck.ValidateCheck) {
                if (_kayoteCounter > 0) {
                    _kayoteCounter -= Time.deltaTime;
                } else {
                    _canJump = false;
                }
            } else {
                _canJump = true;
                _kayoteCounter = _kayoteTime;
            }
        }
        private void FixedUpdate() {
            HandleMovement();
            //Handle Gravity.

            HandleGravity();

            if (!_canJump && !StopGravity) {
                _dynamicGravityForce += _gravityAcceleration;
                
                _isReset = false;
            } else {
                _dynamicGravityForce = 0f;
                if (_wallCheck && !_isReset) {
                 
                    _isReset = true;
                 
                    _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
                }
            }
        }

        private void HandleGravity()
        {
            _rigidbody.AddForce(_dynamicGravityForce * Vector3.down, ForceMode.Force);
        }

        private void HandleMovement() {
            if (!_wallCheck.ValidateCheck) {
                StopGravity = false;

                _movementDirection = _orientation.forward * InputManager.Instance.MoveInput.y + _orientation.right * InputManager.Instance.MoveInput.x;
                
                if (_groundCheck.ValidateCheck) {
                    _rigidbody.AddForce(_movementSpeed * _movementDirection - GetHorizontalVelocity(), ForceMode.VelocityChange);
                } else {
                    _rigidbody.AddForce( _movementSpeed *  _movementDirection, ForceMode.Force);
                }
                

            } else {
                StopGravity = true;

                _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);

                Vector3 wallForward = Vector3.Cross(_wallCheck.GetNormal(), Vector3.Cross(transform.forward, _wallCheck.GetNormal()));

                _rigidbody.AddForce(_wallRunForce *  wallForward, ForceMode.Force);
            }
        }

        private Vector3 GetHorizontalVelocity() {
            return new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
        }

    }
}