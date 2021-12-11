using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drum_stick : MonoBehaviour
{
    Vector3 velocity;
    Vector3 previousPosition;
    public float v;

    // Start is called before the first frame update
    void Start()
    {
        velocity = Vector3.zero;
        previousPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Smooth out velocity 
        Vector3 newVelocity = (this.transform.position - previousPosition) / Time.deltaTime;
        velocity = .6f * velocity + .4f * newVelocity;
        previousPosition = this.transform.position;
        v = velocity.magnitude;
        //print("Velocity: " + v);
    }
    //private void OnCollisionEnter(Collider other)
    //{
    //    print("Velocity on impact: " + velocity.magnitude.ToString() + "\n");
    //    if (other.TryGetComponent(out drum_pad drumpad))
    //    {
    //        drumpad.clip_velocity = velocity.magnitude/10f;
    //        drumpad.hit = true;
    //    }
    //    else if (other.TryGetComponent(out oneshot_pad oneshot))
    //    {
    //        oneshot.clip_velocity = velocity.magnitude/10f;
    //        oneshot.hit = true;
    //    }
    //    else if (other.TryGetComponent(out upnote up))
    //    {
    //        up.clicked = true;
    //    }
    //    else if (other.TryGetComponent(out downnote down))
    //    {
    //        down.clicked = true;
    //    }
    //    else if (other.TryGetComponent(out downnote_drum down_d))
    //    {
    //        down_d.clicked = true;
    //    }
    //    else if (other.TryGetComponent(out upnote_drum up_d))
    //    {
    //        up_d.clicked = true;
    //    }
    //    else if (other.TryGetComponent(out upoctave up_o))
    //    {
    //        up_o.clicked = true;
    //    }
    //    else if (other.TryGetComponent(out downoctave down_o))
    //    {
    //        down_o.clicked = true;
    //    }
    //    else if (other.TryGetComponent(out downoctave_drum down_od))
    //    {
    //        down_od.clicked = true;
    //    }
    //    else if (other.TryGetComponent(out upoctave_drum up_od))
    //    {
    //        up_od.clicked = true;
    //    }
    //    else if(other.TryGetComponent(out next_drum next_drum))
    //    {
    //        next_drum.clicked = true;
    //    }
    //    else if (other.TryGetComponent(out prev_drum prev_drum))
    //    {
    //        prev_drum.clicked = true;
    //    }
    //    else if (other.TryGetComponent(out next_instrument next_i))
    //    {
    //        next_i.clicked = true;
    //    }
    //    else if (other.TryGetComponent(out prev_instrument prev_i))
    //    {
    //        prev_i.clicked = true;
    //    }
    //}
    //void OnCollisionExit(Collider otherObject)

    //{


    //}
}
