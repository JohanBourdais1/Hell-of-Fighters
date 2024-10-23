using System;
using Menu.Character;
using Menu.Map;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class MultiPlayer : MonoBehaviour
    {
        public GameObject map;
        public GameObject champ;

        public void OnPress2()
        {
            if (ChampSelect.Selected && MapSelect.MmapSelected)
            {
                PhotonNetwork.LoadLevel(MapSelect.MapSelected);
            }
        }
        public void OnPress1()
        {
            if (MapSelect.MmapSelected)
            {
                map.SetActive(false);
                champ.SetActive(true);
            }
        }
    }
}
