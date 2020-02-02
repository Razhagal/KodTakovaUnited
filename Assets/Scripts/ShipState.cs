using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ShipState
{
    public ReactiveProperty<int> expectedParts = new ReactiveProperty<int>();
    public ReactiveProperty<int> receivedParts = new ReactiveProperty<int>();

    public ShipState()
    {
        expectedParts.Value = 2;
    }

    public bool ReceiveItem(ShipItemData shipItemData)
    {
        receivedParts.Value++;
        return true;
    }
}
