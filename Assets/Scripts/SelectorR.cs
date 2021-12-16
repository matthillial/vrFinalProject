using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectorR : MonoBehaviour
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
                Debug.Log("SelectorR Hit");
                dial = hit.collider.GetComponentInChildren<Dial>();
                //hit.collider.GetCom

                if (dial)
                {
                    Debug.Log("Its a Dial!");
                    if(dial.miniature)
                    {
                        Debug.Log("Its a Baby Dial!");
                        dial = dial.initMaxiature(transform);
                        //maxiature.GetCompon
                    }
                    //dial.GrabbedR();
                    //Debug.Log("Dial");
                    //dial.grabbedR = true;
                    grabbed = true;
                    
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
        Debug.Log("Right Release");
        if (dial) { dial.grabbedR = false;  }
        grabbed = false;
        active = false;
    }
}
