using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData
{
    public LootType lootTypes;
    public Sprite sprite;
    public Sprite cardSprite;
}

public class ShipItemData : ItemData
{
    public ShipPartType shipTypePart;

    public ShipItemData(ShipPartType shipTypePart)
    {
        this.shipTypePart = shipTypePart;
    }
}

public class RobotItemData : ItemData
{
    public RobotPartType robotTypePart;

    public RobotItemData(RobotPartType robotTypePart)
    {
        this.robotTypePart = robotTypePart;
    }
}