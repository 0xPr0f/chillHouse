/**
 *           Module: MainMenuScript.cs
 *  Descriptiontion: Example class that demonstrates a game menu that incorporates
 *                   Wallet Connect and Moralis Authentication.
 *           Author: Moralis Web3 Technology AB, 559307-5988 - David B. Goodrich 
 *  
 *  MIT License
 *  
 *  Copyright (c) 2021 Moralis Web3 Technology AB, 559307-5988
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the "Software"), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in all
 *  copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 *  SOFTWARE.
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WalletConnectSharp.Core.Models;
using WalletConnectSharp.Unity;
using Assets.Scripts;
using Assets;
using MoralisWeb3ApiSdk;

#if UNITY_WEBGL
using Cysharp.Threading.Tasks;
using Moralis.WebGL.Platform;
using Moralis.WebGL.Platform.Objects;
#else
using System.Threading.Tasks;
using Moralis.Platform;
using Moralis.Platform.Objects;
#endif

/// <summary>
/// Example class that demonstrates a game menu that incorporates Wallet 
/// Connect and Moralis Authentication.
/// </summary>
public class MainMenuScript : MonoBehaviour
{
    public MoralisController moralisController;
    public GameObject authenticationButton;
    public GameObject logoutButton;
    public WalletConnect walletConnect;
    public GameObject qrMenu;
    public GameObject signing;
    Image menuBackground;

    /// <summary>
    /// Unity method - called at application start up.
    /// </summary>
    async void Start()
    {
        signing.SetActive(false);
        menuBackground = (Image)gameObject.GetComponent(typeof(Image));

        qrMenu.SetActive(false);
      //  androidMenu.SetActive(false);
       // iosMenu.SetActive(false);

        // await MoralisInterface.Initialize(MoralisApplicationId, MoralisServerURI, hostManifestData);
        if (moralisController != null && moralisController)
        {
            await moralisController.Initialize();
        }
        else
        {
            // Moralis values not set or initialized.
            Debug.LogError("The MoralisInterface has not been set up, please check you MoralisController in the scene.");
        }

        // If user is not logged in show the "Authenticate" button.
        if (!MoralisInterface.IsLoggedIn())
        {
            AuthenticationButtonOn();
        }
        else
        {
            LogoutButtonOn();
        }
    }

    /// <summary>
    /// Used to start the authentication process and then run the game. If the 
    /// user has a valid Moralis session thes user is redirected to the next 
    /// scene.
    /// </summary>
    public async void Play()
    {
        AuthenticationButtonOff();

        // If the user is still logged in just show game.
        if (MoralisInterface.IsLoggedIn())
        {
            Debug.Log("User is already logged in to Moralis.");
        }
        // User is not logged in, depending on build target, begin wallect connection.
        else
        {
            Debug.Log("User is not logged in.");
            //mainMenu.SetActive(false);

            // The mobile solutions for iOS and Android will be different once we
            // smooth out the interaction with Wallet Connect. For now the duplicated 
            // code below is on purpose just to keep the iOS and Android authentication
            // processes separate.
#if UNITY_ANDROID
            // By pass noraml Wallet Connect for now.
            androidMenu.SetActive(true);

            // Use Moralis Connect page for authentication as we work to make the Wallet 
            // Connect experience better.
            //await LoginViaConnectionPage();
#elif UNITY_IOS
            // By pass noraml Wallet Connect for now.
            iosMenu.SetActive(true);

            // Use Moralis Connect page for authentication as we work to make the Wallet 
            // Connect experience better.
            //await LoginViaConnectionPage();
#else
            qrMenu.SetActive(true);
#endif
        }
    }

    /// <summary>
    /// Handles the Wallet Connect OnConnection event.
    /// When user grants wallet connection to application this 
    /// method is called. A request for signature is sent to wallet. 
    /// If users agrees to sign the message the signed message is 
    /// used to authenticate with Moralis.
    /// </summary>
    /// <param name="data">WCSessionData</param>
    public async void WalletConnectHandler(WCSessionData data)
    {
        Debug.Log("Wallet connection received");
        // Extract wallet address from the Wallet Connect Session data object.
        string address = data.accounts[0].ToLower();

        qrMenu.SetActive(false);

        Debug.Log($"Sending sign request for {address} ...");
        signing.SetActive(true);
        string response = await walletConnect.Session.EthPersonalSign(address, "Moralis Authentication");

        Debug.Log($"Signature {response} for {address} was returned.");
        PlayerPrefs.SetString("Address", address);
        // Create moralis auth data from message signing response.
        Dictionary<string, object> authData = new Dictionary<string, object> { { "id", address }, { "signature", response }, { "data", "Moralis Authentication" } };

        Debug.Log("Logging in user.");

        // Attempt to login user.
        MoralisUser user = await MoralisInterface.LogInAsync(authData);

        if (user != null)
        {
            Debug.Log($"User {user.username} logged in successfully. ");
        }
        else
        {
            Debug.Log("User login failed.");
        }

        HideWalletSelection();

        // TODO: For your own app you may want to move / remove this.
        LogoutButtonOn();
    }

    /// <summary>
    /// Closeout connections and quit the application.
    /// </summary>
    public async void Quit()
    {
        Debug.Log("QUIT");

        // Disconnect wallet subscription.
        await walletConnect.Session.Disconnect();
        // CLear out the session so it is re-establish on sign-in.
        walletConnect.CLearSession();
        // Logout the Moralis User.
        await MoralisInterface.LogOutAsync();
        // Close out the application.
        Application.Quit();
    }

    public void HideWalletSelection()
    {
#if UNITY_ANDROID
        androidMenu.SetActive(false);
#elif UNITY_IOS
        iosMenu.SetActive(false);
#endif
    }

    /// <summary>
    /// Display Moralis connector login page
    /// </summary>
#if UNITY_WEBGL
    private async UniTask LoginViaConnectionPage()
    {
        // Use Moralis Connect page for authentication as we work to make the Wallet 
        // Connect experience better.
        MoralisUser user = await MobileLogin.LogIn(moralisController.MoralisServerURI, moralisController.MoralisApplicationId);

        if (user != null)
        {
            // User is not null so login was successful, show first game scene.
            //SceneManager.LoadScene(SceneMap.GAME_VIEW);
            AuthenticationButtonOff();
        }
        else
        {
            AuthenticationButtonOn();
        }
    }
#else
    private async Task LoginViaConnectionPage()
    {
        // Use Moralis Connect page for authentication as we work to make the Wallet 
        // Connect experience better.
        MoralisUser user = await MobileLogin.LogIn(moralisController.MoralisServerURI, moralisController.MoralisApplicationId);

        if (user != null)
        {
            // User is not null so login was successful, show first game scene.
            //SceneManager.LoadScene(SceneMap.GAME_VIEW);
            AuthenticationButtonOff();
        }
        else
        {
            AuthenticationButtonOn();
        }
    }
#endif
    /// <summary>
    /// Hide the authentiucation button and blackout screen.
    /// </summary>
    private void AuthenticationButtonOff()
    {
        authenticationButton.SetActive(false);
        Color color = menuBackground.color;
        color = new Color(0f, 0f, 0f, 0f);
        menuBackground.color = color;
    }

    /// <summary>
    /// Show the Authentication button superimposed on the blackout screen.
    /// </summary>
    private void AuthenticationButtonOn()
    {
        LogoutButtonOff();
        authenticationButton.SetActive(true);
        Color color = menuBackground.color;
        color = new Color(0f, 0f, 0f, 0.7f);
        menuBackground.color = color;
    }

    /// <summary>
    /// Hide the logof button
    /// </summary>
    private void LogoutButtonOff()
    {
        logoutButton.SetActive(false);
    }

    /// <summary>
    /// Display the logout button.
    /// </summary>
    private void LogoutButtonOn()
    {
        logoutButton.SetActive(true);
    }
   
    private async void OnApplicationQuit()
    {

        // Disconnect wallet subscription.
        await walletConnect.Session.Disconnect();
        // CLear out the session so it is re-establish on sign-in.
        walletConnect.CLearSession();
        // Logout the Moralis User.
        await MoralisInterface.LogOutAsync();
    }
}
