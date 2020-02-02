using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotPartItem : LootableItem
{
    public RobotPartType type;

    private void Start()
    {
        ItemType = LootType.RobotPart;

        switch (type)
        {
            case RobotPartType.LeftHand:
                cardSprite = Resources.Load<Sprite>("RobotParts/RobotHand_2-card");
                break;
            case RobotPartType.RightHand:
                cardSprite = Resources.Load<Sprite>("RobotParts/RobotHand_1-card");
                break;
            case RobotPartType.LeftLeg:
                cardSprite = Resources.Load<Sprite>("RobotParts/RobotLeg_2-card");
                break;
            case RobotPartType.RightLeg:
                cardSprite = Resources.Load<Sprite>("RobotParts/RobotLeg_1-card");
                break;
            default:
                break;
        }
    }
}
