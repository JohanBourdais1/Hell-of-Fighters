using System;
using Menu.Character;
using Menu.Map;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu
{
    public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
    {
        public InputField createInput;
        public InputField joinInput;
        public static bool Client;
        public static bool Host;
        public GameObject lobby;
        public GameObject map;
        public GameObject champ;
        public GameObject leave;
        public GameObject returnbtn;

        private void Start()
        {
            
        }

        public void CreateRoom()
        {
            if (createInput.text.Length>0)
            {
                PhotonNetwork.CreateRoom(createInput.text,new RoomOptions{MaxPlayers = 2,BroadcastPropsChangeToAll = true});
                Host = true;
                returnbtn.SetActive(false);
            }
        }

        public void JoinRoom()
        {
            if (joinInput.text.Length > 0)
            {
                PhotonNetwork.JoinRoom(joinInput.text);
                Client = true;
            }
        }

        public override void OnJoinedRoom()
        {
            returnbtn.SetActive(false);
            lobby.SetActive(false);
            leave.SetActive(true);
            if (!Host)
            {
                champ.SetActive(true);
            }
            else
            {
                map.SetActive(true);
            }
        }

        public void OnLeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("Menu");
            PhotonNetwork.Disconnect();
            Client = false;
            Host = false;
            ChampSelect.Selected = false;
            MapSelect.MmapSelected = false;
        }
        public void Onreturn()
        {
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene("MultiPlayer");
        }
    }
}
