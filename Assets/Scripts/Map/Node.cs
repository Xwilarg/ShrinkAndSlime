using UnityEngine;

namespace LudumDare56.Map
{
    public class Node : MonoBehaviour
    {
        [SerializeField]
        private Node _nextNode;

        public Node NextNode => _nextNode;

        private void OnDrawGizmos()
        {
            if (NextNode != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, NextNode.transform.position);
            }
        }
    }
}
