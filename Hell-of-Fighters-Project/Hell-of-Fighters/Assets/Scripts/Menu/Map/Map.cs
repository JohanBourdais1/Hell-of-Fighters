using UnityEngine;

namespace Menu.Map
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Map", menuName = "Map")]
    public class Map : ScriptableObject
    {
        public string characterName;

        public Sprite characterSprite;
    }
}
