using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerInput : NetworkBehaviour
{
    public Common.InputEvent onInputUpdate;
    public Common.FloatEvent onXAxisUpdate;

    public string turningAxisName;

    public KeyCode boostKey;

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        Common.InputStruct input;
        input.xAxis = Input.GetAxis(turningAxisName);
        input.boost = Input.GetKey(boostKey);

        onInputUpdate.Invoke(input);
        onXAxisUpdate.Invoke(input.xAxis);
    }

}
