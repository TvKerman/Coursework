using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackEnemyUnit : EnemyUnit
{
    private void Start()
    {
        InitBaseData();
        healthPoint = 80;
        damage = 20;
        initiative = 6;
    }
}
