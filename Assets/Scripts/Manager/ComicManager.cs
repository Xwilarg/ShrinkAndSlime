using Live2D.Cubism.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LudumDare56.Manager
{
    public class ComicManager : MonoBehaviour
    {
        [SerializeField]
        private CubismParameter _param;

        [SerializeField]
        private Transform _container;

        private bool _needIncr = false;

        int _nb = 1;

        private void Update()
        {
            if (_needIncr)
            {
                _nb++;
                _needIncr = false;
            }
        }

        private void LateUpdate()
        {
            _param.Value = _nb;
        }

        public void OnNext(InputAction.CallbackContext value)
        {
            if (value.phase == InputActionPhase.Started && _container.gameObject.activeInHierarchy)
            {
                if (_param.Value == _param.MaximumValue)
                {
                    GameManager.Instance.CanPlay = true;
                    _container.gameObject.SetActive(false);
                }
                else
                {
                    _needIncr = true;
                }
            }
        }
    }
}
