using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectorL : MonoBehaviour
{
    bool grabbed = false;

    public InputActionProperty point;
    bool active = false;
    Dial dial;

    // Start is called before the first frame update
    void Start()
    {
        point.action.performed += Activate;
        point.action.canceled += Release;
    }

    // Update is called once per frame
    void Update()
    {
        if (active && !grabbed)
        {

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {
                //laserPointer.SetPosition(1, new Vector3(0, 0, hit.distance));
                //Debug.Log("Hit");
                dial = hit.collider.GetComponentInChildren<Dial>();

                if (dial)
                {
                    Debug.Log("Dial");
                    //dial.grabbedR = true;
                    grabbed = true;
                    dial.GrabbedL();
                }
            }
        }

        /*if (grabbed && !point.ReadValue<bool>())
        {
            dial.grabbedR = false;
            grabbed = false;
        }*/
    }

    public void Activate(InputAction.CallbackContext context)
    {
        active = true;
    }

    public void Release(InputAction.CallbackContext context)
    {
        Debug.Log("Left Release");
        if (dial) { dial.grabbedL = false; }
        grabbed = false;
        active = false;
    }
}
