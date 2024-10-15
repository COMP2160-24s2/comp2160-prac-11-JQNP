using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    [Range(5,10)]
    [SerializeField] private float orthographicCameraZoomRange;
    [Range(50,120)]
    [SerializeField] private float perspectiveCameraZoomRange;
    
    private Camera cam;

    #region Actions
        private Actions actions;
        private InputAction zoomAction;
    #endregion

    void Awake()
    {
        actions = new Actions();
        zoomAction = actions.camera.zoom;
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(cam.orthographic)
        {
            cam.orthographicSize = orthographicCameraZoomRange;
        }
        else
        {
            cam.fieldOfView = perspectiveCameraZoomRange;
        }
    }
}
