using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnLanternOnOff : MonoBehaviour
{
    public Light lantern;
    public bool on=false;
    public AudioSource soundOn;
    public AudioSource soundOff;
    // Start is called before the first frame update
    void Start()
    {
        lantern.enabled=false;
        soundOn.enabled = false;
        soundOff.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("f")){
            if(on){
                lantern.enabled=false;
                on=false;
                soundOff.enabled = true;
                soundOff.Play();
            }
            else {
                lantern.enabled= true;
                on=true;
                soundOn.enabled = true;
                soundOn.Play();
            }
        }
    }
}
