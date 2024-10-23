using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu.Character
{
    public class ChampSelect : MonoBehaviour
    {
        public List<Champ> charcters = new List<Champ>();
        public GameObject charCellPrefab;
        public static bool Selected;
        public static string PlayerSelected;
        
        void Start()
        {
            foreach (var i in charcters)
            {
                SpawnCharcter(i);
            }
        }
        
        private void SpawnCharcter(Champ champ)
        {
            GameObject charcell = Instantiate(charCellPrefab, transform);
            Image artwork = charcell.transform.Find("artwork").GetComponent<Image>();
            TextMeshProUGUI name = charcell.transform.Find("namerect").GetComponentInChildren<TextMeshProUGUI>();
            artwork.sprite = champ.characterSprite;
            name.text = champ.characterName;
        }
    }
}
