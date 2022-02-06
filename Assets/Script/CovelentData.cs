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
using MoralisWeb3ApiSdk;
using System.Threading.Tasks;
using Moralis.Web3Api.Models;

public class CovelentData : MonoBehaviour
{
    // OTHER VARIABLES
    public GameObject CovDataHub;
    public GameObject CovDatalobby;
    public GameObject UIMaster;
    public PhotonView myView;
    public TMP_InputField datahubLobby;
    public TMP_InputField trasaction;
    public GameObject VoiceMaster;
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
    public List<GameObject> NFTimageSpawned = new List<GameObject>();

    
    private void Start()
    {
        addressField.text = "0x407D0E3BB4A87CCf78aAcDb5F1bb80214D147722";

        Refresh();
        Fetch();

        if (!myView.IsMine)
        {
            UIMaster.SetActive(false);
            VoiceMaster.SetActive(false);
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
        
            GetNftAsync();
    
       
        StartCoroutine(Data());

    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(1f);
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
     //   StartCoroutine(OnNFTfetch());

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
        trasaction.text = JToken.Parse(req.text).ToString(Formatting.Indented);


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


    public async void GetNftAsync()
    {

        foreach (GameObject image in NFTimageSpawned)
        {
            Destroy(image.gameObject);
        }
        NFTimageSpawned.Clear();
        NftCollection collection = await MoralisInterface.GetClient().Web3Api.Token.GetAllTokenIds(collectionAddress.text, ChainList.mumbai, null, 0);
        StartCoroutine(wait());
        StartCoroutine(lol(collection));
     /*   for (int tokenId = 0; tokenId < int.Parse(CollectionCount.text); tokenId++)
        {
           test(collection.Result[tokenId].TokenUri);
            
        } */
       
       
    }

    IEnumerator lol(NftCollection data)
    {
       for(int tokenId = 0; tokenId < int.Parse(CollectionCount.text); tokenId++)
        {
            test(data.Result[tokenId].TokenUri);
            yield return new WaitForSeconds(2);
        }
    }
    public void test(string file)
    {
        WWW request = new WWW(file);
        StartCoroutine(On(request));
    }
    IEnumerator On(WWW req)
    {
        yield return req;

        MoralisRoot tokenuri = JsonConvert.DeserializeObject<MoralisRoot>(req.text);
        Nfts(tokenuri.image,tokenuri.name,tokenuri.description);
    }





    public async void Nfts(string image, string name,string description)
    {
          //Performs another web request to collect the image related to the NFT
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(image))
        {
            //Sends webrequest
            await webRequest.SendWebRequest();
            //Gets the image from the web request and stores it as a texture
            Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);
            GameObject  Nftcard = Instantiate(ListPrefab, point.transform.position, Quaternion.identity);
            TMP_Text[] TokenText;
            TokenText = Nftcard.GetComponentsInChildren<TMP_Text>();

            TokenText[0].text = name;
            TokenText[1].text = description;

            Nftcard.transform.SetParent(point.transform);
            //Sets the objects main render material to the texture
            NFTimage.material.mainTexture = texture;

            NFTimageSpawned.Add(Nftcard);
        }
    }




    public void OpenPolygonScan()
    {

        Application.OpenURL("https://mumbai.polygonscan.com/token/" + collectionAddress.text);
    }
    /*
    async public void GetNFTImage()
    {
        for (int tokenId = 0; tokenId < 5; tokenId++)
        {


            //Interacts with the Blockchain to find the URI related to that specific token
            string URI = await ERC721.URI(chain, network, contract,tokenId.ToString());
            //   string response = await EVM.AllErc721(chain, network, account, contract);

            //Perform webrequest to get JSON file from URI
            using (UnityWebRequest webRequest = UnityWebRequest.Get(URI))
            {
                //Sends webrequest
                await webRequest.SendWebRequest();
                //Gets text from webrequest
                string metadataString = webRequest.downloadHandler.text;
                //Converts the metadata string to the Metadata object
                metadata = JsonConvert.DeserializeObject<Metadata>(metadataString);
                print(metadata.image);
            }
        }
            //Performs another web request to collect the image related to the NFT
          /*  using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(metadata.image))
            {
                //Sends webrequest
                await webRequest.SendWebRequest();
                //Gets the image from the web request and stores it as a texture
                Texture texture = DownloadHandlerTexture.GetContent(webRequest);
                //Sets the objects main render material to the texture
                GetComponent<MeshRenderer>().material.mainTexture = texture;
            } */
        

   /* }*/

    /* public void NFT()
     {
         StartCoroutine(OnNFTfetch());


     }*/
    /*  IEnumerator OnNFTfetch()
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

                 StartCoroutine(holdup(tokenId));



              }
          }

      }
      IEnumerator holdup(int tokenID)
      {
          yield return new WaitForSeconds(4f);
          Wait(tokenID);
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
          yield return new WaitForSeconds(20f);
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
   
