using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    public void LoadState(SaveData saveData) {
        gameObject.SetActive(saveData.battleData.meleeEnemies[row].isActive);

        health = saveData.battleData.meleeEnemies[row].maxHealtPoints;
        damage = saveData.battleData.meleeEnemies[row].damage;
        initiative = saveData.battleData.meleeEnemies[row].initiative;
    }
}
