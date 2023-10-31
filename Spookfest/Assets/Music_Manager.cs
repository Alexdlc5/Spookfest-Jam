using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music_Manager : MonoBehaviour
{
    private AudioSource[] tracks;
    public float[] tracklength;
    private float timer = -1;
    private bool tracknum = false;
    private void Start()
    {
        tracks = GetComponents<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        if (timer == -1)
        {
            //start music
            if (!tracknum)
            {
                tracks[0].Play();
                tracknum = true;
                timer = tracklength[0];
            }
            else
            {
                tracks[1].Play();
                tracknum = false;
                timer = tracklength[1];
            }
        }
        else if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else 
        {
            timer = -1;

        }
    }
}
