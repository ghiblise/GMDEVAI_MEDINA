using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bonus item #1 
public class MouseLook : MonoBehaviour
{
    [SerializeField] float mouseSens = 150f;
    [SerializeField] Transform player;
    float xRot = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        player.Rotate(Vector3.up * mouseX);
    }
}
