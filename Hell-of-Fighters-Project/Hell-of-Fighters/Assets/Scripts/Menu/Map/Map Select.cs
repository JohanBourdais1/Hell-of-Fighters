using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu.Map
{
    public class MapSelect : MonoBehaviour
    {
        public List<Map> map = new List<Map>();
        public GameObject charCellPrefab;
        public static bool MmapSelected;
        public static string MapSelected;
        
        void Start()
        {
            foreach (var i in map)
            {
                SpawnMap(i);
            }
        }

        private void SpawnMap(Map map)
        {
            GameObject charcell = Instantiate(charCellPrefab, transform);
            Image artwork = charcell.transform.Find("Image").GetComponent<Image>();
            TextMeshProUGUI name = charcell.transform.Find("NameRect").GetComponentInChildren<TextMeshProUGUI>();
            artwork.sprite = map.characterSprite;
            name.text = map.characterName;
        }
    }
}
