using System.Collections;
using System.Collections.Generic;
using Photon.Voice.Unity;
using UnityEngine;

public class ForceEnable : MonoBehaviour
{
    public GameObject speaker;
    // Start is called before the first frame update
    void Start()
    {
        speaker.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(speaker.activeSelf == false)
        {
            speaker.SetActive(true);
        }

    }
}
