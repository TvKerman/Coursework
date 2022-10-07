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

        [SerializeField] private GameObject _osuMiniGame;
        [SerializeField] private GameObject _rhythmMiniGame;

        private Vector3 _tempPosition;
        private Vector3 _tempLocalEulerAngles;

        private Vector3 _positionOSU = new Vector3(0, 0, 0);
        private Vector3 _positionRhythm = new Vector3(0, 0, 900);

        private RaycastHit hit;


        private GameObject _osu;
        private GameObject _rhythm;

        private IMiniGameLogic _osuGameLogic;
        private IMiniGameLogic _rhythmGameLogic;

        private bool _isActiveOsuMiniGame = false;
        private bool _isActiveRhythmMiniGame = false;



        private void Start()
        {
            List<Unit> unitList = MakeListOfUnits();
            _stepSystem = new StepSystem(unitList);
            _mainCam = Camera.main;

            _osu = Instantiate(_osuMiniGame, _positionOSU, Quaternion.identity);
            _rhythm = Instantiate(_rhythmMiniGame, _positionRhythm, Quaternion.identity);
            _osuGameLogic = _osu.GetComponentInChildren<SpawnCircle>();
            _rhythmGameLogic = _rhythm.GetComponent<Scroller>();
            _osu.SetActive(false);
            _rhythm.SetActive(false);
        }

        void Update()
        {
            Unit currentUnit = _stepSystem.UnitInList;

            FrameMiniGame(_osu, _osuGameLogic, ref _isActiveOsuMiniGame, currentUnit);
            FrameMiniGame(_rhythm, _rhythmGameLogic, ref _isActiveRhythmMiniGame, currentUnit);
            
            if (Input.GetMouseButtonDown(0) &&
                _stepSystem.isAnimationOn is false &&
                currentUnit is Friendly)
            {

                bool isRaycastHit = false;
                if (!_isActiveOsuMiniGame && !_isActiveRhythmMiniGame)
                {
                    isRaycastHit = Physics.Raycast(_mainCam.ScreenPointToRay(Input.mousePosition), out hit);
                }
                if (isRaycastHit && hit.collider.gameObject.tag != "Unit") {
                    return;
                }

                if (currentUnit is RangeFriendly && !_isActiveOsuMiniGame && isRaycastHit) {
                    StartMiniGame(_osu, _osuGameLogic, ref _isActiveOsuMiniGame);
                    SetCameraInMiniGame();
                }

                if (currentUnit is MeleeFriendly && !_isActiveRhythmMiniGame && isRaycastHit) {
                    StartMiniGame(_rhythm, _rhythmGameLogic, ref _isActiveRhythmMiniGame);
                    SetCameraInMiniGame();
                }

            }
            else if (_stepSystem.isAnimationOn is false &&
                    _stepSystem.UnitInList is Enemy) {
                EnemyAttack();
            }
        }

        private void FixedUpdate()
        {
            if (_isActiveOsuMiniGame) {
                _osuGameLogic.LogicOfPhysics();
            }
            if (_isActiveRhythmMiniGame) {
                _rhythmGameLogic.LogicOfPhysics();
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

        private void FrameMiniGame(GameObject miniGame, IMiniGameLogic miniGameLogic, ref bool isStartMiniGame, Unit currentUnit) {
            if (isStartMiniGame) {
                miniGameLogic.GameLogic();
                if (miniGameLogic.isEndMiniGame) {
                    EndMiniGame(miniGame, ref isStartMiniGame);
                    SetCameraInBattleScene();
                    PlayerAttack(currentUnit, miniGameLogic);
                }
            }
        } 

        private void EnemyAttack() {
            Enemy attackingEnemy = _stepSystem.UnitInList as Enemy;
            Unit friendly = _stepSystem.EnemyAttack(attackingEnemy);
            StartDeleyEnemy(attackingEnemy, friendly);
        }

        private void PlayerAttack(Unit currentUnit, IMiniGameLogic miniGame) {
            MeleeEnemy isMeleeEnemyOnScene = FindObjectOfType<MeleeEnemy>();
            _stepSystem.PlayerAttack(hit, isMeleeEnemyOnScene, miniGame);
            Unit enemy = hit.collider.gameObject.GetComponent<Enemy>();
            StartDeleyFriendly(currentUnit, enemy);
        }

        private void StartMiniGame(GameObject miniGame, IMiniGameLogic miniGameLogic, ref bool isStartMiniGame) {
            isStartMiniGame = true;
            miniGame.SetActive(isStartMiniGame);
            miniGameLogic.InitMiniGame();
        }

        private void EndMiniGame(GameObject miniGame, ref bool isStartMiniGame) {
            isStartMiniGame = false;
            miniGame.SetActive(isStartMiniGame);
        }

        private void SetCameraInMiniGame() {
            _tempPosition = new Vector3(_mainCam.transform.position.x, _mainCam.transform.position.y, _mainCam.transform.position.z);
            _tempLocalEulerAngles = new Vector3(_mainCam.transform.localEulerAngles.x, _mainCam.transform.localEulerAngles.y, _mainCam.transform.localEulerAngles.z);
            if (_isActiveOsuMiniGame)
            {
                _mainCam.transform.position = new Vector3(0, -16f, 400f);
            }
            else if (_isActiveRhythmMiniGame)
            {
                _mainCam.transform.position = new Vector3(0, -16f, 600f);
            }
            _mainCam.transform.localEulerAngles = new Vector3(0, 0, 0);
        }

        private void SetCameraInBattleScene() 
        {
            _mainCam.transform.position = _tempPosition;
            _mainCam.transform.localEulerAngles = _tempLocalEulerAngles;
        }

        private void StartDeleyEnemy(Unit enemy, Unit friendly) => StartCoroutine(_stepSystem.DeleyEnemy(enemy, friendly));
        private void StartDeleyFriendly(Unit friendly, Unit enemy) => StartCoroutine(_stepSystem.DeleyFriendly(friendly, enemy));
    }
}
