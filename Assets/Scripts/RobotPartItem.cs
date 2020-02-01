using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotPartItem : LootableItem
{
    public RobotPartType type;

    public RobotPartItem()
    {
        ItemType = LootType.RobotPart;
    }
}
