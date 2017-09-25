using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class RigidbodyFollow : MonoBehaviour
{
    public Rigidbody target = null;

    public float forwardLookOffset;
    public float distance;
    public float height;
    public float lerp;
    public float rotationLerp;

    public AnimationCurve angleToRotation;
    public float angleToRotationMultiplier;

    void FixedUpdate()
    {
        if (target == null)
            return;
        
        Vector3 fv = target.velocity.normalized;

        Vector3 toTarget = target.position + (fv * forwardLookOffset) - transform.position;

        Vector3 newPos = target.position - (fv * distance) + (Vector3.up * height);

        Quaternion look = Quaternion.LookRotation(toTarget);

        float angle = Vector3.Angle(toTarget, transform.forward);
        float rot = angleToRotation.Evaluate(angle) * angleToRotationMultiplier;

        if (Vector3.Cross(toTarget, transform.forward).y < 0)
            rot = -rot;

        Quaternion angleRotation = Quaternion.Euler(0, 0, rot);

        transform.rotation = Quaternion.Lerp(transform.rotation, look, rotationLerp);

        transform.localRotation *= angleRotation;

        transform.position = Vector3.Lerp(transform.position, newPos, lerp);
    }
}
