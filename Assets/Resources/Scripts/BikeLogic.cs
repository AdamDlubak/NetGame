using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BikeMovementController))]
[RequireComponent(typeof(BikeTrail))]


public class BikeLogic : NetworkBehaviour
{

    public Camera leftCamera;
    public Camera rightCamera;
    public Camera backCamera;

    [System.Serializable]
    public struct Parameters
    {
        public LayerMask deadlyObjectsLayerMask;
    }

    public Parameters parameters;

    private Rigidbody _body;
    private BikeMovementController _movementController;
    private BikeTrail _trail;

    public bool _isAlive = true;

    void InitializeLocalPlayer()
    {
        if (!isLocalPlayer)
        {
            Debug.LogError("Script should only run for local player.");
            return;
        }

        Camera cam = Camera.main;
        
        if (cam == null || leftCamera == null || rightCamera == null || backCamera == null)
        {
            Debug.LogError("Camera not found");
        }

        RigidbodyFollow follow = cam.GetComponent<RigidbodyFollow>();
        RigidbodyFollow followLeftCamera = cam.GetComponent<RigidbodyFollow>();
        RigidbodyFollow followRightCamera = cam.GetComponent<RigidbodyFollow>();
        RigidbodyFollow followBackCammera = cam.GetComponent<RigidbodyFollow>();

        if (follow == null)
        {
            Debug.LogError("RigidbodyFollow component not found in camera.");
        }

        follow.target = _body;
        followLeftCamera.target = _body;
        followRightCamera.target = _body;
        followBackCammera.target = _body;
    }

    void Start()
    {
        _body = GetComponent<Rigidbody>();
        _movementController = GetComponent<BikeMovementController>();
        _trail = GetComponent<BikeTrail>();

        _movementController.enabled = false;
        _trail.enabled = false;
        _body.Sleep();

        if (isLocalPlayer)
            InitializeLocalPlayer();
    }

    void OnCollisionEnter(Collision col)
    {
        if (_isAlive && parameters.deadlyObjectsLayerMask.value == (parameters.deadlyObjectsLayerMask.value | (1 << col.gameObject.layer)))
        {
            Die();
        }
    }

    void OnMatchStarted()
    {
        if (!isLocalPlayer)
            return;

        _movementController.enabled = true;
        _trail.enabled = true;
        _body.WakeUp();
    }

    void OnEnable()
    {
        GameMaster.OnMatchStartedEvent += OnMatchStarted;
    }

    void OnDisable()
    {
        GameMaster.OnMatchStartedEvent -= OnMatchStarted;
    }

#region PublicInterface

    public static event System.Action OnDeathEvent;
    public UnityEvent onDeath;

    public void Die()
    {
        if (!isLocalPlayer)
            return;

        if (!_isAlive)
            return;
            
        _isAlive = false;
        onDeath.Invoke();

        if (OnDeathEvent != null)
            OnDeathEvent();
    }
    
    public void Spawn(GameObject prefab)
    {
        Instantiate(prefab, transform.position, transform.rotation);
    }

#endregion
}
