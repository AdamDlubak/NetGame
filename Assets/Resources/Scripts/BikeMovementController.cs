using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BikeMovementController : MonoBehaviour
{
    [System.Serializable]
    public struct Parameters
    {
        public float speed;
        public float turningForce;
        public float boostSpeedMultiplier;

        public float speedLerpingFactor;

        [Space(10)]
        [Header("Boost parameters")]
        public float boostCostPerSecond;
        public float boostRegenPerSecond;
        public float maxBoostAmount;
    }

    public Common.InputStruct inputs;

    public Parameters parameters;

    private Rigidbody _body;

    private float _currentBoostAmount;

    private bool _isBoosting = false;

    void Start ()
    {
        _body = GetComponent<Rigidbody>();
        _body.velocity = transform.forward * parameters.speed;
    }

    void Update()
    {
        if (inputs.boost)
        {
            _currentBoostAmount -= Time.deltaTime * parameters.boostCostPerSecond;
            if (_currentBoostAmount < 0)
            {
                _currentBoostAmount = 0;
            }

            if (!_isBoosting && _currentBoostAmount > 0)
            {
                if (OnBikeBoostStartEvent != null)
                    OnBikeBoostStartEvent();
            }

            if (_isBoosting && _currentBoostAmount <= 0)
            {
                if (OnBikeBoostEndEvent != null)
                    OnBikeBoostEndEvent();
            }

            _isBoosting = _currentBoostAmount > 0;
        }
        else
        {
            if (_isBoosting)
            {
                if (OnBikeBoostEndEvent != null)
                    OnBikeBoostEndEvent();
            }
            
            _isBoosting = false;
            if (_currentBoostAmount < parameters.maxBoostAmount)
            {
                _currentBoostAmount += Time.deltaTime * parameters.boostRegenPerSecond;
                if (_currentBoostAmount >= parameters.maxBoostAmount)
                {
                    _currentBoostAmount = parameters.maxBoostAmount;
                }
            }
        }
    }

    void FixedUpdate()
    {

        float targetSpeed = parameters.speed;

        if (_isBoosting)
        {
            targetSpeed *= parameters.boostSpeedMultiplier;
        }

        float horizInput = inputs.xAxis;
        _body.AddForce(Vector3.Cross(Vector2.up, _body.velocity).normalized * horizInput * parameters.turningForce);

        Vector3 bodyVelocityWithoutYAxis = _body.velocity - (Vector3.up * _body.velocity.y);

        transform.LookAt(transform.position + bodyVelocityWithoutYAxis.normalized, Vector3.up);

        _body.velocity = Vector3.Lerp(_body.velocity, transform.forward * targetSpeed, parameters.speedLerpingFactor);
    }

#region PublicInterface

    public static System.Action OnBikeBoostStartEvent;
    public static System.Action OnBikeBoostEndEvent;

    public void SetInput(Common.InputStruct input)
    {
        this.inputs = input;
    }

    public float GetBoostAmount()
    {
        return _currentBoostAmount;
    }

    public float GetBoostRatio()
    {
        return GetBoostAmount() / parameters.maxBoostAmount;
    }

    public bool IsBoosting()
    {
        return _isBoosting;
    }

    public float GetSpeed()
    {
        if (!_body)
            return 0;
            
        return _body.velocity.magnitude;
    }

    public float GetSpeedRatio()
    {
        return GetSpeed() / (parameters.speed * parameters.boostSpeedMultiplier);
    }

#endregion

}
