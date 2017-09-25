using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InputStringRelay : MonoBehaviour 
{
    public Common.StringEvent onInvoke;

    public InputField inputField;

    public void Go()
    {
        onInvoke.Invoke(inputField.text);
    }
}
