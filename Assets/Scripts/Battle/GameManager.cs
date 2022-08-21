using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TurnBasedBattleSystemFromRomchik
{
    public class GameManager : MonoBehaviour
    {
        private Camera _mainCam;
        private StepSystem _stepSystem;
        private Spawn _spawnSystem;


        private void Start()
        {
            List<Unit> unitList = MakeListOfUnits();
            _stepSystem = new StepSystem(unitList);
            _mainCam = Camera.main;
        }

        void Update()
        {
            Unit currentUnit = _stepSystem.UnitInList;
            if (Input.GetMouseButtonDown(0) &&
                _stepSystem.isAnimationOn is false &&
                (currentUnit is MeleeFriendly ||
                 currentUnit is RangeFriendly))
            {
                RaycastHit hit;
                if (Physics.Raycast(_mainCam.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    MeleeEnemy isMeleeEnemyOnScene = FindObjectOfType<MeleeEnemy>();
                    _stepSystem.PlyerAttack(hit, isMeleeEnemyOnScene);
                    StartDeleyFriendly(currentUnit);
                }

            }
            else if (_stepSystem.isAnimationOn is false &&
                    (_stepSystem.UnitInList is MeleeEnemy ||
                     _stepSystem.UnitInList is RangeEnemy))
            {
                Enemy attackingEnemy = _stepSystem.UnitInList as Enemy;
                _stepSystem.EnemyAttack(attackingEnemy);
                StartDeleyEnemy(attackingEnemy);
            }
        }

        private List<Unit> MakeListOfUnits()
        {
            List<Unit> unitList = FindObjectsOfType<Unit>().Select(x => (Unit)x).ToList();

            foreach (Unit unit in unitList)
            {
                unit.SetMaxValue(unit.health);
            }
            return unitList;

        }

        private void StartDeleyEnemy(Unit enemy) => StartCoroutine(_stepSystem.DeleyEnemy(enemy));
        private void StartDeleyFriendly(Unit friendly) => StartCoroutine(_stepSystem.DeleyFriendly(friendly));
    }
}
