using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetMultiData : MonoBehaviour
{

    public InputField addressField;
    public InputField nickField;

    public void SetAddress()
    {
        DataManager.instance.address = addressField.text;
    }

    public void SetNick()
    {
        DataManager.instance.nick = nickField.text;
    }
}
