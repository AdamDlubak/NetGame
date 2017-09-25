using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSystemController : MonoBehaviour
{
    public UnityEvent onStart;
    public UnityEvent onEnd;

    private bool _isOver = false;
    private ParticleSystem _system;

    void Start()
    {
        _system = GetComponent<ParticleSystem>();
        onStart.Invoke();
    }

    void Update()
    {
        if (!_isOver)
        {
            if (_system.particleCount <= 0)
            {
                End();
            }
        }
    }

    private void End()
    {
        _isOver = true;
        onEnd.Invoke();
    }
}
