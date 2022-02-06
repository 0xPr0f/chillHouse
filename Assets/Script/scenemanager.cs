using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class scenemanager : MonoBehaviour
{
    public GameObject AnimatedScene;
    public GameObject Logout;
    public GameObject[] Object;
    public GameObject[] nottobefalse;
    Scene scene;
    bool isActive = false;
    bool isLogoutActive;

    private void Start()
    {
        scene = SceneManager.GetActiveScene();
      
        foreach (Object objects in Object)
        {
           
            DontDestroyOnLoad(objects);
            
        }
        foreach (Object objecs in nottobefalse)
        {

            DontDestroyOnLoad(objecs);

        }
        StartCoroutine(TrasitionChange());
        
      
    }

   IEnumerator TrasitionChange()
    {
        yield return new WaitForSeconds(2f);
        AnimatedScene.SetActive(false);
    }

    private void Update()
    {
        isLogoutActive = Logout.activeSelf;
        if (isActive == false && isLogoutActive == true)
        {         
            SceneManager.LoadScene(scene.buildIndex + 1);
            isActive = true;
            foreach(GameObject objects in Object)
            {
                objects.SetActive(false);
            }
        }
    }
   
}
