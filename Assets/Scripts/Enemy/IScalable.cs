using UnityEngine;
using UnityEngine.AI;

namespace LudumDare56.Enemy
{
    public interface IScalable
    {
        public GameObject GameObject { get; }
        public float BaseScale { get; }
        public float ScaleProgression { set; get; }

        public abstract NavMeshAgent Agent { get; } 
    }
}
