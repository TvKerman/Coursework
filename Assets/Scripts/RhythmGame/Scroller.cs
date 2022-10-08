using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class Scroller : MonoBehaviour, IMiniGameLogic
{
    private float tempo = 1f;
    [SerializeField] private GameObject square;

    [SerializeField] private ButtonController ButtonBlue;
    [SerializeField] private ButtonController ButtonGreen;
    [SerializeField] private ButtonController ButtonRed;
    [SerializeField] private TextMeshProUGUI Text;

    private int _currentCount = 0;
    private int _endMiniGame = 10;
    private int _score = 0;

    private double _baseTime = 0.8;
    private double _timer;

    private System.Random _random = new System.Random();

    private List<SpawnPoint> _spawnPointsList;
    private List<GameObject> _allSquares;
    private List<GameObject> _allTirgetButton;
    private List<GameObject> _allDeleteButton;

    void Start()
    {
        _spawnPointsList = FindObjectsOfType<SpawnPoint>().Select(x => (SpawnPoint)x).ToList();
        _allSquares = new List<GameObject>();
        _allTirgetButton = new List<GameObject>();
        _allDeleteButton = new List<GameObject>();
        _timer = RandomTimeSpawn();
    }

    public void GameLogic() {
        _score += ButtonBlue.ButtonLogic();
        _score += ButtonGreen.ButtonLogic();
        _score += ButtonRed.ButtonLogic();
        ClearDeleteButtons();
        SetScoreBoard();

        if (_timer <= 0.0 && !isStopSpawn)
        {
            _currentCount++;

            GameObject currentSquare;
            SpawnPoint currentSpawnPoint = GetRandomPos(_spawnPointsList);
            Vector3 currentCordinate = new Vector3(currentSpawnPoint.transform.localPosition.x,
                                                    currentSpawnPoint.transform.localPosition.y,
                                                        currentSpawnPoint.transform.localPosition.z);
            currentSquare = Instantiate(square, currentCordinate, Quaternion.identity);
            currentSquare.transform.parent = ButtonBlue.gameObject.transform.parent;
            currentSquare.transform.localPosition = currentCordinate + new Vector3(0, currentSpawnPoint.transform.parent.position.y, 0);
            _allSquares.Add(currentSquare);
            _timer = RandomTimeSpawn();
        }
        else if (!isStopSpawn) {
            _timer -= Time.deltaTime;
        }

    }

    public void LogicOfPhysics() {
        foreach (var currentSquare in _allSquares)
        {
            currentSquare.transform.position = new Vector3(currentSquare.transform.position.x,
                                        currentSquare.transform.position.y - tempo * 4,
                                        currentSquare.transform.position.z);
        }
    }

    public void InitMiniGame() {
        ButtonBlue.SetDefaultButton();
        ButtonGreen.SetDefaultButton();
        ButtonRed.SetDefaultButton();

        _currentCount = 0;
        _score = 0;
        SetScoreBoard();
        _timer = 0.0;
    }

    private bool isStopSpawn {
        get { return _currentCount >= _endMiniGame; }
    }

    public int currentCount {
        get { return _currentCount; }
        set { _currentCount = value; }
    }

    public bool isEndMiniGame {
        get { return _currentCount >= _endMiniGame && _allSquares.Count == 0; }
    }

    public int MaxScore {
        get { return _endMiniGame * 30; }
    }

    public int GetScore {
        get { return _score; }
    }

    public List<GameObject> Squares {
        get { return _allSquares; }
    }

    public List<GameObject> TirgetButtons {
        get { return _allTirgetButton;}
    }

    public List<GameObject> DeleteButtons
    {
        get { return _allDeleteButton; }
    }

    private void ClearDeleteButtons() {
        _score -= _allDeleteButton.Count * 30;
        for (int index = _allDeleteButton.Count - 1; index > -1; index--) {
            _allSquares.Remove(_allDeleteButton[index]);
            _allTirgetButton.Remove(_allDeleteButton[index]);
            GameObject tmp = _allDeleteButton[index];
            _allDeleteButton.RemoveAt(index);
            Destroy(tmp);
        }
        
    }

    private void SetScoreBoard() {
        Text.text = "Score: " + _score.ToString();
    }

    private double RandomTimeSpawn() {
        int _delta = _random.Next(3);
        int _sign = _random.Next(2);
        _sign = _sign == 0 ? 1 : -1;
        double _doubleDelta = (double)_delta / 10;
        return _baseTime + _sign * _doubleDelta;
    }

    private SpawnPoint GetRandomPos(List<SpawnPoint> spawnPoints)
    {
        int randomValue = Random.Range(0, 3);

        return spawnPoints[randomValue];
    }
}
