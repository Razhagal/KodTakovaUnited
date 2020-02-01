using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class ShipController : MonoBehaviour
{
    public Slider shipRepairSlider;

    private ShipState shipState;

    private void Start()
    {
        shipState = ServiceLocator.Instance.GetInstanceOfType<ShipState>();

        shipState.receivedParts
            .Subscribe(num => 
            {
                UpdateShipState();
            })
            .AddTo(this);
    }

    private void UpdateShipState()
    {
        shipRepairSlider.value = (float)shipState.receivedParts.Value / shipState.expectedParts.Value;

        if (shipState.receivedParts.Value == shipState.expectedParts.Value)
        {
            Debug.Log("Game WON!!!!!!!!!!!!!!!");
        }
    }
}
