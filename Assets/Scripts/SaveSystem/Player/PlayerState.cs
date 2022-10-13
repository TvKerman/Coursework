using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour, IStateSave
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private Movement playerMovement;

    public void SaveState(ref SaveData saveData) {
        saveData.playerData.position = gameObject.transform.position;
        saveData.playerData.rotation = gameObject.transform.localEulerAngles;

        saveData.playerData.isPlayerCanMove = playerMovement.PlayerCanMove;
        saveData.playerData.isPlayerNotLose = gameManager.PlayerNotLose;
        saveData.playerData.isPlayerNotWin = gameManager.PlayerNotWin;

        saveData.playerData.cameraPosition = playerCamera.transform.position;
        saveData.playerData.cameraRotation = playerCamera.transform.localEulerAngles;
    }

    public void LoadState(SaveData saveData) {
        playerMovement.StopPlayer(saveData.playerData.position);
        gameObject.transform.localEulerAngles = saveData.playerData.rotation;


        playerMovement.PlayerCanMove = saveData.playerData.isPlayerCanMove;
        gameManager.PlayerNotLose = saveData.playerData.isPlayerNotLose;
        gameManager.PlayerNotWin = saveData.playerData.isPlayerNotWin;

        playerCamera.transform.localEulerAngles = saveData.playerData.cameraRotation;
        playerCamera.transform.position = saveData.playerData.position;
    }

}
