using UnityEngine;

namespace LudumDare56.Enemy
{
    public abstract class AEnemyController : MonoBehaviour, IScalable
    {
        public GameObject GameObject => gameObject;

        public float BaseScale { private set; get; }

        public float ScaleProgression { set; get; }

        protected virtual void Awake()
        {
            BaseScale = transform.localScale.x;
        }
    }
}