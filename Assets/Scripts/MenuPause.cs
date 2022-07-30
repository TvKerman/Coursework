using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPause : MonoBehaviour
{
    // Start is called before the first frame update
    public void PauseBtn() {
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) {
            gameObject.SetActive(false);
        }
    }
}
