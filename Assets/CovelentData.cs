using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using System;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.UI;

public class CovelentData : MonoBehaviour
{
    // OTHER VARIABLES
    public GameObject CovDataHub;
    public GameObject CovDatalobby;
    public GameObject UIMaster;
    public PhotonView myView;
    public TMP_InputField datahubLobby;
    public TMP_InputField trasaction;

    public TMP_InputField addressField;
    bool isactiveHUB = false;
    bool isactiveLobby = false;
    private string LobbyURL = "https://api.covalenthq.com/v1/80001/tokens/0x407D0E3BB4A87CCf78aAcDb5F1bb80214D147722/nft_token_ids/?&key=ckey_e22bfa8a9c734c1e816244b1529";
    private string TrasactionURL = "https://api.covalenthq.com/v1/80001/address/0x407D0E3BB4A87CCf78aAcDb5F1bb80214D147722/transactions_v2/?quote-currency=USD&format=JSON&block-signed-at-asc=false&no-logs=false&key=ckey_e22bfa8a9c734c1e816244b1529";


    /*------NFTDATA COLLECTION------------------*/

    public TMP_Text CollectionName;
    public TMP_Text CollectionSymbol;
    public TMP_Text CollectionCount;
    public TMP_Text RefreshDate;
    public TMP_Text collectionAddress;

   // public MeshRenderer NFTprofile;
   public  GameObject ListPrefab;
    public GameObject point;
    public Image NFTimage;

    private void Start()
    {
        addressField.text = "0x407D0E3BB4A87CCf78aAcDb5F1bb80214D147722";

        Refresh();
        Fetch();

        if (!myView.IsMine)
        {
            UIMaster.SetActive(false);
        }
        CovDataHub.SetActive(false);
        CovDatalobby.SetActive(false);
    }

    public void Fetch()
    {

        if (addressField.text.Length > 40)
        {
            
            CovData();
            CovDataCount();      
            TrasactionURL = "https://api.covalenthq.com/v1/80001/address/" + addressField.text + "/transactions_v2/?quote-currency=USD&format=JSON&block-signed-at-asc=false&no-logs=false&key=ckey_e22bfa8a9c734c1e816244b1529";
            GetTrasactionDAta();
           // StartCoroutine(OnNFTfetch());


        }


    }


    private void CovData()
    {

        StartCoroutine(Data());

    }

    IEnumerator Data()
    {
        var url = "https://api.covalenthq.com/v1/80001/tokens/" + addressField.text + "/nft_metadata/1/?quote-currency=USD&format=JSON&key=ckey_e22bfa8a9c734c1e816244b1529";


        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        StreamReader reader = new StreamReader(response.GetResponseStream());
        yield return reader;
        string json = reader.ReadToEnd();
        Rootcov data = JsonConvert.DeserializeObject<Rootcov>(json);


        CollectionName.text = data.data.items[0].contract_name;
        CollectionSymbol.text = data.data.items[0].contract_ticker_symbol;
        collectionAddress.text = data.data.items[0].contract_address;
        RefreshDate.text = data.data.updated_at;
    }



    public void Refresh()
    {
        WWW request = new WWW(LobbyURL);
        StartCoroutine(OnResponse(request));
    }
    IEnumerator OnResponse(WWW req)
    {
        yield return req;


        datahubLobby.text = JToken.Parse(req.text).ToString(Newtonsoft.Json.Formatting.Indented);

    }


    private void CovDataCount()
    {
        var url = "https://api.covalenthq.com/v1/80001/tokens/" + addressField.text + "/nft_token_ids/?&key=ckey_e22bfa8a9c734c1e816244b1529";

        StartCoroutine(DataCount(url));
        StartCoroutine(OnNFTfetch());

    }

    IEnumerator DataCount(string url)
    {


        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        StreamReader reader = new StreamReader(response.GetResponseStream());
        yield return reader;
        string json = reader.ReadToEnd();
        Rootcov data = JsonConvert.DeserializeObject<Rootcov>(json);
        int totalNFT = data.data.pagination.total_count;
        CollectionCount.text = totalNFT.ToString();

    }


    public void OpenDataHub()
    {
        if (isactiveHUB == false)
        {
            CovDataHub.SetActive(true);
            isactiveHUB = true;
        }

        else if (isactiveHUB == true)
        {
            CovDataHub.SetActive(false);
            isactiveHUB = false;
        }
    }


    // GET TRANSACTION DATA
    public void GetTrasactionDAta()
    {
        WWW request = new WWW(TrasactionURL);
        StartCoroutine(OnTrasactions(request));
    }
    IEnumerator OnTrasactions(WWW req)
    {
        yield return req;
        trasaction.text = JToken.Parse(req.text).ToString(Newtonsoft.Json.Formatting.Indented);


    }



    public void OpenDataLobby()
    {
        if (isactiveLobby == false)
        {
            CovDatalobby.SetActive(true);
            isactiveLobby = true;
        }

        else if (isactiveLobby == true)
        {
            CovDatalobby.SetActive(false);
            isactiveLobby = false;
        }
    }

    public void OpenPolygonScan()
    {

        Application.OpenURL("https://mumbai.polygonscan.com/token/" + collectionAddress.text);
    }

   /* public void NFT()
    {
        StartCoroutine(OnNFTfetch());


    }*/
    IEnumerator OnNFTfetch()
    {

        yield return new WaitForSeconds(10f);
        if (int.Parse(CollectionCount.text) < 1)
        {
            Debug.Log("lol");
        }
        else if (int.Parse(CollectionCount.text) >= 1)
        {
            for (int tokenId = 0; tokenId < int.Parse(CollectionCount.text) + 1; tokenId++)
            {

                StartCoroutine(holdup());
                Wait(tokenId);
                

            }
        }

    }
    IEnumerator holdup()
    {
        yield return new WaitForSeconds(4f);
    }
    public void Wait(int tid)
    {
       StartCoroutine(chill(tid));
       

    }

    IEnumerator chill(int tokenId)
    {
        yield return new WaitForSeconds(3f);
        var NFTurl = "https://api.covalenthq.com/v1/80001/tokens/" + collectionAddress.text + "/nft_metadata/" + tokenId + "/?quote-currency=USD&format=JSON&key=ckey_e22bfa8a9c734c1e816244b1529";
      
      StartCoroutine(OnImage(NFTurl));

    }

    IEnumerator OnImage(string url)
    {

    
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        StreamReader reader = new StreamReader(response.GetResponseStream());
        yield return reader;
        string json = reader.ReadToEnd();
        Root data = JsonConvert.DeserializeObject<Root>(json);
       // NFTImage(data.data.items[0].nft_data[0].external_data.image_256);
        print(data.data.items[0].nft_data[0].external_data.image_256);

    }
    /* async private void NFTImage(string image)
        {
            using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(image))
            {
                //Sends webrequest
                await webRequest.SendWebRequest();
                //Gets the image from the web request and stores it as a texture
                Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);
            //Sets the objects main render material to the texture
            StartCoroutine(PrintNFT(texture));
           // PrintNFT(texture);
            // NFTprofile.material.mainTexture = texture;
            
            }
        }


    IEnumerator PrintNFT(Texture2D texture2D)
    {
        print("is it reaching here ?");
        GameObject newObject =  Instantiate(ListPrefab,point.transform.position, Quaternion.identity);
        newObject.transform.SetParent(point.transform);
        yield return new WaitForSeconds(5f);
        NFTimage.material.mainTexture = texture2D; */

   /* } */
}
   
