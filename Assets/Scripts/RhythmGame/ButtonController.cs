using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    private SpriteRenderer theSR;

    [SerializeField] private Sprite defImage;
    [SerializeField] private Sprite pressedImage;

    [SerializeField] private KeyCode keyToPress;

    private bool isActiveToPress = false;
    private bool isExitCollider = false;
    private Scroller scroller;

    [SerializeField]private ColliderController colliderController;

    private List<GameObject> temp;

    void Start()
    {
        scroller = GetComponentInParent<Scroller>();
        theSR = GetComponent<SpriteRenderer>();
        temp = new List<GameObject>();
    }

    public int ButtonLogic() {
        int score = 0;
        bool input = Input.GetKeyDown(keyToPress);


        if (colliderController != null)
        {
            isActiveToPress = colliderController.IsActiveToPress;
            isExitCollider = colliderController.IsExitCollider;
        }

        if (input)
        {
            theSR.sprite = pressedImage;
            foreach (var currentSquare in scroller.TirgetButtons)
            {
                if ((int)currentSquare.transform.position.x == (int)gameObject.transform.position.x)
                {
                    temp.Add(currentSquare);
                    score += 30;
                }
            }
        }
        if (input && temp.Count == 0) { 
            score -= 30;
        }

        DestroySquare();


        if (Input.GetKeyUp(keyToPress))
        {
            SetDefaultButton();
        }

        return score;
    }

    public void SetDefaultButton() {
        theSR.sprite = defImage;
    }

    private void DestroySquare()
    {
        for (int index = temp.Count - 1; index > -1; index--)
        {
            scroller.Squares.Remove(temp[index]);
            scroller.TirgetButtons.Remove(temp[index]);
            GameObject tmp = temp[index];
            temp.RemoveAt(index);
            Destroy(tmp);
        }
        colliderController.IsActiveToPress = false;
    }
}
