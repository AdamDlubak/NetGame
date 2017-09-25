using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Sequencer : MonoBehaviour
{
    public UnityEvent onTick;
    public float delay;

    private float _t = 0;

    void Update ()
    {
        _t += Time.deltaTime;

        if (_t > delay)
        {
            onTick.Invoke();
            _t = 0;
        }
    }
}
