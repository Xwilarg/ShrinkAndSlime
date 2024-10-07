using UnityEngine;
using UnityEngine.SceneManagement;

namespace LudumDare56.Menu
{
    public class MenuManager : MonoBehaviour
    {
        public void Play()
        {
            SceneManager.LoadScene("Main");
        }
    }
}
