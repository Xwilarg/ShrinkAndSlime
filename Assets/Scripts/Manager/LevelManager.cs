using LudumDare56.Map;
using UnityEngine;

namespace LudumDare56.Manager
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { private set; get; }

        [SerializeField]
        private Node _firstNode;
        public Node FirstNode => _firstNode;

        private void Awake()
        {
            Instance = this;
        }
    }
}
