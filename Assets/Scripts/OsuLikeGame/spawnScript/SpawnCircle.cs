using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnCircle : MonoBehaviour
{
    [SerializeField] private GameObject _bigCircle;
    [SerializeField] private GameObject _smallCircle;
    [SerializeField] private float spawnRate = 0.01f;
    [SerializeField] private Text text;
    private int score = 0;
    private float randX;
    private float randY;
    Vector3 spawnCordinate;
    Vector3 spawnCordinate1;

    private bool isFree = true;

    Camera mainCam;
    GameObject bigCircle;
    GameObject smallCircle;

    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        if (isFree)
        {
            randX = Random.Range(-350f, 350f);
            randY = Random.Range(-200f, 200f);
            spawnCordinate = new Vector3(randX, randY, 900f);
            spawnCordinate1 = new Vector3(randX, randY, 899f);
            bigCircle = Instantiate(_bigCircle, spawnCordinate, Quaternion.identity);
            smallCircle = Instantiate(_smallCircle, spawnCordinate1, Quaternion.identity);
            isFree = false;
        }

        if (!isFree && Time.time > spawnRate)
        {
            bigCircle.transform.localScale = new Vector3(bigCircle.transform.localScale.x - 10,
                                                      bigCircle.transform.localScale.y - 10,
                                                      bigCircle.transform.localScale.z);
            spawnRate += 0.3f;
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

    private void AddScore(int _score)
    {
        score += _score;
        text.text = $"{score}";
    }

    private void MinusScore(int _score)
    {
        score -= _score;
        text.text = $"{score}";
    }
    private void DestroyCircle(GameObject small, GameObject big)
    {
        Destroy(small);
        Destroy(big);
        isFree = true;
    }
}
