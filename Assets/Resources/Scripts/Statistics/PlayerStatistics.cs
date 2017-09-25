using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public struct PlayerStatisticsStruct
{
    public bool isLocalPlayer;
    public string nick;
    public bool isAlive;
    public float points;
}

public class PlayerStatistics : NetworkBehaviour
{
    [System.Serializable]
    public struct Parameters
    {
        public float pointsPerSecond;
        public GameObject statisticsGUIPrefab;
    }
    public Parameters parameters;
    public PlayerStatisticsStruct statistics;

    [SyncVar]
    public string nick;

    [SyncVar(hook="OnIsAliveUpdate")]
    bool _isAlive;
    [SyncVar(hook="OnPointsUpdate")]
    float _points;

    private GameObject _statisticsGUI;

    public override void OnStartLocalPlayer()
    {
        statistics = new PlayerStatisticsStruct();
        statistics.isLocalPlayer = true;
        CmdSetupStatistics(DataManager.instance.nick);
    }

    void OnIsAliveUpdate(bool isAlive)
    {
        statistics.isAlive = isAlive;
    }

    void OnPointsUpdate(float points)
    {
        statistics.points = points;
    }

    [ServerCallback]
    void Update()
    {
        float delta = Time.deltaTime * parameters.pointsPerSecond;
        if (_isAlive)
            _points += delta;
    }

    [Command]
    void CmdPlayerDied()
    {
        _isAlive = false;
    }
    
    [Command]
    void CmdSetupStatistics(string nick)
    {
        this.nick = nick;
        _isAlive = true;
        _points = 0.0f;
    }

#region PublicInterface

    public void PlayerDied()
    {
        if (!isLocalPlayer)
            return;

        if (_statisticsGUI != null)
        {
            Destroy(_statisticsGUI);
        }

        var prefab = parameters.statisticsGUIPrefab;
        _statisticsGUI = Instantiate<GameObject>(prefab, prefab.transform.position, prefab.transform.rotation);
        _statisticsGUI.GetComponent<StatisticsUI>().localPlayerStatistics = statistics;

        CmdPlayerDied();
    }

#endregion
}
