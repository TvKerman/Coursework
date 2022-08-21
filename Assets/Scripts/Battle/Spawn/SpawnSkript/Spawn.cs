using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Spawn : MonoBehaviour
{
    private List<Enemy> enemyList;
    private List<Friendly> friendlyList;

    //public List<Unit> SpawnUnits(List<Unit> units)
    //{
    //    List <Unit> unitList = new List<Unit>();
    //    
    //    for (int i = 0; i < units.Count; i++)
    //    {
    //        Unit newUnit = Instantiate(units[i], stayPoints[i].transform);
    //        newUnit.Initialize(stayPoints[i]);
    //        unitList.Add(newUnit);
    //    }
    //
    //    return unitList;
    //}
}
