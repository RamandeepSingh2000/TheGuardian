using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class MovementController : MonoBehaviour, IInjectableInputActions
    {
        [SerializeField] private Transform _camera;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Transform _groundCheck;

        private Vector2 _movementDirection;
        private Vector3 _velocity;
        private float _xCameraRotation = 0;
        private float _groundDistance = 0.4f;
        [SerializeField] private LayerMask _groundMask;

        private bool _isGrounded;
        private bool _isSprinting;

        public float WalkingSpeed = 1;
        public float SprintingSpeed = 1;

        public Vector2 CameraRotationSpeed = new Vector2(10, 0.5f);

        public float JumpForce = 1;
        public float Gravity = 1;

        public bool EnableMovement = true;
        public bool EnableCameraRotation = true;

        public Action OnJump;

        private PlayerInputActions _inputActions { get; set; }

        //Delegates
        private Action<InputAction.CallbackContext> _movePerformedAction;
        private Action<InputAction.CallbackContext> _moveCanceledAction;
        private Action<InputAction.CallbackContext> _lookPerformedAction;
        private Action<InputAction.CallbackContext> _lookCanceledAction;
        private Action<InputAction.CallbackContext> _sprintPerformedAction;
        private Action<InputAction.CallbackContext> _sprintCanceledAction;
        private Action<InputAction.CallbackContext> _jumpPerformedAction;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void InjectInputActions(PlayerInputActions inputActions)
        {
            _inputActions = inputActions;
            EnableInputs();
        }

        public void EnableInputs()
        {
            _movePerformedAction = (obj) => SetMovementDirection(obj.ReadValue<Vector2>());
            _moveCanceledAction = (_) => SetMovementDirection(Vector2.zero);
            _lookPerformedAction = (obj) => RotateCamera(obj.ReadValue<Vector2>());
            _lookCanceledAction = (_) => RotateCamera(Vector2.zero);
            _sprintPerformedAction = (_) => Sprint();
            _sprintCanceledAction = (_) => StopSprinting();
            _jumpPerformedAction = (_) => Jump();

            _inputActions.Movement.Enable();
            _inputActions.Movement.Move.performed += _movePerformedAction;
            _inputActions.Movement.Move.canceled += _moveCanceledAction;
            _inputActions.Movement.Look.performed += _lookPerformedAction;
            _inputActions.Movement.Look.canceled += _lookCanceledAction;
            _inputActions.Movement.Sprint.performed += _sprintPerformedAction;
            _inputActions.Movement.Sprint.canceled += _sprintCanceledAction;
            _inputActions.Movement.Jump.performed += _jumpPerformedAction;

            Cursor.lockState = CursorLockMode.Locked;
        }

        public void DisableInputs()
        {
            _inputActions.Movement.Disable();
            _inputActions.Movement.Move.performed -= _movePerformedAction;
            _inputActions.Movement.Move.canceled -= _moveCanceledAction;
            _inputActions.Movement.Look.performed -= _lookPerformedAction;
            _inputActions.Movement.Look.canceled -= _lookCanceledAction;
            _inputActions.Movement.Sprint.performed -= _sprintPerformedAction;
            _inputActions.Movement.Sprint.canceled -= _sprintCanceledAction;
            _inputActions.Movement.Jump.performed -= _jumpPerformedAction;
        }

        private void FixedUpdate()
        {
            Move(_movementDirection);
        }

        public void SetMovementDirection(Vector2 direction)
        {
            _movementDirection = direction;
        }

        public void Move(Vector2 direction)
        {
            if (!EnableMovement)
                return;

            _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);

            //Moves the character controller according to the input received
            float movementSpeed = _isSprinting ? SprintingSpeed : WalkingSpeed;

            Vector3 movement = _movementDirection.y * _characterController.transform.forward + _movementDirection.x * _characterController.transform.right;
            _characterController.Move(movement * movementSpeed * Time.fixedDeltaTime);

            if (_isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }

            _velocity.y += Gravity * Time.fixedDeltaTime;

            _characterController.Move(_velocity * movementSpeed * Time.fixedDeltaTime);
        }

        public void Sprint()
        {
            _isSprinting = true;
        }

        public void StopSprinting()
        {
            _isSprinting = false;
        }

        public void RotateCamera(Vector2 direction)
        {
            _xCameraRotation -= direction.y * CameraRotationSpeed.y;
            _xCameraRotation = Mathf.Clamp(_xCameraRotation, -90f, 90);
            _camera.localRotation = Quaternion.Euler(_xCameraRotation, 0, 0);
            _characterController.transform.Rotate(Vector3.up * CameraRotationSpeed.x * direction.x * Time.deltaTime, Space.Self);
        }

        public void Jump()
        {
            if (_isGrounded)
            {               
                _velocity.y = JumpForce;
                OnJump?.Invoke();
            }                
        }

        public void SetCameraLookDirection(Vector3 direction)
        {

        }

    }
}