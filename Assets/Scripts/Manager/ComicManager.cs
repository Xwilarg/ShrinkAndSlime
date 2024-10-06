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

        private void LateUpdate()
        {
            if (_needIncr)
            {
                _param.Value += 1;
                _needIncr = false;
            }
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
