using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackFriendUnit : FriendUnit
{

    private void Start()
    {
        InitBaseData();
        distance = 4;
        healthPoint = 30;
        damage = 40;
        initiative = 2;
        distanceAttack = 7;
        InitHealthPoint();
        UpdateSlider();
    }

    //public int distanceAttack {
    //    get {
    //        return _distanceAttack; 
    //    }
    //}
}
