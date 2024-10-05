using UnityEngine;

namespace LudumDare56.Slime
{
    public class SlimeEatingBehavior : MonoBehaviour //TODO: See if we still need this...Collisions aren't working?
    {
        private SlimeBehavior _slime;

        private void Awake()
        {
            _slime = GetComponentInParent<SlimeBehavior>();    
        }
        private void OnTriggerStay(Collider other)
        {
            Debug.Log("yo???");
            _slime.CheckForEdibleObjects(other.gameObject);
        }

    }
}