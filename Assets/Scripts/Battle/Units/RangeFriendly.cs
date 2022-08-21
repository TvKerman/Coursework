using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeFriendly : Friendly
{

    void Awake()
    {
        health = 50;
        armour = 10;
        initiative = 15;
        damage = 20;
        type = "range friendly";
    }

    void Update()
    {
    }
}
