using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : Enemy
{
    void Awake()
    {
        health = 50;
        armour = 10;
        initiative = 35;
        damage = 20;

        type = "range enemy";
    }
    void Update()
    {
        
    }
}
