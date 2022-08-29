using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendUnit : DynamicBattlePrototype.Unit
{
    public void InitBaseData()
    {
        distance = 6;
        isSelected = false;
        actionPoint = 2;
        x = (int)transform.position.x;
        y = (int)transform.position.z;
        InitActionPoint();
    }
}
