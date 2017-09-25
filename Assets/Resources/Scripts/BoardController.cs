using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class BoardController : NetworkBehaviour
{

    [SyncVar]
    public float boardSizeWidth = 1;
    [SyncVar]
    public float boardSizeHeight = 1;
    public int obstacleCount = 20;
    [SyncVar]
    public int wallOffsetValue = 100;
    public float obstacleMinSize = 0.2f;
    public float obstacleMaxSize = 1.5f;


    public GameObject board;
    public GameObject wallSouth;
    public GameObject wallNorth;
    public GameObject wallWest;
    public GameObject wallEast;
    public GameObject obstaclePrefab;


    public void InitBoard()
    {
        float standardPlaneSize = 10;
        float wallSquareVerticallyCount = 3;
        float boardSizeX = standardPlaneSize * board.transform.localScale.x;
        float boardSizeZ = standardPlaneSize * board.transform.localScale.z;

        if (board && wallSouth && wallNorth && wallEast && wallWest)
        {
            if (boardSizeWidth >= 1)
            {
                wallSouth.transform.position += Vector3.right * (boardSizeX / 2 * (boardSizeWidth - 1));
                wallNorth.transform.position += Vector3.right * (-(boardSizeX / 2) * (boardSizeWidth - 1));
            }
            else
            {
                wallSouth.transform.position -= Vector3.right * (boardSizeX / 2 * (1 - boardSizeWidth));
                wallNorth.transform.position -= Vector3.right * (-(boardSizeX / 2) * (1 - boardSizeWidth));
            }
            if (boardSizeHeight >= 1)
            {
                wallWest.transform.position += Vector3.forward * (-(boardSizeZ / 2) * (boardSizeHeight - 1));
                wallEast.transform.position += Vector3.forward * (boardSizeZ / 2 * (boardSizeHeight - 1));
            }
            else
            {
                wallWest.transform.position -= Vector3.forward * (-(boardSizeZ / 2) * (1 - boardSizeHeight));
                wallEast.transform.position -= Vector3.forward * (boardSizeZ / 2 * (1 - boardSizeHeight));
            }
            board.transform.localScale = new Vector3(
                board.transform.localScale.x * boardSizeWidth, 
                board.transform.localScale.y, 
                board.transform.localScale.z * boardSizeHeight
                );

            wallSouth.transform.localScale = new Vector3(
                wallSouth.transform.localScale.x, 
                wallSouth.transform.localScale.y, 
                wallSouth.transform.localScale.z * boardSizeHeight
                );
            wallNorth.transform.localScale = new Vector3(
                wallNorth.transform.localScale.x,
                wallNorth.transform.localScale.y,
                wallNorth.transform.localScale.z * boardSizeHeight
                );
            wallWest.transform.localScale = new Vector3(
                wallWest.transform.localScale.x,
                wallWest.transform.localScale.y,
                wallWest.transform.localScale.z * boardSizeWidth
                );
            wallEast.transform.localScale = new Vector3(
                wallEast.transform.localScale.x,
                wallEast.transform.localScale.y,
                wallEast.transform.localScale.z * boardSizeWidth
                );

            Renderer floorTexture = board.GetComponent<Renderer>();
            
            Renderer wallSouthRend = wallSouth.GetComponent<MeshRenderer>();
            Renderer wallNorthRend = wallNorth.GetComponent<MeshRenderer>();
            Renderer wallEastRend = wallEast.GetComponent<MeshRenderer>();
            Renderer wallWestRend = wallWest.GetComponent<MeshRenderer>();

            floorTexture.material.mainTextureScale = new Vector2(boardSizeWidth * boardSizeZ / 2, boardSizeHeight * boardSizeX / 2);

            wallSouthRend.material.mainTextureScale = new Vector2(boardSizeHeight * boardSizeZ / 2, wallSquareVerticallyCount);
            wallNorthRend.material.mainTextureScale = new Vector2(boardSizeHeight * boardSizeZ / 2, wallSquareVerticallyCount);
            wallEastRend.material.mainTextureScale = new Vector2(boardSizeWidth * boardSizeX / 2, wallSquareVerticallyCount);
            wallWestRend.material.mainTextureScale = new Vector2(boardSizeWidth * boardSizeX / 2, wallSquareVerticallyCount);
        }

        if (isServer)
            CreateObstacles(board.transform.localScale.x * standardPlaneSize, board.transform.localScale.z * standardPlaneSize, standardPlaneSize);
    }

    void CreateObstacles(float boardSizeX, float boardSizeZ, float standardPlaneSize)
    {
        int defaultObstaclePositionY = 5;
        var listOfObstacles = new List<GameObject>();

        Vector3 collisionCheckerForward = transform.forward;
        Vector3 collisionCheckerBack = - transform.forward;
        Vector3 collisionCheckerLeft = - transform.right;
        Vector3 collisionCheckerRight = transform.right;

        var offsetX = boardSizeX / wallOffsetValue;
        var offsetZ = boardSizeZ / wallOffsetValue;

        float minX = -boardSizeX / 2 + offsetX;
        float maxX = boardSizeX / 2 - offsetX;
        float minZ = -boardSizeZ / 2 + offsetZ;
        float maxZ = boardSizeZ / 2 - offsetZ;


        for (int i = 0; i < obstacleCount; i++)
        {
            float obstacleSize = (float)Random.Range(obstacleMaxSize, obstacleMinSize);
            float obstaclePositionY = obstacleSize * defaultObstaclePositionY;
            while (true)
            {
                Vector3 obstaclePosition = new Vector3(Random.Range(minX, maxX), obstaclePositionY, Random.Range(minZ, maxZ));
                if (!Physics.Raycast(obstaclePosition, collisionCheckerForward, obstacleSize * standardPlaneSize)
                    && !Physics.Raycast(obstaclePosition, collisionCheckerBack, obstacleSize * standardPlaneSize) 
                    && !Physics.Raycast(obstaclePosition, collisionCheckerLeft, obstacleSize * standardPlaneSize) 
                    && !Physics.Raycast(obstaclePosition, collisionCheckerRight, obstacleSize * standardPlaneSize))
                {
                    listOfObstacles.Add(Instantiate(obstaclePrefab, obstaclePosition, Quaternion.identity) as GameObject);
                    if (isServer)
                    {
                        NetworkServer.Spawn(listOfObstacles[i]);
                    }
                    listOfObstacles[i].transform.localScale = new Vector3(obstacleSize, obstacleSize, obstacleSize);
                    break;
                }
            }
        }
    }
    void Start()
    {
        InitBoard();
    }
}
