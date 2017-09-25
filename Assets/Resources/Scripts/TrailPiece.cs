using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailPiece : MonoBehaviour
{
    private Vector3 _targetScale;
    public float lerpSpeed;

    void Awake()
    {
        _targetScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    void Update ()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, _targetScale, lerpSpeed);
    }
}
