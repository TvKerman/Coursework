using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    void Awake()
    {
        health = 100;
        armour = 30;
        initiative = 10;
        damage = 20;
        type = "melee enemy";

    }

    public void LoadState(SaveData saveData) {
        gameObject.SetActive(saveData.battleData.meleeEnemies[row].isActive);

        health = saveData.battleData.meleeEnemies[row].maxHealtPoints;
    }
}
