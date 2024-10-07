using UnityEngine;
using UnityEngine.SceneManagement;

namespace LudumDare56.Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { private set; get; }

        private void Awake()
        {
            Instance = this;
        }

        private bool _canPlay;
        public bool CanPlay
        {
            set
            {
                _canPlay = value;
                Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.None;
            }
            get => _canPlay;
        }

        public void Retry()
        {
            SceneManager.LoadScene("Main");
        }

        public void Menu()
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
