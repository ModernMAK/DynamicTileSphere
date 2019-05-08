using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private float _zoom = 0.5f;

    private void Awake()
    {
        targetCamera = Camera.main;
    }

    public float ZoomSpeed = 0.1f;

    public float MaxZoom = 3;
    public float MinZoom = 2;
    private Camera targetCamera;
    public float LerpSpeed = 1f; 

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    private Vector3 Target
    {
        get { return Mathf.Lerp(MaxZoom, MinZoom, _zoom) * -transform.forward; }
    }

    private Vector3 Position
    {
        get { return targetCamera.transform.position; }
        set { targetCamera.transform.position = value; }
    }

    void Update()
    {
        var deltaZoom = Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * ZoomSpeed;
        _zoom = Mathf.Clamp01(_zoom + deltaZoom);
        
        if ((Target - Position).sqrMagnitude <= Mathf.Epsilon)
            Position = Target;
        else 
            Position = Vector3.Lerp(Position, Target, Time.deltaTime * LerpSpeed);
    }
}