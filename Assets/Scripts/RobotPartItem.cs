using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotPartItem : LootableItem
{
    public RobotPartType type;

    private void Start()
    {
        ItemType = LootType.RobotPart;
    }
}
