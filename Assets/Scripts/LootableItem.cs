using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LootableItem : MonoBehaviour
{
    public enum LootType
    {
        ShipPart,
        RobotPart,
        EnergyItem
    }

    public LootType ItemType;
}
