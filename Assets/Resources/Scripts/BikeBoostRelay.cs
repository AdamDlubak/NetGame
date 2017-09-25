using System;
using UnityEngine;

public class BikeBoostRelay : MonoBehaviour
{
    public BikeMovementController movementController;
    public TextMesh text;

    void Update()
    {
        text.text = String.Format("{0:P0}", movementController.GetBoostAmount());
    }
}
