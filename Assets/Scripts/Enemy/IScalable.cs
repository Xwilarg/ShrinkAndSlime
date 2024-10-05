using UnityEngine;

namespace LudumDare56.Enemy
{
    public interface IScalable
    {
        public GameObject GameObject { get; }
        public float BaseScale { get; }
        public float ScaleProgression { set; get; }
    }
}
