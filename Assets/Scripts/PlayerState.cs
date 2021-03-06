﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.SceneManagement;

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
    Item_1 = 1,
    Item_2 = 2,
    Item_3 = 3,
    Item_4 = 4,
    Item_5 = 5,
    Item_6 = 6,
    Item_7 = 7,
    Item_8 = 8,
}

public class PlayerState
{
    public ReactiveProperty<int> maxHP = new ReactiveProperty<int>();
    public ReactiveProperty<int> maxEnergy = new ReactiveProperty<int>();

    public ReactiveProperty<int> hp = new ReactiveProperty<int>();
    public ReactiveProperty<int> energy = new ReactiveProperty<int>();

    public ReactiveProperty<bool> canCarryItems = new ReactiveProperty<bool>();
    public ReactiveProperty<ShipItemData> carriedItem = new ReactiveProperty<ShipItemData>();

    public ReactiveDictionary<RobotPartType, bool> parts = new ReactiveDictionary<RobotPartType, bool>();

    public Subject<RobotPartItem> newPartItemReceived = new Subject<RobotPartItem>();

    public PlayerState()
    {
        maxHP.Value = 100;
        maxEnergy.Value = 100;

        hp.Value = 70;
        energy.Value = 20;

        hp.Subscribe(value => 
        {
            if (value <= 0)
            {
                SceneManager.LoadScene("LooseGame");
            }
        });

        energy.Subscribe(value =>
        {
            if (value <= 0)
            {
                SceneManager.LoadScene("LooseGame");
            }
        });

        
         parts.Add(RobotPartType.Head, true);
         parts.Add(RobotPartType.Body, true);
         parts.Add(RobotPartType.LeftHand, false);
         parts.Add(RobotPartType.RightHand, false);
         parts.Add(RobotPartType.LeftLeg, true);
         parts.Add(RobotPartType.RightLeg, false);
         /*
        parts.Add(RobotPartType.Head, true);
        parts.Add(RobotPartType.Body, true);
        parts.Add(RobotPartType.LeftHand, true);
        parts.Add(RobotPartType.RightHand, true);
        parts.Add(RobotPartType.LeftLeg, true);
        parts.Add(RobotPartType.RightLeg, true);

        canCarryItems.Value = true;*/
    }

    public bool ReceivePart(RobotPartItem robotPartItem)
    {
        parts[robotPartItem.type] = true;
        newPartItemReceived.OnNext(robotPartItem);

        Debug.Log("Picked Item : " + Enum.GetName(typeof(RobotPartType), robotPartItem.type));

        if (parts[RobotPartType.LeftHand] && parts[RobotPartType.RightHand])
        {
            canCarryItems.Value = true;
        }
        else
        {
            canCarryItems.Value = false;
        }

        return true;
    }

    public bool CarryItem(ShipPartItem shipPartItem)
    {
        if (canCarryItems.Value && carriedItem.Value == null)
        {
            ShipItemData shipItemData = new ShipItemData(shipPartItem.shipPartType);
            shipItemData.lootTypes = shipPartItem.ItemType;
            shipItemData.sprite = shipPartItem.sprite;
            shipItemData.cardSprite = shipPartItem.cardSprite;

            carriedItem.Value = shipItemData;
            Debug.Log("Picked ship Item : " + Enum.GetName(typeof(ShipPartType), shipPartItem.shipPartType));
            return true;
        }
        else
        {
            Debug.Log("Cannot pick up item");
            return false;
        }
    }

    internal void GiveEnergy(EnergyItem energyItem)
    {
        int newEnergy = energy.Value + energyItem.energy;

        if (newEnergy > maxEnergy.Value)
        {
            newEnergy = maxEnergy.Value;
        }

        energy.Value = newEnergy;
    }

    internal void GiveHP(HPItem hPItem)
    {
        int newHP = hp.Value + hPItem.hp;

        if (newHP > maxHP.Value)
        {
            newHP = maxHP.Value;
        }

        hp.Value = newHP;
    }
}
