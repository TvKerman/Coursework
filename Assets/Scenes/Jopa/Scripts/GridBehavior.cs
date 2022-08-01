using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBehavior : MonoBehaviour
{
    public bool findDistance = false;
    public int rows = 10;
    public int columns = 10;
    public int scale = 1;
    public GameObject gridPrefab;
    public Vector3 leftBottomLocation = new Vector3(0, 0, 0);
    public GameObject[,] gridArray;
    public int startX = 0;
    public int startY = 0;
    public int endX = 2;
    public int endY = 2;
    public List<GameObject> path = new List<GameObject>();
    // Start is called before the first frame update
    void Awake()
    {
        gridArray = new GameObject[columns, rows];
        if (gridPrefab)
            GenerateGrid();
        else
            Debug.Log("Нету префаба Grid");
    }

    // Update is called once per frame
    void Update()
    {
        if (findDistance) {
            SetDistance();
            SetPath();
            findDistance = false;
        }
    }

    void GenerateGrid()
    {

        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                GameObject obj = Instantiate(gridPrefab, new Vector3(leftBottomLocation.x + scale * i, leftBottomLocation.y, leftBottomLocation.z + scale * j), Quaternion.identity);
                obj.transform.SetParent(gameObject.transform);
                obj.GetComponent<GridStats>().x = i;
                obj.GetComponent<GridStats>().y = j;
                gridArray[i, j] = obj;

            }
        }
    }

    void SetDistance() {
        InitialSetUp();
        int x = startX;
        int y = startY;
        int[] testArray = new int[rows * columns];
        for (int step = 1; step < rows * columns; step++) {
            foreach (GameObject obj in gridArray) {
                if (obj && obj.GetComponent<GridStats>().visited == step - 1)
                    TestFourDirections(obj.GetComponent<GridStats>().x, obj.GetComponent<GridStats>().y, step);
            }
        }
    }

    void InitialSetUp()
    {
        foreach (GameObject obj in gridArray)
        {
            if (obj)
            {
                obj.GetComponent<GridStats>().visited = -1;
            }
        }

        gridArray[startX, startY].GetComponent<GridStats>().visited = 0;
    }

    void SetPath() {
        int step;
        int x = endX;
        int y = endY;
        List<GameObject> tempList = new List<GameObject> ();
        path.Clear();
        if (gridArray[endX, endY] && gridArray[endX, endY].GetComponent<GridStats>().visited > 0)
        {
            path.Add(gridArray[x, y]);
            step = gridArray[x, y].GetComponent<GridStats>().visited - 1;
        }
        else {
            Debug.Log("Can't reach the desired location");
            return;
        }

        for (int i = step; step > -1; step--) {
            if (TestDirection(x, y, step, Direction.Up)) {
                tempList.Add(gridArray[x, y + 1]);
            }
            if (TestDirection(x, y, step, Direction.Right))
            {
                tempList.Add(gridArray[x + 1, y]);
            }
            if (TestDirection(x, y, step, Direction.Down))
            {
                tempList.Add(gridArray[x, y - 1]);
            }
            if (TestDirection(x, y, step, Direction.Left))
            {
                tempList.Add(gridArray[x - 1, y]);
            }

            GameObject tempObj = FindClosest(gridArray[endX, endY].transform, tempList);
            path.Add(tempObj);
            x = tempObj.GetComponent<GridStats>().x;
            y = tempObj.GetComponent<GridStats>().y;
            tempList.Clear();
        }
    }

    bool TestDirection(int x, int y, int step, Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return (y + 1 < rows && gridArray[x, y + 1] && gridArray[x, y + 1].GetComponent<GridStats>().visited == step);
            case Direction.Down:
                return (y - 1 > -1 && gridArray[x, y - 1] && gridArray[x, y - 1].GetComponent<GridStats>().visited == step);
            case Direction.Left:
                return (x - 1 > -1 && gridArray[x - 1, y] && gridArray[x - 1, y].GetComponent<GridStats>().visited == step);
            case Direction.Right:
                return (x + 1 < columns && gridArray[x + 1, y] && gridArray[x + 1, y].GetComponent<GridStats>().visited == step);
        }
        return false;
    }

    void TestFourDirections(int x, int y, int step) {
        if (TestDirection(x, y, -1, Direction.Up)) {
            SetVisited(x, y + 1, step);
        }
        if (TestDirection(x, y, -1, Direction.Right)) {
            SetVisited(x + 1, y, step);
        }
        if (TestDirection(x, y, -1, Direction.Down)) {
            SetVisited(x, y - 1, step);
        }
        if (TestDirection(x, y, -1, Direction.Left)) {
            SetVisited(x - 1, y, step);
        }
    }

    void SetVisited(int x, int y, int step) {
        if (gridArray[x, y]) {
            gridArray[x, y].GetComponent<GridStats>().visited = step;
        }
    }

    GameObject FindClosest(Transform targetLocation, List<GameObject> list) {
        float currentDistance = scale * rows * columns;
        int indexNumber = 0;
        for (int i = 0; i < list.Count; i++) {
            if (Vector3.Distance(targetLocation.position, list[i].transform.position) < currentDistance) {
                currentDistance = Vector3.Distance(targetLocation.position, list[i].transform.position);
                indexNumber = i;
            }
        }

        return list[indexNumber];
    }
}

enum Direction {
    Up = 1,
    Right = 2,
    Down = 3,
    Left = 4,
}