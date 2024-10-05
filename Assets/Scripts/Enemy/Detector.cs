using UnityEngine;
using UnityEngine.Events;

namespace LudumDare56.Enemy
{
    public class Detector : MonoBehaviour
    {
        public UnityEvent<Collider> OnTriggerEnterEvt { get; } = new();
        public UnityEvent<Collider> OnTriggerExitEvt { get; } = new();


        private void Awake()
        {
            tag = "Monster"; //Needs this monster tag in case the raycast hits this instead of the model! -Gen
        }
        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterEvt.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            OnTriggerExitEvt.Invoke(other);
        }
    }
}
