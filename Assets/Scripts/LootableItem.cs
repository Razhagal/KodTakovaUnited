using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LootType
{
    ShipPart,
    RobotPart,
    EnergyItem
}

public class LootableItem : MonoBehaviour
{
    [HideInInspector]
    public Sprite sprite;
    public LootType ItemType;
    public Sprite cardSprite;
}
