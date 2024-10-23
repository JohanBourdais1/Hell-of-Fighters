using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class StartGame : MonoBehaviour
    {
        public void OnLoad()
        {
            SceneManager.LoadScene("Loading");
        }

        public void Onreturn()
        {
            SceneManager.LoadScene("Menu");
        }

        public void OnPlaysolo()
        {
            SceneManager.LoadScene("Play Solo");
        }
    }
}
