using LudumDare56.Player;
using UnityEngine;
using UnityEngine.AI;

namespace LudumDare56.Slime
{
    public class SlimeBehavior : MonoBehaviour
    {
        private bool _isfollowing = true; // following Player by default
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private float _maxDistFromPlayer = 15;


        private NavMeshAgent agent;
        private Transform _targetDestination;
        private Camera _cam;
        private MeshRenderer _renderer;
        private Vector3 _slimeSize;

        private GameObject _monsterToEat;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            _cam = Camera.main;
            _renderer = GetComponentInChildren<MeshRenderer>(); // Model is a child of the Slime object
            _slimeSize = _renderer.bounds.size;

            if (_playerTransform == null)
            {
                _playerTransform = Object.FindFirstObjectByType<PlayerController>().transform;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (_isfollowing)
            {
                agent.destination = _playerTransform.transform.position;
            }
            else {

                //Checking if we're at the destination the player put the slime at

                //if we have a target AND we are close to the target
                if (_targetDestination && Vector3.Distance(transform.position, _targetDestination.position) < 3.5)
                {
                    //TODO: Maybe check for nearby creatures to eat?
                    if (_monsterToEat)
                    {
                        CheckForEdibleObjects(_monsterToEat);
                    }
                    //THEN, return to the player
                    _monsterToEat = null;
                    _isfollowing = true;
                }
                else //else try to get back to the player if we're too far!
                {
                    var dist = Vector3.Distance(transform.position, _playerTransform.position);
                    if (dist > _maxDistFromPlayer)
                    {
                        _isfollowing = true;
                    }
                }
               
            
             
            
            }
        }

        public void DirectSlime()
        {
            _isfollowing= false;

            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Monster")
                {
                    _monsterToEat = hit.collider.gameObject;
                }
                _targetDestination = hit.transform;
                agent.SetDestination(hit.point);



            }
        }

        public void CheckForEdibleObjects(GameObject obj)
        {
            if (obj.GetComponent<MeshRenderer>())
            {
                var _otherRenderer = obj.GetComponentInChildren<MeshRenderer>();
                Debug.Log(_otherRenderer.bounds.size);

                if (IsSlimeBigger(_otherRenderer.bounds.size))
                {
                    EatObject(obj);
                }
            }
        }

        private void EatObject(GameObject obj)
        {
            if (obj.GetComponent<MeshRenderer>())
            {
                var _otherRenderer = obj.GetComponentInChildren<MeshRenderer>();
                if (IsSlimeBigger(_otherRenderer.bounds.size))
                {
                    Destroy(obj.gameObject);
                    //TODO: Should probably have the enemies increase by a certain size OR have it increase by a constant
                    transform.localScale *= 1.25f;

                }
            }
        }

        private bool IsSlimeBigger(Vector3 objectSize)
        { 
            //storing slime vars
            var slime_width = _slimeSize.x;
            var slime_height = _slimeSize.y;
            var slime_length = _slimeSize.z;

            //storing object vars

            var object_width = objectSize.x;
            var object_height = objectSize.y;
            var object_length = objectSize.z;

            if (slime_width > object_width && slime_height > object_height && slime_length > object_length)
            {
                return true;
            }


            return false;

        }
    }
}