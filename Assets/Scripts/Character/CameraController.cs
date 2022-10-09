using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform character;
    private Vector3 offset = new Vector3(-1f, 1f, 0f);

    private float zoomSpeed = 4f;
    private float maxZoom = 15f;
    private float minZoom = 5f;

    private float pitch = 2f;
    private float currentZoom = 10f;

    private float rotationSpeed = 100f;
    private float rotationValue = 0f;

    void Update()
    {
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
        rotationValue += Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
    }

    private void LateUpdate()
    {
        transform.position = character.position + offset * currentZoom;
        transform.LookAt(character.position + Vector3.up * pitch);

        transform.RotateAround(character.position, Vector3.up, rotationValue);
    }
}
