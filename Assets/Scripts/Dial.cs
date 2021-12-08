using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Dial : MonoBehaviour
{
    public float value = 0.5f;
    Quaternion initRot;
    int degreeLimit = 160;

    bool grabbedR = false;
    Quaternion ogHandRotR;
    float ogValue;
    bool grabbedL = false;
    Quaternion initHandRotL;

    public InputActionProperty rHandRot;

    // Start is called before the first frame update
    void Start()
    {
        initRot = this.transform.rotation;

        grabbedR = true;
        ogHandRotR = rHandRot.action.ReadValue<Quaternion>();
    }

    // Update is called once per frame
    void Update()
    {
        if (grabbedR)
        {
            Quaternion handQ = rHandRot.action.ReadValue<Quaternion>();
            Vector3 deltaVec3 = handQ.eulerAngles - ogHandRotR.eulerAngles;
            Vector3 normal = initRot * Vector3.up;
            Vector3 deltaProj = Vector3.ProjectOnPlane(deltaVec3, normal);
            value = (deltaProj.magnitude / (2f * degreeLimit)) + 0.5f;
        }
        this.transform.rotation = initRot * Quaternion.Euler(0, (value - 0.5f) * 2f * degreeLimit, 0) ;
        

    }
}
