using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnCircle : MonoBehaviour, IMiniGameLogic
{
    [SerializeField] private GameObject _bigCircle;
    [SerializeField] private GameObject _smallCircle;

    [SerializeField] private float _startSpawnRate = 0f;

    [SerializeField] private Text _text;

    private int _score = 0;
    private int _currentCountCircles = 0;
    private int _endMiniGame = 11;

    private float _randX;
    private float _randY;

    private float _speedMove = 3f;
    private float _speedTime = 1f;
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


        if (Input.GetMouseButtonDown(0) && !isFree)
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
                    DestroyCircle(smallCircle, bigCircle);
                    
                }
            }
        }
    }

    public void LogicOfPhysics() {
        if (!isFree)
        {
            bigCircle.transform.localScale = new Vector3(bigCircle.transform.localScale.x - _speedTime,
                                                      bigCircle.transform.localScale.y - _speedTime,
                                                      bigCircle.transform.localScale.z);
            _spawnRate += 0.05f;

            if (bigCircle.transform.localScale.x <= smallCircle.transform.localScale.x)
            {
                DestroyCircle(smallCircle, bigCircle);
            }
            if (!isFree)
            {
                if (-350f + Mathf.Abs(bigCircle.transform.position.x) < -150f && bigCircle.transform.position.x < 340)
                {
                    bigCircle.transform.Translate(Vector3.right * _speedMove);
                    smallCircle.transform.Translate(Vector3.right * _speedMove);
                }
                else if (350f - Mathf.Abs(bigCircle.transform.position.x) < 150f && bigCircle.transform.position.x > -340)
                {
                    bigCircle.transform.Translate(Vector3.left * _speedMove);
                    smallCircle.transform.Translate(Vector3.left * _speedMove);
                }

                if (200f - Mathf.Abs(bigCircle.transform.position.y) < 150 && bigCircle.transform.position.y > -180)
                {
                    bigCircle.transform.Translate(Vector3.down * _speedMove);
                    smallCircle.transform.Translate(Vector3.down * _speedMove);
                }
                else if (-200f + Mathf.Abs(bigCircle.transform.position.y) < -150 && bigCircle.transform.position.y > 180)
                {
                    bigCircle.transform.Translate(Vector3.up * _speedMove);
                    smallCircle.transform.Translate(Vector3.up * _speedMove);
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

    public int MaxScore {
        get { return (_endMiniGame - 1) * 30; }
    }

    public int GetScore {
        get { return _score; }
    }
    //private void SlideLeft(GameObject big, GameObject small)
    //{
    //    if (!isFree && _timer > _spawnRate)
    //    {
    //        big.transform.position = new Vector3(big.transform.position.x - 50f, big.transform.position.y, big.transform.position.x);
    //        small.transform.position = new Vector3(big.transform.position.x - 50f, big.transform.position.y, big.transform.position.x);
    //    }
    //}
    //
    //private void SlideRight(GameObject big, GameObject small)
    //{
    //    if (!isFree && _timer > _spawnRate)
    //    {
    //        big.transform.position = new Vector3(big.transform.position.x + 50f, big.transform.position.y, big.transform.position.x);
    //        small.transform.position = new Vector3(big.transform.position.x + 50f, big.transform.position.y, big.transform.position.x);
    //    }
    //}
    //
    //
    //private void SlideUp(GameObject big, GameObject small)
    //{
    //    if (!isFree && _timer > _spawnRate)
    //    {
    //        big.transform.position = new Vector3(big.transform.position.x, big.transform.position.y + 50f, big.transform.position.x);
    //        small.transform.position = new Vector3(big.transform.position.x, big.transform.position.y + 50f, big.transform.position.x);
    //    }
    //}
    //
    //private void SlideDown(GameObject big, GameObject small)
    //{
    //    if (!isFree && _timer > _spawnRate)
    //    {
    //        big.transform.position = new Vector3(big.transform.position.x, big.transform.position.y - 50f, big.transform.position.x);
    //        small.transform.position = new Vector3(big.transform.position.x, big.transform.position.y - 50f, big.transform.position.x);
    //    }
    //}

    private bool RandomSlide()
    {
        int randomValue = Random.Range(0, 1);

        return randomValue == 0;
    }
}
