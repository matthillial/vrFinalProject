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
    public GameObject selected;
    public Transform laserOrigin;
    public Transform spawn_point;
    public Material selectable_material;
    public Material default_material;
    public GameObject[] select_options;
    RaycastHit hit;


    // Start is called before the first frame update
    void Start()
    {
        toggle.action.performed += ToggleMode;
        select.action.performed += Select;
        laserPointer.enabled = false;
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
                    laserPointer.material = default_material;
                }
            }
            else
            {
                
            }
        }
        else
        {
  
        }
    }

    void Select(InputAction.CallbackContext context)
    {
        Debug.Log("select method");
        if (canSelect)
        {
            Debug.Log("INSTANTIATED METHOD");
            Instantiate(selected, spawn_point.position, Quaternion.identity);
            canSelect = false;
        }
    }

    void ToggleMode(InputAction.CallbackContext context)
    {
        if (laserPointer.enabled)
        {
            laserPointer.enabled = false;
            Debug.Log("laser point disabled");
        }
        else
        {
            laserPointer.enabled = true;
            Debug.Log("laser point enabled");
        }
        
    }

}
