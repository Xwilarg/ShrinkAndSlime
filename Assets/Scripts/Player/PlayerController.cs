using LudumDare56.Enemy;
using LudumDare56.Manager;
using LudumDare56.SO;
using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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

        [SerializeField]
        private TMP_Text _energyText;

        [SerializeField]
        private Image _reticle;

        [SerializeField]
        private Transform _gunEnd;

        [SerializeField]
        private Transform _healthUI;

        [SerializeField]
        private GameObject _gameOver;

        private CharacterController _controller;
        private Vector2 _mov;
        private bool _isSprinting;
        private float _verticalSpeed;

        private Camera _cam;

        private int _attackLayer;

        private bool _isShooting;

        private float _energyAmount = 75f;

        private LineRenderer _lr;

        private Vector3? _rayTarget;

        private bool _canTakeDamage = true;

        private void Awake()
        {
            Instance = this;
            _controller = GetComponent<CharacterController>();
            _lr = GetComponentInChildren<LineRenderer>();

            Cursor.lockState = CursorLockMode.Locked;

            _attackLayer = LayerMask.GetMask("Map", "Monster", "Sheep");

            _cam = Camera.main;

            _reticle.color = Color.black;

            UpdateUI();
        }

        private void Update()
        {
            if (_rayTarget != null)
            {
                _lr.SetPositions(new[] { _gunEnd.transform.position, _rayTarget.Value });
            }
        }

        private void FixedUpdate()
        {
            if (_isShooting && GameManager.Instance.CanPlay)
            {
                if (_energyAmount > 0f)
                {
                    _energyAmount -= Time.deltaTime * 10f;
                    if (_energyAmount < 0f) _energyAmount = 0f;
                    UpdateUI();

                    _lr.enabled = true;
                    if (Physics.Raycast(_camHead.transform.position, _camHead.transform.forward, out var hit, 1000f, _attackLayer))
                    {
                        if (hit.collider.transform.TryGetComponent<IScalable>(out var sc))
                        {
                            sc.ScaleProgression = Mathf.Clamp01(sc.ScaleProgression + Time.deltaTime);
                            var size = Mathf.Lerp(sc.BaseScale, sc.BaseScale * .1f, sc.ScaleProgression);
                            sc.GameObject.transform.localScale = Vector3.one * size;
                            if(sc.Agent != null) // if we have a navmeshagent on this enemy we need to shrink the radius too! - Gen
                            {
                                var radiusSize = Mathf.Lerp(sc.Agent.radius, sc.Agent.radius * .1f, sc.ScaleProgression);
                                sc.Agent.radius = radiusSize;
                            }
                            _rayTarget = hit.point;
                        }
                        else
                        {
                            _rayTarget = _camHead.transform.position + (_camHead.transform.forward * 1000f);
                        }

                    }
                    else
                    {
                        _rayTarget = _camHead.transform.position + (_camHead.transform.forward * 1000f);
                    }

                    _reticle.color = Color.blue;
                }
                else
                {
                    _reticle.color = Color.red;
                    _lr.enabled = false;
                    _rayTarget = null;
                }
            }
            else
            {
                _reticle.color = Color.black;
                _lr.enabled = false;
                _rayTarget = null;
            }

            _camHead.transform.eulerAngles = new(_camHead.VerticalAxis.Value, _camHead.HorizontalAxis.Value, 0f);

            var pos = GameManager.Instance.CanPlay ? _mov : Vector2.zero;
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

        public void GainEnergy(float amount)
        {
            _energyAmount += amount;
            if (_energyAmount > 100f) _energyAmount = 100f;

            UpdateUI();
        }

        public void TakeDamage()
        {
            if (_healthUI.childCount > 0)
            {
                Destroy(_healthUI.GetChild(_healthUI.childCount - 1).gameObject);
                StartCoroutine(TakeDamageInvul());
            }
            else
            {
                GameManager.Instance.CanPlay = false;
                Cursor.lockState = CursorLockMode.None;
                _gameOver.SetActive(true);
            }
        }

        private IEnumerator TakeDamageInvul()
        {
            _canTakeDamage = false;
            yield return new WaitForSeconds(1f);
            _canTakeDamage = true;
        }

        private void UpdateUI()
        {
            _energyText.text = $"{_energyAmount:0}%";
        }

        public void OnMovement(InputAction.CallbackContext value)
        {
            _mov = value.ReadValue<Vector2>().normalized;
        }

        public void OnJump(InputAction.CallbackContext value)
        {
            if (GameManager.Instance.CanPlay && _controller.isGrounded && value.phase == InputActionPhase.Started)
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
            if (value.phase == InputActionPhase.Started && GameManager.Instance.CanPlay)
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
