using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectObject : MonoBehaviour
{
    public LineRenderer laserPointer;
    public InputActionProperty toggle;
    public InputActionProperty select;
    bool canSelect;
    bool canEdit;
    public GameObject selected;
    public GameObject editButton;
    public Transform laserOrigin;
    public Transform spawn_point;
    public Material selectable_material;
    public Material default_material;
    public GameObject[] select_options;
    RaycastHit hit;

    public GameObject controller;
    public GameObject drumstick;


    // Start is called before the first frame update
    void Start()
    {
        selected = null;
        editButton = null;
        controller.SetActive(true);
        drumstick.SetActive(false);
        toggle.action.performed += ToggleMode;
        select.action.performed += Select;
        laserPointer.enabled = true;
        canSelect = false;
    }

    private void OnDestroy()
    {
        toggle.action.performed -= ToggleMode;
        select.action.performed -= Select;
    }

    // Update is called once per frame
    void Update()
    {
        if (laserPointer.enabled)
        {
            laserPointer.SetPosition(0, laserOrigin.position);

            if (Physics.Raycast(laserOrigin.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {
                laserPointer.SetPosition(1, hit.point);
                if (hit.collider.GetComponent<Selectable>())
                {
                    canSelect = true;
                    //Debug.Log("this item is selectable");
                    selected = select_options[hit.collider.GetComponent<Selectable>().select_id];
                    laserPointer.material = selectable_material;
                }
                else
                {
                    canSelect = false;
                    selected = null;
                    laserPointer.material = default_material;
                }

                if (hit.collider.GetComponent<EditButton>())
                {
                    //canEdit = true;
                    //Debug.Log("canEdit is " + canEdit + " after raycast");
                    editButton = hit.collider.gameObject;
                    laserPointer.material = selectable_material;
                }
                else
                {
                    //Debug.Log("canEdit is " + canEdit + " when not on raycast");
                    editButton = null;
                }

            }
        }
    }

    void Select(InputAction.CallbackContext context)
    {
        Debug.Log("select method");
        if (canSelect)
        {
            GameObject spawned = Instantiate(selected, spawn_point.position, Quaternion.identity);
            spawned.AddComponent<Grabbable>();
            spawned.AddComponent<Rigidbody>();
            spawned.GetComponent<Rigidbody>().isKinematic = true;
            spawned.GetComponent<Rigidbody>().useGravity = false;
            canSelect = false;
        }

        if(editButton != null)
        {
            if (editButton.TryGetComponent(out upoctave_drum upoctavedrum))
            {
                Debug.Log("changing drum octave up");
                upoctavedrum.clicked = true;
            }
            else if (editButton.TryGetComponent(out downoctave_drum downoctavedrum))
            {
                Debug.Log("changing drum octave down");
                downoctavedrum.clicked = true;
            }
            else if (editButton.TryGetComponent(out downnote_drum downnotedrum))
            {
                Debug.Log("changing drum note down");
                downnotedrum.clicked = true;
            }
            else if (editButton.TryGetComponent(out upnote_drum upnotedrum))
            {
                Debug.Log("changing drum note up");
                upnotedrum.clicked = true;
            }
            else if (editButton.TryGetComponent(out next_drum nextdrum))
            {
                Debug.Log("changing drum sample to next");
                nextdrum.clicked = true;
            }
            else if (editButton.TryGetComponent(out prev_drum prevdrum))
            {
                Debug.Log("changing drum sample to previous");
                prevdrum.clicked = true;
            }
            else if (editButton.TryGetComponent(out upoctave upoctave))
            {
                Debug.Log("changing drum octave up");
                upoctave.clicked = true;
            }
            else if (editButton.TryGetComponent(out downoctave downoctave))
            {
                Debug.Log("changing drum octave down");
                downoctave.clicked = true;
            }
            else if (editButton.TryGetComponent(out downnote downnote))
            {
                Debug.Log("changing drum note down");
                downnote.clicked = true;
            }
            else if (editButton.TryGetComponent(out upnote upnote))
            {
                Debug.Log("changing drum note up");
                upnote.clicked = true;
            }
            else if (editButton.TryGetComponent(out next_instrument nextinstrument))
            {
                Debug.Log("changing drum sample to next");
                nextinstrument.clicked = true;
            }
            else if (editButton.TryGetComponent(out prev_instrument previnstrument))
            {
                Debug.Log("changing drum sample to previous");
                previnstrument.clicked = true;
            }

        }



        //Debug.Log("canEdit is " + canEdit + " at the end of SELECT");
    }

    void ToggleMode(InputAction.CallbackContext context)
    {
        if (controller.GetComponent<GraspGrabber>().canToggle)
        {
            if (laserPointer.enabled)
            {
                laserPointer.enabled = false;
                Debug.Log("laser point disabled");
                drumstick.SetActive(true);
                controller.SetActive(false);
            }
            else
            {
                laserPointer.enabled = true;
                Debug.Log("laser point enabled");
                drumstick.SetActive(false);
                controller.SetActive(true);
            }
        }
    }

}
