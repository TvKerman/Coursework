using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackEnemyUnit : EnemyUnit
{
    private void Start()
    {
        InitBaseData();
        healthPoint = 25;
        damage = 30;
        initiative = 1;
        distance = 4;
        distanceAttack = 7;
    }
}
