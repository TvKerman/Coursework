using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Scroller : MonoBehaviour
{
    private float tempo = 1f;
    [SerializeField] private GameObject square;
    private bool isStarted = false;

    private GameObject currentSquare;

    private List<SpawnPoint> spawnPointsList;

    public GameObject CurrentSquare
    {
        get { return currentSquare; }
        set { currentSquare = value; }
    }

    public bool IsStarted
    {
        get {return isStarted;}
        set {isStarted = value;}
    }
    void Start()
    {
        spawnPointsList = FindObjectsOfType<SpawnPoint>().Select(x => (SpawnPoint)x).ToList();
    }

    void Update()
    {
        if (!isStarted)
        {
            SpawnPoint currentSpawnPoint = GetRandomPos(spawnPointsList);
            Vector3 currentCordinate = new Vector3(currentSpawnPoint.transform.localPosition.x,
                                                    currentSpawnPoint.transform.localPosition.y,
                                                        currentSpawnPoint.transform.localPosition.z);
            currentSquare = Instantiate(square, currentCordinate, Quaternion.identity);

            currentSquare.transform.position = new Vector3(currentSquare.transform.position.x,
                                                    currentSquare.transform.position.y - tempo * Time.deltaTime,
                                                    currentSquare.transform.position.z);

            isStarted = true;
        }
        if (isStarted)
        {
            currentSquare.transform.position = new Vector3(currentSquare.transform.position.x,
                                                    currentSquare.transform.position.y - tempo,
                                                    currentSquare.transform.position.z);
        }
    }

    private SpawnPoint GetRandomPos(List<SpawnPoint> spawnPoints)
    {
        int randomValue = Random.Range(0, 3);

        return spawnPoints[randomValue];
    }
}
