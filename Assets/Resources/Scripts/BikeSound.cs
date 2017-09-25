using UnityEngine;

[RequireComponent(typeof(BikeMovementController))]
[RequireComponent(typeof(BikeLogic))]
public class BikeSound : MonoBehaviour 
{

    [System.Serializable]
    public struct Parameters
    {
        public float volumeMultiplier;
        public float pitchMultiplier;

        [Header("Volume curves")]
        public AnimationCurve lowAccelerationVolumeCurve;
        public AnimationCurve highAcceleratinoVolumeCurve;

        
        [Space(10)]
        [Header("Pitch curves")]
        public AnimationCurve lowAccelerationPitchCurve;
        public AnimationCurve highAccelerationPitchCurve;

        [Space(10)]
        [Header("Audio source parameters")]
        public float minDistance;
        public float maxRolloffDistance;
        public float dopplerLevel;

        [Header("2D <-> 3D")]
        [Range(0, 1)]
        public float spatialBlend;
    }

    public Parameters parameters;

    public AudioClip accelerationLowClip;
    public AudioClip accelerationHighClip;

    private AudioSource _high;
    private AudioSource _low;

    private BikeMovementController _bikeMovementController;

    void Start()
    {
        _high = SetupAudioSource(accelerationHighClip);
        _low = SetupAudioSource(accelerationLowClip);

        _bikeMovementController = GetComponent<BikeMovementController>();
    }

    void OnEnable()
    {
        if (_high && _low)
        {
            _high.enabled = true;
            _low.enabled = true;
        }
    }

    void OnDisable()
    {
        if (_high && _low)
        {
            _high.enabled = false;
            _low.enabled = false;
        }
    }

    AudioSource SetupAudioSource(AudioClip clip)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = 0;
        source.loop = true;

        // start the clip from a random point
        source.time = Random.Range(0f, clip.length);
        source.Play();
        source.minDistance = parameters.minDistance;
        source.maxDistance = parameters.maxRolloffDistance;
        source.dopplerLevel = parameters.dopplerLevel;
        source.spatialBlend = parameters.spatialBlend;

        return source;
    }

    void Update()
    {
        float speed = _bikeMovementController.GetSpeedRatio();

        _low.volume = parameters.lowAccelerationVolumeCurve.Evaluate(speed) * parameters.volumeMultiplier;
        _low.pitch = parameters.lowAccelerationPitchCurve.Evaluate(speed) * parameters.pitchMultiplier;

        _high.volume = parameters.highAcceleratinoVolumeCurve.Evaluate(speed) * parameters.volumeMultiplier;
        _high.pitch = parameters.highAccelerationPitchCurve.Evaluate(speed) * parameters.pitchMultiplier;
    }
}
