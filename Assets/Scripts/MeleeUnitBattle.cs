using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeUnitBattle : MonoBehaviour
{
    private Camera mainCam;
    private int layerMask;

    public GameObject enemy;

    EnemyStats enemyStats = new EnemyStats();

    float enemyHp;
    float enemyArmor;
    float enemyDamage;

    void Start()
    {
        layerMask = LayerMask.NameToLayer("MeleeEnemy");
        mainCam = Camera.main;

        enemyHp = enemyStats.EnemyHp;
        enemyArmor = enemyStats.EnemyArmor;
        enemyDamage = enemyStats.EnemyDamage;

        if (layerMask == -1)
        {
            Debug.Log("Layer does not exist");
        }
        else
        {
            layerMask = 1 << layerMask;
            Debug.Log(layerMask);
        }
    }

    void Update()
    {
         
        if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), 
                                                Mathf.Infinity, layerMask))
        {
            if (Input.GetMouseButtonDown(0))
            {
                enemyHp = enemyHp -(10f / enemyArmor) * 100;
                Debug.Log($"Ok{enemyHp}");
                if (enemyHp <= 0)
                {
                    enemy.SetActive(false);
                }
            }
        }
    }
}
