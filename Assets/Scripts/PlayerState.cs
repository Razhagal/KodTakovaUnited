using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public enum RobotPartType
{
    Head,
    Body,
    LeftHand,
    RightHand,
    LeftLeg,
    RightLeg
}

public enum ShipPartType
{
    Tire,
}

public class PlayerState
{
    public ReactiveProperty<int> maxHP = new ReactiveProperty<int>();
    public ReactiveProperty<int> maxEnergy = new ReactiveProperty<int>();

    public ReactiveProperty<int> hp = new ReactiveProperty<int>();
    public ReactiveProperty<int> energy = new ReactiveProperty<int>();

    public ReactiveProperty<bool> canCarryItems = new ReactiveProperty<bool>();
    public ReactiveProperty<LootableItem> carriedItem = new ReactiveProperty<LootableItem>();

    public ReactiveDictionary<RobotPartType, bool> parts = new ReactiveDictionary<RobotPartType, bool>();

    public PlayerState()
    {
        maxHP.Value = 100;
        maxEnergy.Value = 100;

        hp.Value = 100;
        energy.Value = 100;

        parts.Add(RobotPartType.Head, true);
        parts.Add(RobotPartType.Body, true);
        parts.Add(RobotPartType.LeftHand, false);
        parts.Add(RobotPartType.RightHand, false);
        parts.Add(RobotPartType.LeftLeg, true);
        parts.Add(RobotPartType.RightLeg, false);
    }

    public bool ReceivePart(RobotPartItem robotPartItem)
    {
        parts[robotPartItem.type] = true;

        return true;
    }

    public bool CarryItem(ShipPartItem shipPartItem)
    {
        if (canCarryItems.Value)
        {
            carriedItem.Value = shipPartItem;
        }

        return true;
    }
}
