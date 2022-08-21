using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBehavior : MonoBehaviour
{
    public int rows = 10;
    public int columns = 10;
    public int scale = 1;
    public GameObject gridPrefab;
    public Vector3 leftBottomLocation = new Vector3(0, 0, 0);
    
    private List<GameObject> _path = new List<GameObject>();
    private GameObject[,] gridArray;
    private int _startX = 0;
    private int _startY = 0;
    private int _endX = 2;
    private int _endY = 2;
    // Start is called before the first frame update
    void Awake()
    {
        gridArray = new GameObject[columns, rows];
        if (gridPrefab)
            GenerateGrid();
        else
            Debug.Log("Не удалось инициализировать Grid");
    }

    // Update is called once per frame
    //void Update()
    //{
    //    //if (findDistance) {
    //    //    SetDistance();
    //    //    SetPath();
    //    //    findDistance = false;
    //    //}
    //}

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
        int x = _startX;
        int y = _startY;
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

        gridArray[_startX, _startY].GetComponent<GridStats>().visited = 0;
    }

    void SetPath() {
        int step;
        int x = _endX;
        int y = _endY;
        List<GameObject> tempList = new List<GameObject> ();
        _path.Clear();
        if (gridArray[_endX, _endY] && gridArray[_endX, _endY].GetComponent<GridStats>().visited > 0)
        {
            _path.Add(gridArray[x, y]);
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

            GameObject tempObj = FindClosest(gridArray[_endX, _endY].transform, tempList);
            _path.Add(tempObj);
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
                return (y + 1 < rows && gridArray[x, y + 1] && gridArray[x, y + 1].GetComponent<GridStats>().visited == step && gridArray[x, y + 1].GetComponent<GridStats>().isFree);
            case Direction.Down:
                return (y - 1 > -1 && gridArray[x, y - 1] && gridArray[x, y - 1].GetComponent<GridStats>().visited == step && gridArray[x, y - 1].GetComponent<GridStats>().isFree);
            case Direction.Left:
                return (x - 1 > -1 && gridArray[x - 1, y] && gridArray[x - 1, y].GetComponent<GridStats>().visited == step && gridArray[x - 1, y].GetComponent<GridStats>().isFree);
            case Direction.Right:
                return (x + 1 < columns && gridArray[x + 1, y] && gridArray[x + 1, y].GetComponent<GridStats>().visited == step && gridArray[x + 1, y].GetComponent<GridStats>().isFree);
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

    public void SetRangeMovement(int x, int y, int distance) {
        _startX = x;
        _startY = y;
        InitialSetUp();
        int[] testArray = new int[rows * columns];
        for (int step = 1; step < distance; step++) {
            foreach (GameObject obj in gridArray) {
                if (obj && obj.GetComponent<GridStats>().visited == step - 1)
                    TestFourDirections(obj.GetComponent<GridStats>().x, obj.GetComponent<GridStats>().y, step);
            }
        }

        SetSelectRange(distance);
    }

    private void SetSelectRange(int distance) {
        foreach (GameObject item in gridArray)
        {
            if (item.GetComponent<GridStats>().visited < distance && item.GetComponent<GridStats>().visited != -1)
                item.GetComponent<GridStats>().SelectItem();
        }
    }

    public void UpdateMap() {
        foreach (GameObject item in gridArray) {
            item.GetComponent<GridStats>().SelectGridItem();
        }
    }

    public void SetStartCoordinates(DynamicBattlePrototype.Unit unit) {
        _startX = unit.x;
        _startY = unit.y;
    }

    public void SetEndCoordinates(int x, int y) {
        _endX = x;
        _endY = y;
    }

    public void SetEndCoordinates(GridStats item) {
        _endX = item.x;
        _endY = item.y;
    }

    public void FindPath() {
        SetDistance();
        SetPath();
    }

    public void ResetMap() {
        InitialSetUp();
        DeselectAllGridItems();
        UpdateMap();
    }

    private void DeselectAllGridItems() {
        foreach (var item in gridArray) {
            item.GetComponent<GridStats>().DeselectItem();
        }
    }

    public GameObject GetGridItem(int x, int y) {
        return gridArray[x, y];
    }

    public GameObject GetGridItem(DynamicBattlePrototype.Unit unit) {
        return GetGridItem(unit.x, unit.y);
    }

    public List<GameObject> path {
        get {
            return _path;
        }
    }

    public GameObject lastItemInPath {
        get {
            if (_path.Count == 0) throw new System.Exception();
            return _path[0];
        }
    }

    public GameObject firstItemInPath {
        get
        {
            if (_path.Count == 0) throw new System.Exception();
            return _path[^1];
        }     
    }

    public void ClearPath() {
        _path.Clear();
    }

    public void RangeAttackDistance(DynamicBattlePrototype.Unit unit, int distance) {
        foreach (var obj in gridArray) {
            GridStats item = obj.GetComponent<GridStats>();
            if (Mathf.Sqrt(Mathf.Pow(item.x - unit.x, 2) + Mathf.Pow(item.y - unit.y, 2)) <= (float)distance) {
                item.SelectInRangedAttack();
            }
        }
    }
}



enum Direction {
    Up = 1,
    Right = 2,
    Down = 3,
    Left = 4,
}