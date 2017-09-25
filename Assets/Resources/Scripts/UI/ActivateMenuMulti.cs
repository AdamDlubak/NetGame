using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateMenuMulti : MonoBehaviour
{
    public static event System.Action OnMenuMultiActivated;

    public void NotifyNetworkManager()
    {
        if (OnMenuMultiActivated != null)
            OnMenuMultiActivated();
    }
}
