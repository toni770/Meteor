using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Collections.Generic;

public class NetworkManager_Custom : NetworkManager
{
    List<MatchInfoSnapshot> matchList = new List<MatchInfoSnapshot>();
    bool matchCreated;
    NetworkMatch networkMatch;

    void Awake()
    {
        networkMatch = gameObject.AddComponent<NetworkMatch>();
    }

    public void CrearSala()
    {
        string matchName = "room";
        uint matchSize = 4;
        bool matchAdvertise = true;
        string matchPassword = "";

        networkMatch.CreateMatch(matchName, matchSize, matchAdvertise, matchPassword, "", "", 0, 0, OnMatchCreated);
    }

    public void UnirSala()
    {
        networkMatch.ListMatches(0, 20, "", true, 0, 0, OnMatchListed);
    }

    public void Desconectar()
    {
        NetworkManager.singleton.StopHost();
    }

    void OnGUI()
    {
        // You would normally not join a match you created yourself but this is possible here for demonstration purposes.
        if (GUILayout.Button("Create Room"))
        {
            string matchName = "room";
            uint matchSize = 4;
            bool matchAdvertise = true;
            string matchPassword = "";

            networkMatch.CreateMatch(matchName, matchSize, matchAdvertise, matchPassword, "", "", 0, 0, OnMatchCreated);
        }

         if (GUILayout.Button("List rooms"))
        {
            networkMatch.ListMatches(0, 20, "", true, 0, 0, OnMatchListed);
        }

    }

    public void OnMatchCreated(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (success)
        {
            Debug.Log("Create match succeeded");
            matchCreated = true;
            NetworkServer.Listen(matchInfo, 9000);
            Utility.SetAccessTokenForNetwork(matchInfo.networkId, matchInfo.accessToken);
            NetworkManager.singleton.StartHost(matchInfo);
        }
        else
        {
            Debug.LogError("Create match failed: " + extendedInfo);
        }
    }

    public void OnMatchListed(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
    {
        if (success && matches != null && matches.Count > 0)
        {
            networkMatch.JoinMatch(matches[0].networkId, "", "", "", 0, 0, OnMatchJoineded);
        }
        else if (!success)
        {
            Debug.LogError("List match failed: " + extendedInfo);
        }
    }

    public void OnMatchJoineded(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (success)
        {
            Debug.Log("Join match succeeded");
            if (matchCreated)
            {
                Debug.LogWarning("Match already set up, aborting...");
                return;
            }
            Utility.SetAccessTokenForNetwork(matchInfo.networkId, matchInfo.accessToken);
            NetworkClient myClient = new NetworkClient();
            myClient.RegisterHandler(MsgType.Connect, OnConnected);
            myClient.Connect(matchInfo);
            NetworkManager.singleton.StartClient(matchInfo);
        }
        else
        {
            Debug.LogError("Join match failed " + extendedInfo);
        }
    }

    public void OnConnected(NetworkMessage msg)
    {
        Debug.Log("Connected!");
    }

    private void OnLevelWasLoaded(int level)
    {
        if(level ==0)
        {
            ConfigBotonesMenu();
        }
        else
        {
            ConfigBotonesJuego();
        }
    }

    void ConfigBotonesMenu()
    {

        GameObject.Find("Host").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("Host").GetComponent<Button>().onClick.AddListener(CrearSala);

        GameObject.Find("Join").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("Join").GetComponent<Button>().onClick.AddListener(UnirSala);
    }

    void ConfigBotonesJuego()
    {

        GameObject.Find("Disc").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("Disc").GetComponent<Button>().onClick.AddListener(Desconectar);
    }
}