using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LootableItem : MonoBehaviour
{
    public enum LootType
    {
        ShipPart,
        PlayerPart,
        EnergyItem
    }

    public LootType ItemType;
}
