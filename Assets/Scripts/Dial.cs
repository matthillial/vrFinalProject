using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Audio;


public class Dial : MonoBehaviour
{
    Quaternion initRot;
    int degreeLimit = 160;

    float dialRot = 0;

    public bool grabbedR = false;
    public bool grabbedL = false;
    public bool moving = false;

    //public InputActionProperty rTrig;
    public GameObject rHand;
    public GameObject lHand;
    Vector3 previousTrackerPos;

    public AudioMixer mixer;
    public string parameterName;
    public float min;
    public float max;

    public bool miniature;
    public GameObject maxiature;
        

    // Start is called before the first frame update
    void Start()
    {
        initRot = this.transform.rotation.normalized;
        Vector3 normal = (initRot * Vector3.down).normalized;
        Debug.Log(normal);


    }

    // Update is called once per frame
    void Update()
    {

        if (grabbedR)
        {
            Vector3 trackerPos = rHand.transform.Find("Tracker").transform.position;

            float deltaAngle = getDeltaAngle(trackerPos, previousTrackerPos);

            dialRot += deltaAngle;

            previousTrackerPos = trackerPos;

        }
        if (grabbedL)
        {
            Vector3 trackerPos = lHand.transform.Find("Tracker").transform.position;

            float deltaAngle = getDeltaAngle(trackerPos, previousTrackerPos);

            dialRot += deltaAngle;

            previousTrackerPos = trackerPos;
        }
        if (miniature && maxiature != null)
        {
            dialRot = maxiature.GetComponentInChildren<Dial>().dialRot;
        }
        dialRot = Mathf.Max(dialRot, -degreeLimit);
        dialRot = Mathf.Min(dialRot, degreeLimit);
        //this.transform.rotation = initRot * Quaternion.Euler(0, (value - 0.5f) * 2f * degreeLimit, 0) ;

        //this.transform.localRotation =  initRot * Quaternion.Euler(0, dialRot, 0) ;
        this.transform.localRotation =  Quaternion.Euler(0, dialRot, 0);

        //if (!moving) { this.transform.rotation = initRot * this.transform.rotation; }
        mixer.SetFloat(parameterName, Mathf.Lerp(min, max, GetValue()));

    }

    public void Release(Quaternion newRot)
    {
        initRot = newRot;
        moving = false;
    }

    float GetValue()
    {
        return (dialRot / (degreeLimit / 2f)) + 0.5f;
    }

    public void GrabbedR()
    {
        //ogHandRotR = rHand.transform.rotation;
        Vector3 normal = (initRot * Vector3.down).normalized;
        //previousAngle = Vector3.Dot(rHand.transform.eulerAngles, normal);
        previousTrackerPos = rHand.transform.Find("Tracker").transform.position;
        grabbedR = true;
        grabbedL = false;
        //initDialRot = Vector3.Dot(ogHandRotR.eulerAngles, normal);
    }

    public void GrabbedL()
    {
        //ogHandRotR = rHand.transform.rotation;
        Vector3 normal = (initRot * Vector3.down).normalized;
        //previousAngle = Vector3.Dot(rHand.transform.eulerAngles, normal);
        previousTrackerPos = lHand.transform.Find("Tracker").transform.position;
        grabbedL = true;
        grabbedR = false;
        //initDialRot = Vector3.Dot(ogHandRotR.eulerAngles, normal);
    }

    public Dial initMaxiature(Transform hand)
    {
        GameObject dialHolder = gameObject.transform.parent.gameObject;
        if (maxiature != null)
        {
            Destroy(maxiature);
        }
        float oldDialRot = dialRot;
        maxiature = Instantiate(dialHolder, null);
        maxiature.transform.position = hand.transform.position + 0.1f * (hand.transform.rotation * Vector3.forward);
        
        Dial newDial = maxiature.GetComponentInChildren<Dial>();
        newDial.dialRot = oldDialRot;
        newDial.miniature = false;
        return newDial;
        //return maxiature;
    }


    float getDeltaAngle(Vector3 now, Vector3 last)
    {
        initRot = transform.parent.transform.rotation;
        Vector3 normal = (initRot * Vector3.up).normalized;
        Vector3 positionNoY = transform.position;
        positionNoY.y = 0;
        now = Vector3.ProjectOnPlane(now, normal);// + positionNoY;
        last = Vector3.ProjectOnPlane(last, normal);// + positionNoY;

        //rHand.transform.Find("Cube").transform.position = now;
        Vector3 yAxis = initRot * new Vector3(1, 0, 0);
        Vector3 xAxis = initRot * new Vector3(0, 0, -1);
        //Vector3 origin = new Vector3(0, 0, 0); // transform.position;
        Vector3 origin = Vector3.ProjectOnPlane(rHand.transform.position, normal);
        //rHand.transform.Find("OriginCube").transform.position = origin;

        Vector3 nowYProj = Vector3.Project(now, yAxis);
        nowYProj.x = origin.x;
        nowYProj.z = origin.z;
        //rHand.transform.Find("YCube").transform.position = nowYProj;
        float nowY = Vector3.Dot((nowYProj - origin), yAxis);
        float nowX = Vector3.Dot(now - nowYProj, xAxis);
        float nowAngle = Mathf.Atan2(nowY, nowX);

        Vector3 lastYProj = Vector3.Project(last, yAxis);
        lastYProj.x = origin.x;
        lastYProj.z = origin.z;
        float lastY = Vector3.Dot((lastYProj - origin), yAxis);
        float lastX = Vector3.Dot(last - lastYProj, xAxis);
        float lastAngle = Mathf.Atan2(lastY, lastX);

        return -Mathf.Rad2Deg * (nowAngle - lastAngle);
    }
}
