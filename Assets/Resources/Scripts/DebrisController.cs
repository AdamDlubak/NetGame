using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisController : MonoBehaviour, Common.ISpawnable {

    [System.SerializableAttribute]
    public struct Parameters
    {
        public float velocityMultiplier;
        public float randomSpread;

        public float debrisLifetime;

        public bool spawnEmbers;
        public GameObject emberPrefab;
        public int emberCount;
    }

    public Parameters parameters;

    public List<Rigidbody> debrisBodies = new List<Rigidbody>();

    private List<ParticleSystem> _particleSystems;

    private float _lifetime;

    void Start()
    {
        foreach (var b in debrisBodies)
        {
            Vector3 random = Common.Methods.RandomVector3();
            b.velocity = (transform.forward * parameters.velocityMultiplier) + random*parameters.randomSpread;
        }

        _lifetime = parameters.debrisLifetime;

        _particleSystems = new List<ParticleSystem>(GetComponentsInChildren<ParticleSystem>());

        if (parameters.spawnEmbers)
        {
            for (int i = 0; i < parameters.emberCount; i++)
            {
                GameObject ember = Instantiate(parameters.emberPrefab, transform.position, transform.rotation);

                Rigidbody emberBody = ember.GetComponent<Rigidbody>();

                if (emberBody)
                {
                    Vector3 random = Common.Methods.RandomVector3();
                    emberBody.velocity = (transform.forward * parameters.velocityMultiplier) + random*parameters.randomSpread;
                }
            }
        }
    }

    void Update()
    {
        if (_lifetime <= 0)
        {
            foreach (var b in debrisBodies)
            {
                Destroy(b.gameObject);
            }

            debrisBodies.Clear();

            bool areParticlesDone = true;

            foreach (var p in _particleSystems)
            {
                if (p.particleCount != 0)
                {
                    areParticlesDone = false;
                    break;
                }
            }

            if (areParticlesDone)
            {
                foreach (var p in _particleSystems)
                {
                    Destroy(p.gameObject);
                }
                _particleSystems.Clear();

                Destroy(gameObject);
            }
        }
        else
        {
            _lifetime -= Time.deltaTime;
        }
    }

#region PublicInterface

    public void SetVelocity(Vector3 velocity)
    {
    }

    public void SpawnedBy(GameObject spawner)
    {
        Rigidbody body = spawner.GetComponent<Rigidbody>();
        if (body)
        {
            SetVelocity(body.velocity);
        }
    }

    public void ParticleBoom(int amount)
    {
        foreach (var p in _particleSystems)
        {
            p.Emit(amount);
        }
    }

#endregion

}
