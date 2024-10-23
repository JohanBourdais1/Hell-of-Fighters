using Menu.Character;
using Menu.Map;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class Playsolo : MonoBehaviour
    {
        public GameObject map;
        public GameObject champ;

        public void OnPressMap()
        {
            if (MapSelect.MmapSelected)
            {
                map.SetActive(false);
                champ.SetActive(true);
            }
        }
        public void OnPressChar()
        {
            if (ChampSelect.Selected && MapSelect.MmapSelected)
            {
                SceneManager.LoadScene(MapSelect.MapSelected);
            }
        }
        public void Onreturn()
        {
            SceneManager.LoadScene("MultiPlayer");
        }
    }
}
