using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickable : MonoBehaviour
{
    public AudioSource audioSource;
    bool isOpen;
    Transform doorTransform;
    

    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
        doorTransform = GetComponent<Transform>();
    }

    private void OnMouseDown()
    {
        if(isOpen == false)
        {
            audioSource.Play();
            doorTransform.rotation = Quaternion.Euler(0, 90,0);
            isOpen = true;
        }
       else if (isOpen == true)
        {
            audioSource.Play();
            doorTransform.rotation = Quaternion.Euler(0, 0, 0);
            isOpen = false;
        } 
        //audioSource.PlayOneShot(audio);
        Debug.Log("clicked");
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
