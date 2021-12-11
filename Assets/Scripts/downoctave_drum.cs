using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class downoctave_drum : MonoBehaviour
{
    public GameObject pad;
    public GameObject arrow1;
    public GameObject arrow2;
    public Material hit;
    public Material regular;

    float hit_time;

    public bool clicked;
    // Start is called before the first frame update
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        if (clicked)
        {
            hit_time = Time.time;
            arrow1.GetComponent<Renderer>().material = hit;
            arrow2.GetComponent<Renderer>().material = hit;
            if (pad.GetComponent<drum_pad>().cur_note <= 11)
            {
                //Ideally this would pop up as a tooltip by controller or pad
                print("Note is too low\n");
                clicked = false;
            }
            else
            {
                pad.GetComponent<drum_pad>().cur_note = pad.GetComponent<drum_pad>().cur_note - 12;
                clicked = false;
            }
        }
        if(Time.time - hit_time > 0.2f)
        {
            arrow1.GetComponent<Renderer>().material = regular;
            arrow2.GetComponent<Renderer>().material = regular;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.TryGetComponent(out drum_stick end))
        {
            clicked = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {

    }
}
