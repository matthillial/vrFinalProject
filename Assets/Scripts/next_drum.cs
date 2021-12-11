using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class next_drum : MonoBehaviour
{
    public GameObject pad;
    public GameObject arrow;
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
            arrow.GetComponent<Renderer>().material = hit;
            if ((pad.GetComponent<drum_pad>().samples.Length-1) == pad.GetComponent<drum_pad>().sample_choice)
            {
                //Loop back to first drum choice
                pad.GetComponent<drum_pad>().sample_choice = 0;
                clicked = false;
            }
            else
            {
                pad.GetComponent<drum_pad>().sample_choice = pad.GetComponent<drum_pad>().sample_choice + 1;
                clicked = false;
            }
        }
        if(Time.time - hit_time > 0.2f)
        {
            arrow.GetComponent<Renderer>().material = regular;
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
