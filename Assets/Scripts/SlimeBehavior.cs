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

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            _cam = Camera.main;
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
                if (_targetDestination && Vector3.Distance(transform.position, _targetDestination.position) > 5)
                {
                    //TODO: Maybe check for nearby creatures to eat?

                    //THEN, return to the player
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

            if (Physics.Raycast(ray,out hit))
            {
                agent.SetDestination(hit.point);
            }
        }
    }
}