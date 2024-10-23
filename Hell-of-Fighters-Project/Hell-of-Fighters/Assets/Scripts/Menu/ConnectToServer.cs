using Photon.Pun;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class ConnectToServer : MonoBehaviourPunCallbacks
    {
        // Start is called before the first frame update
        void Start()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            SceneManager.LoadScene("Lobby");
        }
        
    }
}
