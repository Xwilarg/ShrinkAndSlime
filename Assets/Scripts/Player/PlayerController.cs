using LudumDare56.Enemy;
using LudumDare56.SO;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LudumDare56.Player
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance { private set; get; }

        [SerializeField]
        private PlayerInfo _info;

        [SerializeField]
        private GameObject _model;

        [SerializeField]
        private CinemachineOrbitalFollow _camHead;

        private CharacterController _controller;
        private Vector2 _mov;
        private bool _isSprinting;
        private float _verticalSpeed;

        private Camera _cam;

        private int _attackLayer;

        private bool _isShooting;

        private bool CanMove => true;

        private void Awake()
        {
            Instance = this;
            _controller = GetComponent<CharacterController>();

            Cursor.lockState = CursorLockMode.Locked;

            _attackLayer = LayerMask.GetMask("Map", "Monster");

            _cam = Camera.main;
        }

        private void FixedUpdate()
        {
            if (!CanMove)
            {
                return;
            }

            if (_isShooting && Physics.Raycast(_camHead.transform.position, _camHead.transform.forward, out var hit, 1000f, _attackLayer) && hit.collider.TryGetComponent<IScalable>(out var sc))
            {
                sc.ScaleProgression = Mathf.Clamp01(sc.ScaleProgression + Time.deltaTime);
                var size = Mathf.Lerp(sc.BaseScale, sc.BaseScale * .1f, sc.ScaleProgression);
                hit.collider.transform.localScale = Vector3.one * size;
            }

            _camHead.transform.eulerAngles = new(_camHead.VerticalAxis.Value, _camHead.HorizontalAxis.Value, 0f);

            var pos = _mov;
            Vector3 desiredMove = _cam.transform.forward * pos.y + _cam.transform.right * pos.x;
            _model.transform.LookAt(transform.position + _cam.transform.forward, Vector3.up);
            _model.transform.rotation = Quaternion.Euler(0f, _model.transform.rotation.eulerAngles.y, 0f);

            // Get a normal for the surface that is being touched to move along it
            Physics.SphereCast(transform.position, _controller.radius, Vector3.down, out RaycastHit hitInfo,
                               _controller.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            Vector3 moveDir = Vector3.zero;
            moveDir.x = desiredMove.x * _info.ForceMultiplier * (_isSprinting ? _info.SpeedRunningMultiplicator : 1f);
            moveDir.z = desiredMove.z * _info.ForceMultiplier * (_isSprinting ? _info.SpeedRunningMultiplicator : 1f);

            if (_controller.isGrounded && _verticalSpeed < 0f) // We are on the ground and not jumping
            {
                moveDir.y = -.1f; // Stick to the ground
                _verticalSpeed = -_info.GravityMultiplicator;
            }
            else
            {
                // We are currently jumping, reduce our jump velocity by gravity and apply it
                _verticalSpeed += Physics.gravity.y * _info.GravityMultiplicator * Time.fixedDeltaTime;
                moveDir.y += _verticalSpeed;
            }

            var p = transform.position;
            _controller.Move(moveDir);
        }

        public void OnMovement(InputAction.CallbackContext value)
        {
            _mov = value.ReadValue<Vector2>().normalized;
        }

        public void OnJump(InputAction.CallbackContext value)
        {
            if (_controller.isGrounded && value.phase == InputActionPhase.Started)
            {
                _verticalSpeed = _info.JumpForce;
            }
        }

        public void OnSprint(InputAction.CallbackContext value)
        {
            _isSprinting = value.ReadValueAsButton();
        }

        public void OnAttack(InputAction.CallbackContext value)
        {
            if (value.phase == InputActionPhase.Started)
            {
                _isShooting = true;
            }
            else if (value.phase == InputActionPhase.Canceled)
            {
                _isShooting = false;
            }
        }
    }
}
