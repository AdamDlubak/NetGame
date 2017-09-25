using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera mainCamera;
    public Camera spectatorCamera;
    public Camera leftCamera;
    public Camera rightCamera;
    public Camera backCamera;
    public GameObject player;
    public KeyCode lookLeft = KeyCode.Keypad4;
    public KeyCode lookRight = KeyCode.Keypad6;
    public KeyCode lookBack = KeyCode.Keypad8;

    private BikeLogic _bikeLogic;
    private GameMaster _gameMaster;
    private long _waitingCouter;
    private bool _previousCameraPlayMode = false;

    void Start()
    {
        mainCamera = Camera.main;
        _waitingCouter = 0;
        _gameMaster = GameObject.FindWithTag("GameMaster").GetComponent<GameMaster>();
        _bikeLogic = player.GetComponent<BikeLogic>();

        spectatorCamera = GameObject.FindWithTag("SpectatorCamera").GetComponent<Camera>();
        if (_gameMaster._cameraPlayMode && _bikeLogic._isAlive)
        {
            mainCamera.enabled = true;
            leftCamera.enabled = false;
            rightCamera.enabled = false;
            backCamera.enabled = false;
            spectatorCamera.enabled = false;
        }

    }
    void Update()
    {
        if (_previousCameraPlayMode != _gameMaster._cameraPlayMode)
        {
            DefaultCamera();
            _previousCameraPlayMode = _gameMaster._cameraPlayMode;
        }
        if (_gameMaster._cameraPlayMode && _bikeLogic._isAlive)
        {
            if (Input.GetKeyDown(lookLeft))
            {
                mainCamera.enabled = false;
                leftCamera.enabled = true;
                rightCamera.enabled = false;
                backCamera.enabled = false;
                spectatorCamera.enabled = false;
            }
            else if (Input.GetKeyDown(lookRight))
            {
                mainCamera.enabled = false;
                leftCamera.enabled = false;
                rightCamera.enabled = true;
                backCamera.enabled = false;
                spectatorCamera.enabled = false;
            }
            else if (Input.GetKeyDown(lookBack))
            {
                mainCamera.enabled = false;
                leftCamera.enabled = false;
                rightCamera.enabled = false;
                backCamera.enabled = true;
                spectatorCamera.enabled = false;
            }
            else if (Input.GetKeyUp(KeyCode.Keypad4) || Input.GetKeyUp(KeyCode.Keypad6) || Input.GetKeyUp(KeyCode.Keypad8)) DefaultCamera();
        }
        else
        {
            if (_waitingCouter >= 300)
            {
                mainCamera.enabled = false;
                leftCamera.enabled = false;
                rightCamera.enabled = false;
                backCamera.enabled = false;
                spectatorCamera.enabled = true;
                _waitingCouter = 0;
            }
            else _waitingCouter++;
        }
    }
    void DefaultCamera()
    {
        if (_gameMaster._cameraPlayMode && _bikeLogic._isAlive)
        {
            mainCamera.enabled = true;
            leftCamera.enabled = false;
            rightCamera.enabled = false;
            backCamera.enabled = false;
            spectatorCamera.enabled = false;
        }
        else {
            mainCamera.enabled = false;
            leftCamera.enabled = false;
            rightCamera.enabled = false;
            backCamera.enabled = false;
            spectatorCamera.enabled = true;
        }
    }
}