using UnityEngine;

namespace LudumDare56.Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { private set; get; }

        private void Awake()
        {
            Instance = this;
        }

        public bool CanPlay { set; get; }
    }
}
