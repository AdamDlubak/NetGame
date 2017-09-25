using UnityEngine;
using UnityEngine.Events;

public class BikeEvents : MonoBehaviour 
{

    void OnEnable()
    {
        BikeMovementController.OnBikeBoostStartEvent += onBikeBoostStart.Invoke;
        BikeMovementController.OnBikeBoostEndEvent += onBikeBoostEnd.Invoke;
        BikeLogic.OnDeathEvent += onBikeDeath.Invoke;
    }

    void OnDisable()
    {
        BikeMovementController.OnBikeBoostStartEvent -= onBikeBoostStart.Invoke;
        BikeMovementController.OnBikeBoostEndEvent -= onBikeBoostEnd.Invoke;
        BikeLogic.OnDeathEvent -= onBikeDeath.Invoke;
    }

#region PublicInterface
    public UnityEvent onBikeBoostStart;
    public UnityEvent onBikeBoostEnd;

    public UnityEvent onBikeDeath;

#endregion
}
