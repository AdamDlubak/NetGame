using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Common
{
    [System.Serializable]
    public struct InputStruct
    {
        public float xAxis;

        public bool boost;
    }

    [System.Serializable]
    public class InputEvent : UnityEvent<InputStruct> {}

    [System.Serializable]
    public class FloatEvent : UnityEvent<float> {}

    [System.Serializable]
    public class StringEvent : UnityEvent<string> {}

    public static class Methods
    {
        public static Vector3 RandomVector3()
        {
            return new Vector3(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        }
    }

    public interface ISpawnable
    {
        void SpawnedBy(GameObject spawner);
    }

    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour 
    {
        private static T _instance;

        public static T instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();

                    if (_instance == null)
                    {
                        GameObject obj = new GameObject();
                        _instance = obj.AddComponent<T>();

                        DontDestroyOnLoad(obj);
                    }
                }

                return _instance;
            }
        }
    }
}
