using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instrument_pad : MonoBehaviour
{
    //Gameobject where audio is played from
    public AudioSource audio_source;
    public AudioClip[] samples;
    public int sample_choice;


    //0-1 float - changes volume of played clip
    float clip_velocity = 1.0f;
    public int cur_note;
    string[] notes = new string[12] { "C", "C#" ,"D","D#","E","F","F#","G","G#","A","A#","B"};
    
    
    //array of materials colored from red to purple
    public Material[] note_materials;
    public Material[] notehit_materials;
    public Material cur_material;

    public bool hit = false;
    float playstart;

    public GameObject drumstick;
    public GameObject text;
    //Takes in a key from 0-71 and gives 
    public float KeyToPitch(int midiKey)
    {
        int c4Key = midiKey - 24;

        float pitch = Mathf.Pow(2, c4Key / 12f);

        return pitch;
    }
    // Start is called before the first frame update
    void Start()
    {
        audio_source = gameObject.GetComponent<AudioSource>();
        audio_source.clip = samples[sample_choice];
        //middle C
        cur_note = 24;

    }

    // Update is called once per frame
    void Update()
    {
        audio_source.clip = samples[sample_choice];
        text.GetComponent<UnityEngine.UI.Text>().text = notes[cur_note % 12];
        if (hit)
        {
            playstart = Time.time;
            cur_material = notehit_materials[cur_note % 12];
            this.gameObject.GetComponent<Renderer>().material = cur_material;
            audio_source.pitch = KeyToPitch(cur_note);
            audio_source.PlayOneShot(audio_source.clip,clip_velocity);
            hit = false;
        }
        if(Time.time - playstart > 0.25f)
        {
            cur_material = note_materials[cur_note % 12];
            this.gameObject.GetComponent<Renderer>().material = cur_material;
        }
    }
}
