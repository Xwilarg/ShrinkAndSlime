using LudumDare56.Enemy;
using UnityEngine;

namespace LudumDare56.Map
{
    public class Sheep : MonoBehaviour, IScalable
    {
        public GameObject GameObject => gameObject;

        public float BaseScale { private set; get; }

        public float ScaleProgression { get; set; }

        private void Awake()
        {
            BaseScale = transform.localScale.x;
        }
    }
}
