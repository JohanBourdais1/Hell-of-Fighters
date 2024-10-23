using UnityEngine;

namespace Menu
{
    public class Muqiueplayer : MonoBehaviour
    {
        public AudioClip AudioClip;
        private static Muqiueplayer instance;
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        public void PlayMusic()
        {
            if (AudioClip != null)
            {
                AudioSource audioSource = GetComponent<AudioSource>();
                if (audioSource == null)
                {
                    audioSource = gameObject.AddComponent<AudioSource>();
                }
                audioSource.loop = true;
                audioSource.clip = AudioClip;
                audioSource.Play();
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            Muqiueplayer musicPlayer = FindObjectOfType<Muqiueplayer>();
            if (musicPlayer != null)
            {
                musicPlayer.PlayMusic();
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
