using System.Collections;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;
using Newtonsoft.Json.Linq;

public class Mint
{
    public string chain;
    public string name;
    public string description;
    public string file_url;
    public string mint_to_address;

}

public class NFTPort : MonoBehaviour
{

    public TMP_InputField NFTname;
    public TMP_InputField NFTdescription;
    public TMP_InputField NFTaddress;
    public TMP_InputField NFTlink;
    public GameObject Nftlogport;
    bool isNFTlogActive;
    public TMP_InputField outputArea;
    private const string NFTPORTAPI = "https://api.nftport.xyz/v0/mints/easy/urls";
    private const string NFTPORTKEY = "fce489e1-5703-4637-8c6d-4b0d1d1431ea";



    public void OpenNFTportLogs()
    {
        if (isNFTlogActive == false)
        {
            Nftlogport.SetActive(true);
            isNFTlogActive = true;
        }

        else if (isNFTlogActive == true)
        {
            Nftlogport.SetActive(false);
            isNFTlogActive = false;
        }
    }


    public void MintNFT()
    {
        if (NFTdescription.text.Length > 5 && NFTname.text.Length > 2 && NFTaddress.text.Length > 40 && NFTlink.text.Length > 8)
        {            
            StartCoroutine(OnMint());
        }
        else
        {
            outputArea.text = "The fields did not meet the required amount";
        }
    }

    IEnumerator OnMint()
    {
        outputArea.text = "Loading...";
        string url = NFTPORTAPI;
        var uwr = new UnityWebRequest(url, "POST");

        // Set the values for the NFT you want to mint
        Mint nft = new Mint();
        nft.chain = "polygon";
        nft.name = NFTname.text;
        nft.description = NFTdescription.text;
        nft.file_url = NFTlink.text;
        nft.mint_to_address = NFTaddress.text;

        string json = JsonUtility.ToJson(nft);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);

        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        //Set headers for the request
        uwr.SetRequestHeader("Content-Type", "application/json");
        uwr.SetRequestHeader("Authorization", NFTPORTKEY);

        //Makes request
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
            outputArea.text = uwr.error;
        else
            outputArea.text = 
        JToken.Parse(uwr.downloadHandler.text).ToString(Newtonsoft.Json.Formatting.Indented);
    }
}


