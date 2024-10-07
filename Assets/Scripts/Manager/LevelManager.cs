using LudumDare56.Map;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LudumDare56.Manager
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { private set; get; }

        private void Awake()
        {
            Instance = this;
            SceneManager.LoadScene("Level Design", LoadSceneMode.Additive);
        }
    }
}
