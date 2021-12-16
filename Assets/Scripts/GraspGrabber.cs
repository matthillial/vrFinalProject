using UnityEngine;
using UnityEngine.InputSystem;

public class GraspGrabber : Grabber
{
    public InputActionProperty grabAction;

    Grabbable currentObject;
    Grabbable grabbedObject;
    public bool canToggle;
    public Transform RightController;
    public Transform LeftController;
    public Transform PositionOnGrab;
    public Vector3 ControllerDiffOnGrab;
    public Vector3 ScaleOnGrab;
    Vector3 ControllerDiff;
    bool scale;


    // Start is called before the first frame update
    void Start()
    {
        canToggle = true;
        grabbedObject = null;
        currentObject = null;
        scale = false;

        grabAction.action.performed += Grab;
        grabAction.action.canceled += Release;
    }

    private void OnDestroy()
    {
        grabAction.action.performed -= Grab;
        grabAction.action.canceled -= Release;
    }

    // Update is called once per frame
    void Update()
    {
        if(scale)
        {
            ControllerDiff = RightController.position - LeftController.position;
            grabbedObject.transform.localScale = new Vector3((Vector3.Magnitude(ControllerDiff) / Vector3.Magnitude(ControllerDiffOnGrab)) * ScaleOnGrab.x, (Vector3.Magnitude(ControllerDiff) / Vector3.Magnitude(ControllerDiffOnGrab)) * ScaleOnGrab.y, (Vector3.Magnitude(ControllerDiff) / Vector3.Magnitude(ControllerDiffOnGrab)) * ScaleOnGrab.z);
        }
    }

    public override void Grab(InputAction.CallbackContext context)
    { 
        canToggle = false;
        if (currentObject && grabbedObject == null)
        {
            if (currentObject.GetCurrentGrabber() != null)
            {
                Debug.Log("Two Handed");
                scale = true;
                //grabbedObject.GetComponent<IsGrabbed>().twoHand = true;
            }

            grabbedObject = currentObject;
            grabbedObject.SetCurrentGrabber(this);
            

            if (grabbedObject.GetComponent<Rigidbody>())
            {
                grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
            }

            if (grabbedObject.GetComponent<Dial>())
            {
                grabbedObject.GetComponent<Dial>().moving = true;
            }

            grabbedObject.transform.parent = this.transform;
            Debug.Log("moving");

            PositionOnGrab = this.transform;
            ControllerDiffOnGrab = RightController.position - LeftController.position;
            ScaleOnGrab = grabbedObject.transform.localScale;
        }



    }

    public override void Release(InputAction.CallbackContext context)
    {
        canToggle = true;
        Debug.Log("Release");
        if (grabbedObject)
        {
            if (grabbedObject.GetComponent<Rigidbody>())
            {
                grabbedObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }

            if (grabbedObject.GetComponent<Dial>())
            {
                grabbedObject.GetComponent<Dial>().Release(this.transform.rotation);
                Debug.Log("Grasp Release Dial");
            }

            if (grabbedObject.GetComponent<IsGrabbed>())
            {
                grabbedObject.GetComponent<IsGrabbed>().padIsGrabbed = false;
                grabbedObject.GetComponent<IsGrabbed>().twoHand = false;
            }
            
            scale = false;
            grabbedObject.SetCurrentGrabber(null);
            grabbedObject.transform.parent = null;
            grabbedObject = null;
            currentObject = null;

        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (currentObject == null && other.GetComponent<Grabbable>())
        {
            currentObject = other.gameObject.GetComponent<Grabbable>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (currentObject)
        {
            if (other.GetComponent<Grabbable>() && currentObject.GetInstanceID() == other.GetComponent<Grabbable>().GetInstanceID())
            {
                currentObject = null;
            }
        }
    }
}
