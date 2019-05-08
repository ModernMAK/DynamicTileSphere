using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    public float dragSpeed = 2;

    private void Awake()
    {
        targetCamera = Camera.main;
    }

    private Vector3 dragOrigin;
    private Camera targetCamera;


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(0)) return;

        Vector3 pos = targetCamera.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector3 move = new Vector3(-pos.y * dragSpeed,  pos.x * dragSpeed,0);

        transform.Rotate(move * Time.deltaTime, Space.Self);
    }
}