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

    private GameObject currentSquare;

    [SerializeField]private ColliderController colliderController;

    void Start()
    {
        scroller = FindObjectOfType<Scroller>();
        theSR = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (colliderController != null)
        {
            isActiveToPress = colliderController.IsActiveToPress;
            isExitCollider = colliderController.IsExitCollider;
        }

        currentSquare = scroller.CurrentSquare;

        if (Input.GetKeyDown(keyToPress))
        {
            theSR.sprite = pressedImage;

            // Дальше Бога нет
            if (isActiveToPress && currentSquare.transform.position.x == -50 && Input.GetKeyDown("q")) 
            {
                DestroySquare();
            }
            else if (isActiveToPress && currentSquare.transform.position.x == 0 && Input.GetKeyDown("w"))
            {
                DestroySquare();
            }
            else if (isActiveToPress && currentSquare.transform.position.x == 50 && Input.GetKeyDown("e"))
            {
                DestroySquare();
            }
            //Дальше тоже
        }

        if (Input.GetKeyUp(keyToPress))
        {
            theSR.sprite = defImage;
        }

        if (isExitCollider)
        {
            Destroy(currentSquare);
            scroller.IsStarted = false;
            colliderController.IsActiveToPress = false;
            colliderController.IsExitCollider = false;
        }
    }


    private void DestroySquare()
    {
        Destroy(currentSquare);
        scroller.IsStarted = false;
        colliderController.IsActiveToPress = false;
    }
}
