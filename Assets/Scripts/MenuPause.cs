using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPause : MonoBehaviour
{
    

    public void PauseBtn() {
        gameObject.SetActive(true);

    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) {

            FindObjectOfType<Movement>().PauseIsOver();
            gameObject.SetActive(false);
        }
    }

}
