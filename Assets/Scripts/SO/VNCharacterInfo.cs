using UnityEngine;

namespace LudumDare56.VN
{
    [CreateAssetMenu(menuName = "ScriptableObject/VNCharacterInfo", fileName = "VNCharacterInfo")]
    public class VNCharacterInfo : ScriptableObject
    {
        public string Name;
        public string DisplayName;
        public Sprite Image;
    }
}