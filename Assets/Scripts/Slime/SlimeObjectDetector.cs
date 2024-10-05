using UnityEngine;

namespace LudumDare56.Slime
{
    public class SlimeObjectDetector : MonoBehaviour // If a monster comes into the radius, try to eat it!
    {
        private SlimeBehavior _slime;

        private void Awake()
        {
            _slime = GetComponentInParent<SlimeBehavior>();    
        }
        private void OnTriggerStay(Collider other)
        {
            _slime.CheckForEdibleObjects(other.gameObject);
        }

    }
}