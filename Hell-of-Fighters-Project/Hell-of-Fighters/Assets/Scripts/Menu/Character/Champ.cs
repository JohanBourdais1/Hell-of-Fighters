using UnityEngine;

namespace Menu.Character
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Character", menuName = "Character")]
    public class Champ : ScriptableObject
    {
        public string characterName;

        public Sprite characterSprite;
    }
}
