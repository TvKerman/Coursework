using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace TurnBasedBattleSystemFromRomchik
{
    public class Unit : MonoBehaviour
    {
        public int health;
        public int damage;
        public int initiative;
        public int armour;

        public string type;

        [SerializeField] Slider healthBar;
        public StayPoint stayPoint;

        public void Initialize(StayPoint currentStayPoint)
        {
            stayPoint = currentStayPoint;
        }

        public void SetHealth(int health)
        {
            healthBar.value = health;
        }

        public void SetMaxValue(int health)
        {
            healthBar.maxValue = health;
            healthBar.value = health;
        }

        void Start()
        {
        }

        void Update()
        {

        }

        public void Damage(int damage)
        {
            health -= damage;
            SetHealth(health);
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
