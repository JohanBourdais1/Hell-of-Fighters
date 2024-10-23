using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class MainMenu : MonoBehaviour
    {
        public string scenetoload;
        
        public void Play()
        {
            SceneManager.LoadScene(scenetoload);
        }

        public void Setting()
        {
            SceneManager.LoadScene("Option");
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
