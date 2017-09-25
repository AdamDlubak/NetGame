using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DebugInput : MonoBehaviour
{

    [System.Serializable]
    public struct InputEvent
    {
        public UnityEvent onInput;
        public KeyCode keyCode;
    }

    public List<InputEvent> inputEvents;

    void Update()
    {
        foreach (InputEvent ev in inputEvents)
        {
            if (Input.GetKeyDown(ev.keyCode))
                ev.onInput.Invoke();
        }
    }

}
