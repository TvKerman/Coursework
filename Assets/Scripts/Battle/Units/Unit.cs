using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TurnBasedBattleSystemFromRomchik
{
    public class Unit : MonoBehaviour
    {
        private int maxHealtPoints;
        
        public int health;
        public int damage;
        public int initiative;
        public int armour;

        public string type;

        [SerializeField] Slider healthBar;
        [SerializeField] TextMeshProUGUI healtPoints;
        [SerializeField] GameObject IconItem;
        [SerializeField] private Animator _animator;
        public StayPoint stayPoint;

        private void Start()
        {
            SetItemIconPosition();
        }

        // Прошу ногами не бить
        private void SetItemIconPosition() {
            Vector3 iconPosition = new Vector3(0, 0, 0);
            if (gameObject.transform.localPosition.z > -15.0f)
            {
                iconPosition.y = 130;
            }
            else if (gameObject.transform.localPosition.z > -17.0f)
            {
                iconPosition.y = 0;
            }
            else
            {
                iconPosition.y = -130;
            }

            if (gameObject.transform.localPosition.x < -4.0f)
            {
                iconPosition.x = -700;
            }
            else if (gameObject.transform.localPosition.x <= -2.0f)
            {
                iconPosition.x = -590;
            }
            else if (gameObject.transform.localPosition.x <= 1f)
            {
                iconPosition.x = 590;
            }
            else
            {
                iconPosition.x = 705;
            }

            IconItem.transform.localPosition = iconPosition;
        }

        private void SetHealtPointsUI() {
            healtPoints.text = health.ToString() + "/" + maxHealtPoints.ToString();
        }

        public void Initialize(StayPoint currentStayPoint)
        {
            stayPoint = currentStayPoint;
        }

        public void SetHealth(int health)
        {
            healthBar.value = health;
            SetHealtPointsUI();
        }

        public void SetMaxValue(int health)
        {
            healthBar.maxValue = health;
            healthBar.value = health;
            maxHealtPoints = health;
            SetHealtPointsUI();
        }

        public void Damage(int damage)
        {
            health -= damage;

            if (health <= 0)
            {
                //healthBar.gameObject.SetActive(false);
                IconItem.SetActive(false);
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

        public void SetFalseUnitUI() {
            IconItem.SetActive(false);
        }

        public void SetTrueUnitUI() {
            IconItem.SetActive(true);
        }

        public void Dead() {
            Destroy(gameObject);
        }
    }
}
