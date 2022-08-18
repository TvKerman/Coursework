using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackFriendUnit : FriendUnit
{
    private void Start()
    {
        InitBaseData();
        healthPoint = 100;
        damage = 20;
        initiative = 5;
    }
}
