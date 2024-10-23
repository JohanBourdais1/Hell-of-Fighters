using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script_HealthBar
{
    public class HealthBar : MonoBehaviourPun
    {
        public Slider slider;
        public TextMeshProUGUI nameplayer;
        
        public void SetMaxHealth(int health)
        {
            slider.maxValue = health;
            slider.value = slider.maxValue;
        }

        public void SetHealth(int health)
        {
            slider.value = health;
        }
    }
}
