using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPartItem : LootableItem
{
    public ShipPartType shipPartType;

    public ShipPartItem()
    {
        ItemType = LootType.ShipPart;
    }
}
