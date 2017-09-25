using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformMatchVelocityOrientation : MonoBehaviour
{
    public Rigidbody rigidbody;

    void FixedUpdate ()
    {
        Vector3 forward = rigidbody.velocity.normalized;
        transform.LookAt(transform.position + forward, Vector3.up);
    }
}
