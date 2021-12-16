using UnityEngine;
using UnityEngine.InputSystem;

public class GraspGrabber : Grabber
{
    public InputActionProperty grabAction;

    Grabbable currentObject;
    Grabbable grabbedObject;
    public bool canToggle;

    // Start is called before the first frame update
    void Start()
    {
        canToggle = true;
        grabbedObject = null;
        currentObject = null;

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

    }

    public override void Grab(InputAction.CallbackContext context)
    {
        canToggle = false;
        if (currentObject && grabbedObject == null)
        {
            if (currentObject.GetCurrentGrabber() != null)
            {
                currentObject.GetCurrentGrabber().Release(new InputAction.CallbackContext());
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
            }

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
