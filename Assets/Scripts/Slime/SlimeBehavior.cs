using LudumDare56.Enemy;
using LudumDare56.Player;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace LudumDare56.Slime
{
    public class SlimeBehavior : MonoBehaviour
    {
        private bool _isfollowing = true; // following Player by default
      //  [SerializeField] private float _maxDistFromPlayer = 15; //TODO: Discuss if we still want this
        [SerializeField] private float _growMultiplier = 1.5f;
        [SerializeField] private float _lerpDuration = 2f; // how long it takes to grow into the new size
        [SerializeField] private Animator _handAnimator; // we are setting off triggers based on what we command the slime!

        private NavMeshAgent agent;
        private Vector3 _targetDestination;
        private Camera _cam;
        [SerializeField]
        private SkinnedMeshRenderer _renderer;
        private Vector3 _slimeSize;

        private GameObject _monsterToEat;

        private float _timeElapsed;
        private bool _isGrowing = false;

        private Vector3 _targetScale;
        private Vector3 _startScale;

        private int _clicked = 0;
        private float _clickTime;
        private float _clickDelay = 0.3f;

        private bool _isCheckingClicks = false;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            _cam = Camera.main;
            _slimeSize = _renderer.bounds.size;

            if(_handAnimator)
            {
                _handAnimator.speed = 2.5f; // speed of only the hand should be a smidge sped up!
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (_isfollowing)
            {
                agent.SetDestination(PlayerController.Instance.transform.position);
            }
            else
            {
                //Checking if we're at the destination the player put the slime at
                //if we have a target AND we are close to the target
                if (Vector3.Distance(transform.position, _targetDestination) < 3.5)
                {
                    //TODO: Maybe check for nearby creatures to eat?
                    if (_monsterToEat)
                    {
                        CheckForEdibleObjects(_monsterToEat);
                        _monsterToEat = null;
                    }

                    //Stay there if there's no monster to eat
                }
                else //else try to get back to the player if we're too far!
                {
                    //Commented out because I want to discuss if we think this is necessary to keep! -Geneva
                    /*
                    var dist = Vector3.Distance(transform.position, _playerTransform.position);
                    if (dist > _maxDistFromPlayer)
                    {
                        _isfollowing = true;
                    }*/
                }
            }

            if(_isGrowing)
            {
                Grow();
            }
        }


        public void DirectSlime(InputAction.CallbackContext context)
        {
            if (context.canceled) // if we RELEASED the mouse button
            {
                // remembering WHERE we clicked
                Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.tag == "Monster")
                    {
                        _monsterToEat = hit.collider.gameObject;
                    }
                    _targetDestination = hit.point;
                }

                //checking for double click
                _clicked++;
                if (_clicked == 1)
                {
                    if (!_isCheckingClicks)
                    {
                        StartCoroutine(CheckForDoubleClick());
                    }

                }
            }
        }

        private IEnumerator CheckForDoubleClick()
        {
            _isCheckingClicks = true;
            _clickTime = 0f;
            while (_clicked != 0)
            {
                yield return new WaitForEndOfFrame();
                _clickTime += Time.deltaTime;

                if(_clickTime >= _clickDelay)
                {
                    if(_clicked ==1)
                    {
                        GoToLocation();
                    }
                    else // the double-click!
                    {
                        ComeBack();
                    }
                    _clicked = 0; 
                    _isCheckingClicks= false;
                }

            }
        }

        private void ComeBack()
        {
            _clicked = 0;
            _clickTime = 0;
            _isfollowing = true;

            if(_handAnimator)
            {
                _handAnimator.SetTrigger("ComeHere");
            }
          
        }

        private void GoToLocation()
        {
            _isfollowing = false;
            agent.SetDestination(_targetDestination);

            if (_handAnimator)
            {
                _handAnimator.SetTrigger("GoThere");
            }
        }

        public void CheckForEdibleObjects(GameObject obj)
        {
            var isc = GetMeshRenderer(obj);
            var meshRenderer = isc.GameObject.GetComponentInChildren<MeshRenderer>();
            if (meshRenderer)
            {
                if (IsSlimeBigger(meshRenderer.bounds.size))
                {
                    if (Vector3.Distance(transform.position, obj.transform.position) < 3.5) // if we're close, go for it!
                    {
                        EatObject(isc);
                    }
                    else// if not, follow the object!
                    {
                        agent.SetDestination(obj.transform.position);
                    }
                }
            }
        }

        private IScalable GetMeshRenderer(GameObject obj) // helper func because the meshrenderer could be on a sibling object
        {
            var meshRenderer = obj.GetComponent<IScalable>();

            if (meshRenderer != null)
            {
                return meshRenderer;
            }

            var siblingMeshRenderer = obj.transform.parent.GetComponentInChildren<IScalable>(); 

            if(siblingMeshRenderer != null)
            {
                return siblingMeshRenderer;
            }

            return null;
        }

        private void EatObject(IScalable obj)
        {
            PlayerController.Instance.GainEnergy(15f);

            Destroy(obj.GameObject); // The enemy models are usually inside a parent, so we'll destroy the parent

            _isfollowing = true; // follow the player again after we eat something!

            _targetScale = transform.localScale * _growMultiplier;
            _startScale = transform.localScale;
            _isGrowing = true;
        }

        private void Grow()
        {

            if(_timeElapsed < _lerpDuration)
            {
                transform.localScale = Vector3.Lerp(_startScale, _targetScale, _timeElapsed/_lerpDuration); // keep growing!
                _timeElapsed += Time.deltaTime;
            }
            else
            {
                transform.localScale = _targetScale;
                _isGrowing = false;
            }


        }

        private bool IsSlimeBigger(Vector3 objectSize)
        { 
            //storing slime vars
            var slime_width = _slimeSize.x;
            var slime_height = _slimeSize.y;
            var slime_depth = _slimeSize.z;

            //storing object vars

            var object_width = objectSize.x;
            var object_height = objectSize.y;
            var object_depth = objectSize.z;

            if (slime_width > object_width && slime_height > object_height || slime_height > object_height && slime_depth > object_depth)
            {
                return true;
            }


            return false;

        }
    }
}