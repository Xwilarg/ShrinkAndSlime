using LudumDare56.Enemy;
using LudumDare56.Manager;
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
        [SerializeField] private float _maxDistFromPlayer = 25; 
        [SerializeField] private float _growMultiplier = 1.5f;
        [SerializeField] private float _lerpDuration = 2f; // how long it takes to grow into the new size
        [SerializeField] private Animator _handAnimator; // we are setting off triggers based on what we command the slime!
        [SerializeField] private GameObject _outroComic;
        private Animator _anim;

        private NavMeshAgent agent;
        private Vector3 _targetDestination;
        private Camera _cam;
        [SerializeField]
        private SkinnedMeshRenderer _renderer;

        private GameObject _monsterToEat;

        private float _timeElapsed;
        private bool _isGrowing = false;

        private Vector3 _targetScale;
        private Vector3 _startScale;

        private int _clicked = 0;
        private float _clickTime;
        private float _clickDelay = 0.3f;

        private bool _isCheckingClicks = false;
        private float _dist_multiplier = 0f;
        private float _max_dist_from_enemy = 3f;

        private Vector3 playerPosition => PlayerController.Instance.transform.position;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            _anim = GetComponentInChildren<Animator>();
            _cam = Camera.main;

            if(_handAnimator)
            {
                _handAnimator.speed = 2.5f; // speed of only the hand should be a smidge sped up!
            }
        }

        // Update is called once per frame
        void Update()
        {
            _anim.SetBool("IsMoving", agent.velocity.magnitude > 0f);

            if (_isfollowing)
            {
             
                //Teleporting stuff
                if(Vector3.Distance(transform.position, playerPosition) > _maxDistFromPlayer)
                {
                    Vector3 newPos = new Vector3(playerPosition.x, playerPosition.y+1, playerPosition.z - 2f);
                    transform.position = newPos;
                    agent.enabled = false; // need to temporarily turn this off or the slime for some reason gets stuck in furniture
                }
                else
                {
                    if(!agent.enabled)
                    {
                        agent.enabled = true;
                    }
                   
                    agent.SetDestination(playerPosition);
                }
            }
            else
            {
                //Checking if we're at the destination the player put the slime at
                //if we have a target AND we are close to the target
                if (Vector3.Distance(transform.position, _targetDestination) < 3.5)
                {
                    if (_monsterToEat)
                    {
                        CheckForEdibleObjects(_monsterToEat);
                        _monsterToEat = null;
                    }

                }
            }

            if(_isGrowing)
            {
                Grow();
            }
        }


        public void DirectSlime(InputAction.CallbackContext context)
        {
            if (context.canceled && GameManager.Instance.CanPlay) // if we RELEASED the mouse button
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
            agent.stoppingDistance = 5f;

            if (_handAnimator)
            {
                _handAnimator.SetTrigger("ComeHere");
            }
          
        }

        private void GoToLocation()
        {
            _isfollowing = false;
            agent.SetDestination(_targetDestination);
            agent.stoppingDistance = 0f;

            if (_handAnimator)
            {
                _handAnimator.SetTrigger("GoThere");
            }
        }

        public void CheckForEdibleObjects(GameObject obj)
        {
            var isc = GetScalable(obj);
            var meshRenderer = isc.GameObject.GetComponentInChildren<MeshRenderer>();
            var skinnedMeshRenderer = isc.GameObject.GetComponentInChildren<SkinnedMeshRenderer>();
            

            if (meshRenderer != null)
            {
                if (IsSlimeBigger(meshRenderer.bounds.size))
                {
                    if (Vector3.Distance(transform.position, obj.transform.position) < _max_dist_from_enemy) // if we're close, go for it!
                    {
                        EatObject(isc);
                    }
                    else// if not, follow the object!
                    {
                        if(agent.enabled) // can't follow objects not on the nav mesh
                        {
                            agent.SetDestination(obj.transform.position);
                        }
                        
                    }
                }
            }
            else if ( skinnedMeshRenderer != null) //todo: probably clean this up haha
            {
                if (IsSlimeBigger(skinnedMeshRenderer.bounds.size))
                {
                    if (Vector3.Distance(transform.position, obj.transform.position) < _max_dist_from_enemy) // if we're close, go for it!
                    {
                        EatObject(isc);
                    }
                    else// if not, follow the object!
                    {
                        if (agent.enabled) // can't follow objects not on the nav mesh
                        {
                            agent.SetDestination(obj.transform.position);
                        }
                    }
                }
            }
            else
            {
                _isfollowing = true;
            }
        }

        private IScalable GetScalable(GameObject obj) // helper func because the meshrenderer could be on a sibling object
        {
            if(obj.tag == "Bullet")
            {
                return null; // we should NOT be trying to eat the bullets omg
            }

            var scalableObj = obj.GetComponent<IScalable>();

            if (scalableObj != null)
            {
                return scalableObj;
            }

            var siblingScalableObj = obj.transform.parent.GetComponentInChildren<IScalable>(); 

            if(siblingScalableObj != null)
            {
                return siblingScalableObj;
            }

            return null;
        }

        private void EatObject(IScalable obj)
        {

            if (obj.GameObject.CompareTag("Boss"))
            {
                GameManager.Instance.CanPlay = false;
                _outroComic.SetActive(true);
            }


            if(obj.GameObject.transform.parent != null && obj.GameObject.name.ToLower().Equals("model")) // we want to destroy the object holding the model! 
            {
                Destroy(obj.GameObject.transform.parent.gameObject); // The enemy models are inside a parent, so we'll destroy the parent
            }
            else
            {
                Destroy(obj.GameObject); // if it's not being contained, just destory the object
            }
           

            _isfollowing = true; // follow the player again after we eat something!

            _targetScale = transform.localScale * _growMultiplier;
            _startScale = transform.localScale;
            _dist_multiplier += 1.5f;
            _max_dist_from_enemy += 0.75f;
            _maxDistFromPlayer += 0.25f;
            agent.stoppingDistance += (3+ _dist_multiplier); //increase distance from the player because the bigger the slime the closer they are haha
            _isGrowing = true;
        }

        private void Grow()
        {
            if (_timeElapsed < _lerpDuration)
            {
                transform.localScale = Vector3.Lerp(_startScale, _targetScale, _timeElapsed/_lerpDuration); // keep growing!
                _timeElapsed += Time.deltaTime;
            }
            else
            {
                transform.localScale = _targetScale;
                _timeElapsed = 0;
                _isGrowing = false;
            }

        


        }

        private bool IsSlimeBigger(Vector3 objectSize)
        {
            var _slimeSize = _renderer.bounds.size;
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