/**
 * A singleton class to allow point-and-click movement of the marble.
 * 
 * It publishes a TargetSelected event which is invoked whenever a new target is selected.
 * 
 * Author: Malcolm Ryan
 * Version: 1.0
 * For Unity Version: 2022.3
 */

using UnityEngine;
using UnityEngine.InputSystem;

// note this has to run earlier than other classes which subscribe to the TargetSelected event
[DefaultExecutionOrder(-100)]
public class UIManager : MonoBehaviour
{
#region UI Elements
    [SerializeField] private Transform crosshair;
    [SerializeField] private Transform target;
    [SerializeField] private bool usingOldCamera = true;
    private Camera cam;
    public LayerMask wallLayer;
#endregion 

#region Singleton
    static private UIManager instance;
    static public UIManager Instance
    {
        get { return instance; }
    }
#endregion 

#region Actions
    private Actions actions;
    private InputAction mouseAction;
    private InputAction deltaAction;
    private InputAction selectAction;
#endregion

#region Events
    public delegate void TargetSelectedEventHandler(Vector3 worldPosition);
    public event TargetSelectedEventHandler TargetSelected;
#endregion

#region Init & Destroy
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There is more than one UIManager in the scene.");
        }

        instance = this;

        actions = new Actions();
        mouseAction = actions.mouse.position;
        deltaAction = actions.mouse.delta;
        selectAction = actions.mouse.select;

        cam = Camera.main;

        Cursor.visible = false;
        target.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        actions.mouse.Enable();
    }

    void OnDisable()
    {
        actions.mouse.Disable();
    }
#endregion Init

#region Update
    void Update()
    {
        MoveCrosshair();
        SelectTarget();
        Vector2 crossPos = deltaAction.ReadValue<Vector2>();
        crosshair.position = new Vector3(crossPos.x, crossPos.y, 0f);
                
    }

    private void MoveCrosshair() 
    {
        Vector2 mousePos = mouseAction.ReadValue<Vector2>();
        Ray ray = cam.ScreenPointToRay(mousePos);
        if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, wallLayer))
        {
            if(usingOldCamera)
            {
                Vector3 crosshairPos = hit.point;
                crosshairPos.y = 0.5f; // Stop the marble from trying to path into the ground.
                crosshair.position = crosshairPos;
            }
            else
            {
                // Rect screenBorder = new Rect(0, 0, Screen.width, Screen.height);
                Vector3 crosshairPos = hit.point;
                Mathf.Clamp(crosshairPos.x, 0, Screen.width/2);
                Mathf.Clamp(crosshairPos.z, 0, Screen.height/2);
                crosshairPos.y = 0.5f; // Stop the marble from trying to path into the ground.
                crosshair.position = crosshairPos;
            }
        }
        // FIXME: Move the crosshair position to the mouse position (in world coordinates)
        // crosshair.position = ...;
    }

    private void SelectTarget()
    {
        if (selectAction.WasPerformedThisFrame())
        {
            // set the target position and invoke 
            target.gameObject.SetActive(true);
            target.position = crosshair.position;     
            TargetSelected?.Invoke(target.position);
        }
    }

#endregion Update

}
