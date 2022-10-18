using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridBehavior : MonoBehaviour
{
    public int rows;
    public int columns;
    public int scale = 1;
    public GameObject gridPrefab;
    public GameObject boxPrefab;
    public GameObject voidPrefab;
    public Vector3 leftBottomLocation = new Vector3(0, 0, 0);
    
    private List<GameObject> _path = new List<GameObject>();
    private GameObject[,] gridArray;
    private int _startX = 0;
    private int _startY = 0;
    private int _endX = 2;
    private int _endY = 2;

    void Awake()
    {
        if (gridPrefab)
            //GenerateGrid(GenerateLevelData());
            GenerateGrid();
        else
            Debug.Log("Не удалось инициализировать Grid");
    }

    void GenerateGrid()
    {
        gridArray = new GameObject[columns, rows];
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

    void GenerateGrid(string[] data) {
        columns = Convert.ToInt32(data[0]);
        rows = Convert.ToInt32(data[1]);
        gridArray = new GameObject[columns, rows];
        List<List<char>> level = ConvertLevelData(data);

        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                if (level[i][j] == '*') {
                    GameObject obj = Instantiate(gridPrefab, new Vector3(leftBottomLocation.x + scale * i, leftBottomLocation.y, leftBottomLocation.z + scale * j), Quaternion.identity);
                    obj.transform.SetParent(gameObject.transform);
                    obj.GetComponent<GridStats>().x = i;
                    obj.GetComponent<GridStats>().y = j;
                    gridArray[i, j] = obj;
                }
                if (level[i][j] == 'b') {
                    GameObject obj = Instantiate(boxPrefab, new Vector3(leftBottomLocation.x + scale * i, leftBottomLocation.y, leftBottomLocation.z + scale * j), Quaternion.identity);
                    obj.transform.SetParent(gameObject.transform);
                    obj.GetComponent<GridStats>().x = i;
                    obj.GetComponent<GridStats>().y = j;
                    obj.GetComponent<GridStats>().SetIsOccupiedGridItem();
                    gridArray[i, j] = obj;
                }
                if (level[i][j] == 'v') {
                    GameObject obj = Instantiate(voidPrefab, new Vector3(leftBottomLocation.x + scale * i, leftBottomLocation.y, leftBottomLocation.z + scale * j), Quaternion.identity);
                    obj.transform.SetParent(gameObject.transform);
                    obj.GetComponent<GridStats>().x = i;
                    obj.GetComponent<GridStats>().y = j;
                    obj.GetComponent<GridStats>().SetIsOccupiedGridItem();
                    gridArray[i, j] = obj;
                }
            }
        }
    }

    List<List<char>> ConvertLevelData(string[] data) {
        List<List<char>> level = new List<List<char>>();
        for (int i = 2; i < data.Length; i++)
        {
            level.Add(new List<char>());
            for (int j = 0; j < data[i].Length; j++)
            {
                if (data[i][j] == '*' || data[i][j] == 'v' || data[i][j] == 'b')
                    level[i - 2].Add(data[i][j]);
            }
        }

        return level;
    }

    string[] GenerateLevelData() {
        string filename = Directory.GetCurrentDirectory() + "\\Assets\\Scenes\\Grid\\Grid2.txt";
        StreamReader levelfile = new StreamReader(filename);
        string level = levelfile.ReadToEnd();
        levelfile.Close();
        string[] data = level.Split(new char[] { ' ', '\n' });
        return data;
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
            if (item)
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
    
    public void FindPathBeforeGridItem(DynamicBattlePrototype.Unit unit, GridStats item) {
        GetGridItem(unit).GetComponent<GridStats>().SetIsFreeGridItem();

        SetStartCoordinates(unit);
        SetEndCoordinates(item);

        FindPath();

        unit.SetPath(path);
        unit.SelectPath();

        ClearPath();
        GetGridItem(unit).GetComponent<GridStats>().SetIsOccupiedGridItem();
    }

    public void FindPathToAttack(DynamicBattlePrototype.Unit unit, GridStats enemyGridItem) {
        unit.DeselectPath();
        unit.DeletePath();

        GetGridItem(unit).GetComponent<GridStats>().SetIsFreeGridItem();
        SetStartCoordinates(unit);

        enemyGridItem.SetIsFreeGridItem();
        SetEndCoordinates(enemyGridItem);
        
        FindPath();

        enemyGridItem.SetIsOccupiedGridItem();
        GetGridItem(unit).GetComponent<GridStats>().SetIsOccupiedGridItem();
    }
}



enum Direction {
    Up = 1,
    Right = 2,
    Down = 3,
    Left = 4,
}