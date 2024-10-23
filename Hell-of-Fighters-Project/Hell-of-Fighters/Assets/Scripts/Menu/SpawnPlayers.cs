
using System.Collections.Generic;
using IA;
using Menu.Character;
using Menu.Map;
using Photon.Pun;
using Script_HealthBar;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Player = ScriptPLayer.Player;


namespace Menu
{
    public class SpawnPlayers : MonoBehaviourPun,IPunObservable
    {
        public GameObject playerPrefabs;
        public List<GameObject> champ = new List<GameObject>();
        public GameObject endScreen;
        public static bool isgame;
        public GameObject IA;
        public EnnemyIA EnnemyIa;
        private void Start()
        {
            Muqiueplayer musicPlayer = FindObjectOfType<Muqiueplayer>();
            if (musicPlayer != null)
            {
                Destroy(musicPlayer.gameObject);
            }
            if (CreateAndJoinRooms.Client)
            {
                foreach (var i in champ)
                {
                    if (i.name==ChampSelect.PlayerSelected)
                    {
                        playerPrefabs = i;
                    }
                }
                PhotonNetwork.Instantiate(playerPrefabs.name, new Vector3(8, 6, 10), Quaternion.identity);
            }
            else if (CreateAndJoinRooms.Host)
            {
                foreach (var i in champ)
                {
                    if (i.name==ChampSelect.PlayerSelected)
                    {
                        playerPrefabs = i;
                    }
                }
                PhotonNetwork.Instantiate(playerPrefabs.name, new Vector3(-8, 6, 10), Quaternion.identity);
            }
            else
            {
                foreach (var i in champ)
                {
                    if (i.name==ChampSelect.PlayerSelected)
                    {
                        playerPrefabs = i;
                    }
                }
                Instantiate(playerPrefabs, new Vector3(-8, 6, 10),quaternion.identity);
                Instantiate(IA, new Vector3(10, 2.5f, 10), Quaternion.identity);
                EnnemyIa = FindObjectOfType<EnnemyIA>();
            }
        }
        
        void Update()
        {
            if (CreateAndJoinRooms.Client || CreateAndJoinRooms.Host)
            {
                foreach (var i in Player.Players)
                {
                    if (i.isDead)
                    {
                        endScreen.SetActive(true);
                        isgame = true;
                        photonView.RPC("SyncCanvasElements", RpcTarget.Others);
                    }
                }
            }
            else
            {
                foreach (var i in Player.Players)
                {
                    if (i.isDead)
                    {
                        endScreen.SetActive(true);
                        isgame = true;
                    }
                }
                if (EnnemyIa.isDead)
                {
                    endScreen.SetActive(true);
                    isgame = true;
                }
            }
        }
        [PunRPC]
        void SyncCanvasElements()
        {
            isgame = true;
            endScreen.SetActive(true);
        }
        [PunRPC]
        void SyncCanvasElementsst()
        {
            isgame = false;
            endScreen.SetActive(false);
        }
        public void LeaveGame()
        {
            isgame = false;
            endScreen.SetActive(false);
            foreach (var i in Player.Players)
            {
                if (!i.solo)
                {
                    photonView.RPC("SyncCanvasElementsst", RpcTarget.Others);
                }
            }
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
            Player.Players = new List<Player>();
            CreateAndJoinRooms.Client = false;
            CreateAndJoinRooms.Host = false;
            ChampSelect.Selected = false;
            MapSelect.MmapSelected = false;
            SceneManager.LoadScene("Menu");
        }
        

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(isgame);
            }
            else
            {
                isgame = (bool)stream.ReceiveNext();
                endScreen.SetActive(isgame);
            }
        }
    }
}
