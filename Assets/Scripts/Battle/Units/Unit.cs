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
        [SerializeField] private Animator _animator;
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

        public void Damage(int damage)
        {
            health -= damage;

            if (health <= 0)
            {
                healthBar.gameObject.SetActive(false);
                AnimationDead();
            }
            else 
            {
                SetHealth(health);
            }
        }

        public void AnimationAttack() {
            if (_animator != null) {
                _animator.SetBool("Attack", true);
            }
        }

        public void AnimationHit() {
            if (_animator != null) {
                _animator.SetBool("Hit", true);
            }
        }

        public void AnimationDead() {
            if (_animator != null) {
                _animator.SetBool("Dead", true);
                gameObject.tag = "Dead";
            }
        }

        public void Dead() {
            Destroy(gameObject);
        }
    }
}
