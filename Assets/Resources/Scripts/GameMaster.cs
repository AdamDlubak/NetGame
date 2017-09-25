using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class GameMaster : NetworkBehaviour
{
    [SyncVar]
    private int _playersPresent = 0;

    [SyncVar]
    private bool _matchInProgress = false;

    [SyncVar]
    public bool _cameraPlayMode = false;
    public const int MAX_PLAYERS_IN_MATCH = 4;

    [ServerCallbackAttribute]
    void Update()
    {

        ServerUpdate();
    }

    void ServerUpdate()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            RpcGameStarted();
        }
    }

    [ClientRpc]
    public void RpcGameStarted()
    {
        Debug.Log("Game started RPC.");
        _cameraPlayMode = true;
        if (OnMatchStartedEvent != null)
            OnMatchStartedEvent();
    }

#region PublicInterface

    public int GetPresentPlayersCount()
    {
        return _playersPresent;
    }

    public bool IsMatchInProgress()
    {
        return _matchInProgress;
    }

    public static System.Action OnMatchStartedEvent;

#endregion
}
