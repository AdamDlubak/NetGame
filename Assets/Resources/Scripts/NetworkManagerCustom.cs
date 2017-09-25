using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkManagerCustom : NetworkManager
{

    public InputField hostAddress;
    public int customNetworkPort = 7777;
    public Button hostButton;
    public Button connectButton;
    public static event System.Action OnEscapeMenuOpened;

    public void StartUpHost()
    {
        SetPort();
        NetworkManager.singleton.StartHost();
    }

    public void JoinGame()
    {
        SetHostAddress();
        SetPort();
        NetworkManager.singleton.StartClient();
    }

    void SetHostAddress()
    {
        string hostAddressInputValue = GameObject.Find("AdressTextInput").GetComponent<InputField>().text;
        if (hostAddressInputValue != "")
            NetworkManager.singleton.networkAddress = hostAddressInputValue;
        else
            NetworkManager.singleton.networkAddress = "localhost";
    }

    void SetPort()
    {
        NetworkManager.singleton.networkPort = customNetworkPort;
    }

    private void OnEnable()
    {
        EscapeMenuMonitor.OnEscapeMenuOpened += SetupOtherSceneButtons;
        ActivateMenuMulti.OnMenuMultiActivated += SetupMenuSceneButtons;
    }

    private void OnDisable()
    {
        EscapeMenuMonitor.OnEscapeMenuOpened -= SetupOtherSceneButtons;
        ActivateMenuMulti.OnMenuMultiActivated -= SetupMenuSceneButtons;
    }

    void SetupMenuSceneButtons()
    {
        //hostButton.onClick.RemoveAllListeners();
        //hostButton.onClick.AddListener(StartUpHost);

        //connectButton.onClick.RemoveAllListeners();
        //connectButton.onClick.AddListener(JoinGame);

        GameObject.Find("ConnectButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("ConnectButton").GetComponent<Button>().onClick.AddListener(JoinGame);

        GameObject.Find("HostButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("HostButton").GetComponent<Button>().onClick.AddListener(StartUpHost);
    }

    void SetupOtherSceneButtons()
    {
        GameObject.Find("ConfirmButton").GetComponent<Button>().onClick.RemoveAllListeners();

        if (!Network.isClient)
        {
            print("Server");
            GameObject.Find("ConfirmButton").GetComponent<Button>().onClick.AddListener(NetworkManager.singleton.StopHost);
        }
        else if (!Network.isClient)
        {
            print("Client");
            GameObject.Find("ConfirmButton").GetComponent<Button>().onClick.AddListener(NetworkManager.singleton.StopHost);
        }
        else
        {
            print("Something disconnection menu");
            GameObject.Find("ConfirmButton").GetComponent<Button>().onClick.AddListener(NetworkManager.singleton.StopHost);
        }
    }

    public void DisconnectGame()
    {
        NetworkManager.singleton.StopHost();
        NetworkManager.singleton.StopClient();
    }
}
