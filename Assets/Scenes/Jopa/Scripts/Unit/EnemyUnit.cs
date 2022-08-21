using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : DynamicBattlePrototype.Unit
{
    //private GameObject _priorityTarget;
    //private bool _isPriorityTargetFind = false;

    public void InitBaseData()
    {
        distance = 5;
        isSelected = false;
        actionPoint = 2;
        x = (int)transform.position.x;
        y = (int)transform.position.z;
        InitActionPoint();
    }

    //public bool isPriorityTargetFind {
    //    get { 
    //        return _isPriorityTargetFind; 
    //    }
    //}

    //public GameObject priorityTarget
    //{
    //    get
    //    {
    //        if (_priorityTarget == null) throw new System.Exception();
    //        return _priorityTarget;
    //    }
    //    set {
    //        _priorityTarget = value;
    //    }
    //}
}
