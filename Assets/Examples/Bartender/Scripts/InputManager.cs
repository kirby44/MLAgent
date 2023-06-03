using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Debug.Log("Mouse position is " + Input.mousePosition);
        }
    }
}
