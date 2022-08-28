using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridStats : MonoBehaviour
{
    public int visited = -1;
    public int x = 0;
    public int y = 0;

    private bool _isSelected = false;
    public bool _isPath = false;
    private bool _isDefaultColorInit = false;
    public bool _isFree = true;
    private bool _isEnemyInGridItem = false;
    private bool _isRangedAttackGridItem = false;
    private Color _defaultColor;

    public void SelectGridItem() {
        if (gameObject.tag != "GridItem")
            return;

        if (!_isDefaultColorInit && gameObject.GetComponent<MeshRenderer>().materials[0].color != Color.green && 
                                    gameObject.GetComponent<MeshRenderer>().materials[0].color != Color.blue && 
                                    gameObject.GetComponent<MeshRenderer>().materials[0].color != Color.red &&
                                    gameObject.GetComponent<MeshRenderer>().materials[0].color != Color.yellow) {
            _defaultColor = gameObject.GetComponent<MeshRenderer>().materials[0].color;
            _isDefaultColorInit = true;
        }

        if (_isPath)
            gameObject.GetComponent<MeshRenderer>().materials[0].color = Color.green;
        else if (!_isPath && _isSelected)
            gameObject.GetComponent<MeshRenderer>().materials[0].color = Color.blue;
        else if (_isEnemyInGridItem)
            gameObject.GetComponent<MeshRenderer>().materials[0].color = Color.red;
        else if (_isRangedAttackGridItem)
            gameObject.GetComponent<MeshRenderer>().materials[0].color = Color.yellow;
        else
            gameObject.GetComponent<MeshRenderer>().materials[0].color = _defaultColor;
        
    }

    public void SelectItem() {
        _isSelected = true;
    }
    
    public void AddItemInPath() {
        _isPath = true;
    }

    public void DeselectItem() {
        _isSelected = false;
        _isPath = false;
        _isRangedAttackGridItem = false;
        _isEnemyInGridItem = false;
    }

    public void DeleteItemInPath() {
        _isPath = false;
    }

    public void SetIsFreeGridItem() {
        _isFree = true;
    }

    public void SetIsOccupiedGridItem() {
        _isFree = false;
    }

    public void SelectInEnemyGridItem() {
        _isEnemyInGridItem = true;
    }

    public void DeselectInEnemyGridItem() {
        _isEnemyInGridItem = false;
    }

    public void SelectInRangedAttack() {
        _isRangedAttackGridItem = true;
    }

    public void DeselectIsRangedAttack() {
        _isRangedAttackGridItem= false;
    }

    public bool isFree {
        get {
            return _isFree;
        }
    }

    public bool isSelected {
        get {
            return _isSelected;
        }
    }

    public bool isEnemyInGridItem {
        get {
            return _isEnemyInGridItem;
        }
    }

    public bool isRangedAttack {
        get {
            return _isRangedAttackGridItem;
        }
    }

    public static bool operator ==(GridStats gridItem1, GridStats gridItem2)
    {
        return (gridItem1.x == gridItem2.x) && (gridItem1.y == gridItem2.y);
    }

    public static bool operator !=(GridStats gridItem1, GridStats gridItem2) {
        return !(gridItem1 == gridItem2);
    }
}
