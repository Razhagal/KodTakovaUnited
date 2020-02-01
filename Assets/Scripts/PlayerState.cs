using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerState
{
    public ReactiveProperty<int> maxHP = new ReactiveProperty<int>();
    public ReactiveProperty<int> maxEnergy = new ReactiveProperty<int>();

    public ReactiveProperty<int> hp = new ReactiveProperty<int>();
    public ReactiveProperty<int> energy = new ReactiveProperty<int>();

    public ReactiveProperty<bool> canCarryItems = new ReactiveProperty<bool>();
    public ReactiveProperty<LootableItem> carriedItem = new ReactiveProperty<LootableItem>();

    public PlayerState()
    {
        maxHP.Value = 100;
        maxEnergy.Value = 100;

        hp.Value = 100;
        energy.Value = 100;
    }
}
