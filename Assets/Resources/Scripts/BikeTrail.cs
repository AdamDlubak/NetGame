using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BikeTrail : NetworkBehaviour
{
    [System.SerializableAttribute]
    public struct Parameters
    {
        public float minDistance;

        public Transform spawnPoint;
        public GameObject trailPiecePrefab;

        public float forwardScaleModifier;
    }

    public Parameters parameters;

    private Vector3 _lastSpawnPoint;

    void Start()
    {
        _lastSpawnPoint = parameters.spawnPoint.position;
    }

    [Client]
    void Update()
    {
        float distance = Vector3.Distance(parameters.spawnPoint.position, _lastSpawnPoint);

        if (distance > parameters.minDistance)
        {
            CmdSpawn();
        }
    }

    [Command]
    private void CmdSpawn()
    {
        GameObject newTrailPiece = Instantiate(parameters.trailPiecePrefab, parameters.spawnPoint.position, parameters.spawnPoint.rotation);

        Vector3 lastSpawnPoint = _lastSpawnPoint;
        Vector3 newSpawnPoint = parameters.spawnPoint.position;

        _lastSpawnPoint = newSpawnPoint;
        NetworkServer.Spawn(newTrailPiece);
        RpcPositionFragmentBetween(newTrailPiece, lastSpawnPoint, newSpawnPoint);
    }

    [ClientRpc]
    private void RpcPositionFragmentBetween(GameObject trailInstance, Vector3 lastPos, Vector3 newPos)
    {
        Transform fragment = trailInstance.transform;
        
        fragment.position = lastPos;

        fragment.LookAt(newPos, Vector3.up);

        fragment.position += (newPos - lastPos) * 0.5f;

        float distance = (newPos - lastPos).magnitude;

        fragment.localScale = new Vector3(fragment.localScale.x, fragment.localScale.y, parameters.forwardScaleModifier * distance);
    }
}