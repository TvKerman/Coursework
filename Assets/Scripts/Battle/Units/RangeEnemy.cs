using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : Enemy
{

    public void LoadState(SaveData saveData)
    {
        gameObject.SetActive(saveData.battleData.rangeEnemies[row].isActive);

        health = saveData.battleData.rangeEnemies[row].maxHealthPoints;
        damage = saveData.battleData.rangeEnemies[row].damage;
        initiative = saveData.battleData.rangeEnemies[row].initiative;

    }
}
