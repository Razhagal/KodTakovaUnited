using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class HudController : MonoBehaviour
{
    public Slider hpSlider;
    public Slider energySlider;

    public Transform itemContainer;
    public Image item;

    public BetweenSlider hpBetweenSlider;
    public BetweenSlider energyBetweenSlider;
    public BetweenScale itemContainerBetweenSlider;

    private PlayerState playerState;

    public void Start()
    {
        playerState = ServiceLocator.Instance.GetInstanceOfType<PlayerState>();

        hpSlider.value = (float)playerState.hp.Value / playerState.maxHP.Value;
        energySlider.value = (float)playerState.energy.Value / playerState.maxEnergy.Value;

        playerState.hp
            .Pairwise()
            .Subscribe(values => 
            {
                UpdateHP(values.Previous, values.Current);
            })
            .AddTo(this);

        playerState.energy
            .Pairwise()
            .Subscribe(values =>
            {
                UpdateHP(values.Previous, values.Current);
            })
            .AddTo(this);

        playerState.canCarryItems
            .Subscribe(value =>
            {
                itemContainer.gameObject.SetActive(value);
            })
            .AddTo(this);

        playerState.carriedItem
            .Subscribe(newItem =>
            {
                if (newItem == null)
                {
                    item.gameObject.SetActive(false);
                    return;
                }
                else
                {
                    item.gameObject.SetActive(true);
                    item.sprite = newItem.sprite;
                    item.SetNativeSize();
                }
            })
            .AddTo(this);
    }

    private void UpdateHP(int oldValue, int newValue)
    {
        hpBetweenSlider.From = (float)oldValue / playerState.maxHP.Value;
        hpBetweenSlider.To = (float)newValue / playerState.maxHP.Value;

        hpBetweenSlider.ResetToBeginning();
        hpBetweenSlider.PlayForward();
    }

    public void OnCLickChangeHP(int hp)
    {
        playerState.hp.Value = hp;
    }

    private void UpdateEnergy(int oldValue, int newValue)
    {
        energyBetweenSlider.From = (float)oldValue / playerState.maxEnergy.Value;
        energyBetweenSlider.To = (float)newValue / playerState.maxEnergy.Value;

        energyBetweenSlider.ResetToBeginning();
        energyBetweenSlider.PlayForward();
    }

    public void OnCLickChangeEnergy(int energy)
    {
        playerState.energy.Value = energy;
    }
}
