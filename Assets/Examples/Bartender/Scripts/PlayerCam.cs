using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    float xRotation;
    float yRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void Rotate(float pitchChange, float yawChange)
    {
        Vector3 rotationVector = transform.rotation.eulerAngles;
        float pitch = rotationVector.x;
        float yaw = rotationVector.y;
        pitch += pitchChange * Time.deltaTime * sensX;
        yaw += yawChange * Time.deltaTime * sensY;
        //pitch = Mathf.Clamp(pitch, -90f, 90f);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}
