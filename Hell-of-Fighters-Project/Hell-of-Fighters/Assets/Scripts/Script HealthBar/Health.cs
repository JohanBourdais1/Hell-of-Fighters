
using Photon.Pun;
using UnityEngine;

namespace Script_HealthBar
{
    public class Health : MonoBehaviourPun
    {
        public int health;
        public int healthMax;
        public int damage1;
        public int damage2;
        public int damage3;
        public int damage4;
        public int damage5;
        public int damage6;
        public bool isHit;
        public bool invincible;
        
        public void TakeDamage(int damage)
        {
            if (!invincible)
            {
                health -= damage;
                isHit = true;
            }
        }

        public void Heal(int heal = 15)
        {
            health += heal;
        }
        
        
    }
}
