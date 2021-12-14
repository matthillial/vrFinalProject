using UnityEngine;
using UnityEngine.InputSystem;

public class GraspGrabber : Grabber
{
    public InputActionProperty grabAction;

    Grabbable currentObject;
    Grabbable grabbedObject;

    // Start is called before the first frame update
    void Start()
    { 
        grabbedObject = null;
        currentObject = null;
        Debug.Log("Start");
        Debug.Log(currentObject == null);
        Debug.Log(grabbedObject == null);


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
        Debug.Log("Grab");
        Debug.Log(currentObject == null);
        Debug.Log(currentObject == null);
        if (currentObject && grabbedObject == null)
        {
            Debug.Log("AHHHHHH");
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

            grabbedObject.transform.parent = this.transform;
            Debug.Log("moving");
        }
    }

    public override void Release(InputAction.CallbackContext context)
    {
        Debug.Log("Release");
        if (grabbedObject)
        {
            if (grabbedObject.GetComponent<Rigidbody>())
            {
                grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
                grabbedObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }

            grabbedObject.SetCurrentGrabber(null);
            grabbedObject.transform.parent = null;
            grabbedObject = null;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter ");
        Debug.Log(currentObject == null);
        Debug.Log(other);
        //Debug.Log(other.GetComponent<Grabbable>());

        if (currentObject == null && other.GetComponent<Grabbable>())
        {
            Debug.Log("Trigger Enter");
            currentObject = other.gameObject.GetComponent<Grabbable>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (currentObject)
        {
            Debug.Log("Trigger Exit");
            if (other.GetComponent<Grabbable>() && currentObject.GetInstanceID() == other.GetComponent<Grabbable>().GetInstanceID())
            {
                currentObject = null;
            }
        }
    }
}
