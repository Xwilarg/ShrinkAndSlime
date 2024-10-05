using LudumDare56.Player;
using UnityEngine;
using UnityEngine.AI;

namespace LudumDare56.Slime
{
    public class SlimeBehavior : MonoBehaviour
    {
        private bool is_following = true; // following Player by default
        [SerializeField] private Transform player_transform;

        private NavMeshAgent agent;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            if (player_transform == null)
            {
                player_transform = Object.FindFirstObjectByType<PlayerController>().transform;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (is_following)
            {
                agent.destination = player_transform.transform.position;
            }
        }
    }
}