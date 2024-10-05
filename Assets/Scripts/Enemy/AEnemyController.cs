using UnityEngine;

namespace LudumDare56.Enemy
{
    public abstract class AEnemyController : MonoBehaviour, IScalable
    {
        [SerializeField]
        protected Transform _model;

        public GameObject GameObject => _model.gameObject;

        public float BaseScale { private set; get; }

        public float ScaleProgression { set; get; }

        protected virtual void Awake()
        {
            BaseScale = _model.localScale.x;
        }
    }
}