using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRotator : MonoBehaviour
{

    Renderer _renderer;
    Rigidbody _rigidBody;

    float _speed;
    public float speedMultiplier;

    void Start()
    {
        _rigidBody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        _speed = _rigidBody.velocity.magnitude;
        transform.RotateAround(_renderer.bounds.center, transform.right, _speed * speedMultiplier * Time.deltaTime);
    }
}
