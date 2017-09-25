using UnityEngine;
using System.Collections;

public class CameraSpectator : MonoBehaviour
{

    public int speedCamera = 20;
    public Camera spectatorCamera;

    void Start()
    {
        spectatorCamera = GameObject.FindWithTag("SpectatorCamera").GetComponent<Camera>();
    }
    void Update()
    {
        speedCamera = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) ? 40 : 20;

        if (Input.GetKey(KeyCode.A)) transform.position = transform.position + spectatorCamera.transform.right * -1 * speedCamera * Time.deltaTime;
        
        if (Input.GetKey(KeyCode.S)) transform.position = transform.position + spectatorCamera.transform.forward * -1 * speedCamera * Time.deltaTime;

        if (Input.GetKey(KeyCode.D)) transform.position = transform.position + spectatorCamera.transform.right * speedCamera * Time.deltaTime;

        if (Input.GetKey(KeyCode.W)) transform.position = transform.position + spectatorCamera.transform.forward * speedCamera * Time.deltaTime;

        if (Input.GetKey(KeyCode.Q)) transform.position = transform.position + spectatorCamera.transform.up * speedCamera * Time.deltaTime;

        if (Input.GetKey(KeyCode.E)) transform.position = transform.position + spectatorCamera.transform.up * -1 * speedCamera * Time.deltaTime;
    }
}