using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnCircle : MonoBehaviour, IMiniGameLogic
{
    [SerializeField] private GameObject _bigCircle;
    [SerializeField] private GameObject _smallCircle;
    [SerializeField] private float _startSpawnRate = 0.01f;
    [SerializeField] private Text _text;
    private int _score = 0;
    private int _currentCountCircles = 0;
    private int _endMiniGame = 6;
    private float _randX;
    private float _randY;
    private float _spawnRate;
    private float _timer = 0f;
    Vector3 spawnCordinate;
    Vector3 spawnCordinate1;

    private bool isFree = true;

    Camera mainCam;
    GameObject bigCircle;
    GameObject smallCircle;

    void Start()
    {
        mainCam = Camera.main;
        _spawnRate = _startSpawnRate;
    }

    public void GameLogic() {
        _timer += Time.deltaTime;

        if (isFree)
        {
            _currentCountCircles++;
            if (isEndMiniGame)
            {
                return;
            }

            _randX = Random.Range(-350f, 350f);
            _randY = Random.Range(-200f, 200f);
            spawnCordinate = new Vector3(_randX, _randY, 900f);
            spawnCordinate1 = new Vector3(_randX, _randY, 899f);
            bigCircle = Instantiate(_bigCircle, spawnCordinate, Quaternion.identity);
            smallCircle = Instantiate(_smallCircle, spawnCordinate1, Quaternion.identity);

            isFree = false;
        }

        if (!isFree && _timer > _spawnRate)
        {
            bigCircle.transform.localScale = new Vector3(bigCircle.transform.localScale.x - 10,
                                                      bigCircle.transform.localScale.y - 10,
                                                      bigCircle.transform.localScale.z);
            _spawnRate += 0.3f;
            if (bigCircle.transform.localScale.x <= 50)
            {
                DestroyCircle(smallCircle, bigCircle);
            }
        }
        else if (Input.GetMouseButtonDown(0) && !isFree)
        {
            RaycastHit hit;
            if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hit))
            {
                Debug.Log(hit.collider.gameObject);
                SmallCircle hitSmallCircle = hit.collider.gameObject.GetComponent<SmallCircle>();
                BigCircle hitBigCircle = hit.collider.gameObject.GetComponent<BigCircle>();
                BackGround hitBackGround = hit.collider.gameObject.GetComponent<BackGround>();


                if (hitSmallCircle != null)
                {
                    AddScore(30);
                    DestroyCircle(smallCircle, bigCircle);
                }
                else if (hitBigCircle != null)
                {
                    AddScore(20);
                    DestroyCircle(smallCircle, bigCircle);
                }
                else if (hitBackGround != null)
                {
                    MinusScore(30);
                }
            }
        }
    }

    public void InitMiniGame() {
        _currentCountCircles = 0;
        _score = 0;
        _timer = 0f;
        _spawnRate = _startSpawnRate;
    }

    private void AddScore(int _score)
    {
        this._score += _score;
        _text.text = $"{this._score}";
    }

    private void MinusScore(int _score)
    {
        this._score -= _score;
        _text.text = $"{this._score}";
    }
    private void DestroyCircle(GameObject small, GameObject big)
    {
        Destroy(small);
        Destroy(big);
        isFree = true;
    }

    public int currentCircles {
        get { return _currentCountCircles; }
        set { _currentCountCircles = value; }
    }

    public bool isEndMiniGame {
        get { return _currentCountCircles >= _endMiniGame; }
    }
}
