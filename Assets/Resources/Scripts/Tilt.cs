using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Tilt : MonoBehaviour {

    [System.Serializable]
    public struct Parameters
    {
        public AnimationCurve tiltToRotation;
        public Vector3 rotationAxis;

        public bool interpolationEnabled;
        public float interpolationSpeed;
    }

    public Parameters parameters;

    [Range(-1, 1)]
    public float tilt = 0;

    private float _currentTilt;

    void Update()
    {
        if (parameters.interpolationEnabled)
        {
            _currentTilt = Mathf.Lerp(_currentTilt, tilt, parameters.interpolationSpeed);
        }
        else
        {
            _currentTilt = tilt;
        }

        transform.localRotation = Quaternion.Euler(parameters.rotationAxis * parameters.tiltToRotation.Evaluate(_currentTilt));
    }

#region PublicInterface

    public void SetTilt(float value)
    {
        tilt = value;
    }

#endregion
}
