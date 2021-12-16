using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;
using System;

public class replaymanager : MonoBehaviour
{
    //Hold down Y/B buttons to record and then playback starts immediately
    public InputAction right_record;
    //public InputAction left_record = new InputAction(type: InputActionType.Button, binding: "<XRController>{LeftHand}/secondaryButton");
    public InputAction left_record;

    public GameObject drumstick;
    public GameObject right_drumstick;
    public GameObject right_child;
    public GameObject left_drumstick;
    public GameObject left_child;
    private GameObject right_clone;
    private GameObject left_clone;

    public Material recording_mat;
    public Material playing_mat;
    public Material default_mat;

    private bool right_recorded = false;
    private bool left_recorded = false;

    private List<Vector3> right_positions = new List<Vector3>();
    private List<Quaternion> right_rotations = new List<Quaternion>();
    private List<Vector3> left_positions = new List<Vector3>();
    private List<Quaternion> left_rotations = new List<Quaternion>();

    private Vector3[] right_play_positions;
    private Quaternion[] right_play_rotations;
    private Vector3[] left_play_positions;
    private Quaternion[] left_play_rotations;


    private bool right_recording = false;
    private bool right_playing = false;

    private bool left_recording = false;
    private bool left_playing = false;

    private int right_frame = 0;
    private int left_frame = 0;

    public int bpm = 120;
    private float onebar_in_seconds = 2.0f;
    private float onebeat_in_seconds = 0.5f;
    public float timestarted;

    // Start is called before the first frame update
    void Start()
    {
        right_record.Enable();
        left_record.Enable();
    }
    private void OnDestroy()
    {
        right_record.Disable();
        left_record.Disable();
    }

    // Update is called once per frame
    private void Update()
    {
        onebeat_in_seconds = 60f / (float)bpm;
        onebar_in_seconds = onebeat_in_seconds * 4;
        if (right_record.triggered && right_recording == false)
        {
            record_right();
            timestarted = Time.time;
        }else if (left_record.triggered && left_recording == false)
        {
            record_left();
            timestarted = Time.time;
        }
        if (Mathf.Floor(Time.time - timestarted) == onebar_in_seconds)
        {
            if (right_recording == true)
            {
                stop_right_and_loop();
            }
            if (left_recording == true)
            {
                stop_left_and_loop();
            }
        }
        if (right_recording)
        {
            right_positions.Add(right_drumstick.transform.position);
            right_rotations.Add(right_drumstick.transform.rotation);
        }
        if (left_recording)
        {
            left_positions.Add(left_drumstick.transform.position);
            left_rotations.Add(left_drumstick.transform.rotation);
        }
        if (right_playing)
        {
            if (right_frame >= right_play_positions.Length)
            {
                right_frame = 0;
            }
                right_clone.transform.position = right_play_positions[right_frame];
                right_clone.transform.rotation = right_play_rotations[right_frame];
                right_frame++;
        }
        if (left_playing)
        {
            if (left_frame >= left_play_positions.Length)
            {
                left_frame = 0;
            }
            left_clone.transform.position = left_play_positions[left_frame];
            left_clone.transform.rotation = left_play_rotations[left_frame];
            left_frame++;
        }
    }

    public void record_right()
    {
        //Debug.Log("Right started to record");
        if(right_drumstick.activeSelf == false)
        {

        }
        else
        {
            if (right_recorded == true)
            {
                right_positions.Clear();
                right_rotations.Clear();
                Destroy(right_clone);
                right_recorded = false;
            }
            right_recording = true;
            right_playing = false;
            right_child.GetComponent<Renderer>().material = recording_mat;
        }  
    }
    public void stop_right_and_loop()
    {
       //Debug.Log("Right stopped recording");
        if (right_drumstick.activeSelf == false)
        {

        }
        else
        {
            right_child.GetComponent<Renderer>().material = default_mat;
            right_clone = Instantiate(drumstick);
            right_play_positions = right_positions.ToArray();
            right_play_rotations = right_rotations.ToArray();
            right_recorded = true;
            right_recording = false;
            right_playing = true;
        }
    }
    public void record_left()
    {
        //Debug.Log("Left started to record");
        if (left_drumstick.activeSelf == false)
        {

        }
        else
        {
            if (left_recorded == true)
            {
                left_positions.Clear();
                left_rotations.Clear();
                Destroy(left_clone);
                left_recorded = false;
            }
            left_recording = true;
            left_playing = false;
            left_child.GetComponent<Renderer>().material = recording_mat;
        }
    }
    public void stop_left_and_loop()
    {
       //Debug.Log("Left stopped recording");
        if (left_drumstick.activeSelf == false)
        {

        }
        else
        {
            left_child.GetComponent<Renderer>().material = default_mat;
            left_clone = Instantiate(drumstick);
            left_play_positions = left_positions.ToArray();
            left_play_rotations = left_rotations.ToArray();
            left_recorded = true;
            left_recording = false;
            left_playing = true;
        }
    }
}
