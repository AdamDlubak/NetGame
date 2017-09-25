using UnityEngine;
using UnityEngine.Events;

public class OnBikeDeath : MonoBehaviour
{
    public UnityEvent onBikeDeath;

    void OnEnable()
    {
        BikeLogic.OnDeathEvent += OnBikeDeathInternal;
    }

    void OnDisable()
    {
        BikeLogic.OnDeathEvent -= OnBikeDeathInternal;
    }

    private void OnBikeDeathInternal()
    {
        onBikeDeath.Invoke();
    }
}