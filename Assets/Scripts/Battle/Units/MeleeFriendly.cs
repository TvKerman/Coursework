using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeFriendly : Friendly
{
    void Awake()
    {
        health = 100;
        armour = 30;
        initiative = 20;
        damage = 10;
        type = "melee friendly";
    }

    void Update()
    {
        
    }
}
