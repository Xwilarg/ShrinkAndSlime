using UnityEngine;

namespace LudumDare56.Manager
{
    public class BGMManager : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _bgm;

        private void Update()
        {
            if (!_bgm.isPlaying)
            {
                _bgm.time = 12.97f;
                _bgm.Play();
            }
        }
    }
}
