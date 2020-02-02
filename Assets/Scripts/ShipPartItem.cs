using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPartItem : LootableItem
{
    public ShipPartType shipPartType;

    private void Start()
    {
        ItemType = LootType.ShipPart;
        sprite = Resources.Load<Sprite>("ShipParts/" + (int)shipPartType);
        cardSprite = Resources.Load<Sprite>("ShipParts/card-" + (int)shipPartType);
    }
}
