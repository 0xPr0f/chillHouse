using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class billboard : MonoBehaviour
{
	Camera cam;
	public TMP_Text username;
	public GameObject UsernameText;
	public PhotonView view;

    private void Start()
    {
        if (view.IsMine)
        {
			UsernameText.SetActive(false);
        }
		username.text = PlayerPrefs.GetString("Nickname");
    }

    void Update()
	{
		if (cam == null)
			cam = FindObjectOfType<Camera>();

		if (cam == null)
			return;

		transform.LookAt(cam.transform);
		transform.Rotate(Vector3.up * 180);
	}
}