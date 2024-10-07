using LudumDare56.Map;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace LudumDare56.Enemy
{
    public abstract class AEnemyController : MonoBehaviour, IScalable
    {
        [SerializeField]
        protected Transform _modelContainer, _model;

        /// <summary>
        /// Next position the AI should go to
        /// </summary>
        protected Node _targetNode;

        /// <summary>
        /// Overriden target of who we should fight
        /// </summary>
        protected GameObject _fightingTarget;

        /// <summary>
        /// Animator base, conditions called in specific enemy classes
        /// </summary>
        [SerializeField]
        protected Animator _animator;

        public GameObject GameObject => _modelContainer.gameObject;

        public float BaseScale { private set; get; }

        public NavMeshAgent Agent { private set; get; }

        public float ScaleProgression { set; get; }

        protected virtual void Awake()
        {
            BaseScale = _model.localScale.x;
            Agent = GetComponent<NavMeshAgent>();

            var d = GetComponentInChildren<Detector>();
            d.OnTriggerEnterEvt.AddListener((c) =>
            {
                if (c.CompareTag("Player")) _fightingTarget = c.gameObject;
            });
            d.OnTriggerExitEvt.AddListener((c) =>
            {
                if (_fightingTarget != null && c.gameObject.GetInstanceID() == _fightingTarget.GetInstanceID()) _fightingTarget = null;
            });
        }

        protected virtual void Start()
        {
            _targetNode = GameObject.FindObjectsOfType<Node>().OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).First();
        }
    }
}