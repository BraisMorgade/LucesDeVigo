using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_controller : MonoBehaviour
{
    public float sensitivity=1000f;
    public Transform head;
    public float minPitch;
    public float maxPitch;
    float pitch = 0f;
    float yaw = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        yaw = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        pitch += Input.GetAxis("Mouse Y")* sensitivity * Time.deltaTime;

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        head.Rotate(Vector3.up * yaw);

        transform.localEulerAngles = new Vector3(-pitch, 0, 0);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
     
    }
}
