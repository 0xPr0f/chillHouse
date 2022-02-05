using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    public Animator transitionAnim;
    public TMP_InputField usernameInput;
    public TMP_Text buttonText;

   public void onClickConnect()
    {
        if(usernameInput.text.Length >= 2)
        {
            PhotonNetwork.NickName = usernameInput.text;
            buttonText.text = "Connecting...";
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        StartCoroutine(LoadScene());
        // scene trasition here
    }

    IEnumerator LoadScene()
    {
        transitionAnim.SetTrigger("end");
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene("Lobby");
    }
}
