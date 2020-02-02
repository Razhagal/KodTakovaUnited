using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;
using UnityEngine.SceneManagement;

public class HudController : MonoBehaviour
{
    public ItemNotification itemNotification;
    [Space]
    public Slider hpSlider;
    public Slider energySlider;

    public Transform itemContainer;
    public Image item;

    public BetweenSlider hpBetweenSlider;
    public BetweenSlider energyBetweenSlider;
    public BetweenScale itemContainerBetweenSlider;

    public GameObject shipObjects;
    public TextMeshProUGUI txtShipProgress;
    public BetweenScale betweenScaleShipProgress;

    private PlayerState playerState;
    private ShipState shipState;

    public void Start()
    {
        playerState = ServiceLocator.Instance.GetInstanceOfType<PlayerState>();
        shipState = ServiceLocator.Instance.GetInstanceOfType<ShipState>();

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
                UpdateEnergy(values.Previous, values.Current);
            })
            .AddTo(this);

        playerState.canCarryItems
            .Subscribe(value =>
            {
                itemContainer.gameObject.SetActive(value);
               // shipObjects.gameObject.SetActive(value);
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
                    PickedItem(newItem);
                }
            })
            .AddTo(this);

        playerState.newPartItemReceived
            .Subscribe(newPart => 
            {
                PickedItem(newPart);
            })
            .AddTo(this);

        shipState.receivedParts
            .Subscribe(vall => 
            {
                txtShipProgress.text = vall + "/" + shipState.expectedParts.Value;
                betweenScaleShipProgress.ResetToBeginning();
                betweenScaleShipProgress.PlayForward();

                if (shipState.receivedParts.Value == shipState.expectedParts.Value)
                {
                    SceneManager.LoadScene("EndGame");
                }
            })
            .AddTo(this);

        StartCoroutine(LooseEnergy());
        //test
       // shipState.expectedParts.Value = 1;
    }

    private IEnumerator LooseEnergy()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            playerState.energy.Value -= 10;

            // test
           //ShipState x = ServiceLocator.Instance.GetInstanceOfType<ShipState>();
           //x.receivedParts.Value = x.expectedParts.Value;
        }
    }

    private void PickedItem(RobotPartItem newItem)
    {
        string info = "";

        if ((newItem.type == RobotPartType.LeftHand && playerState.parts[RobotPartType.RightHand]) ||
            (newItem.type == RobotPartType.RightHand && playerState.parts[RobotPartType.LeftHand]))
        {
            info = "You can now carry ship parts to repair your ship";
        }
        else if ((newItem.type == RobotPartType.LeftHand && !playerState.parts[RobotPartType.RightHand]) ||
            (newItem.type == RobotPartType.RightHand && !playerState.parts[RobotPartType.LeftHand]))
        {
            info = "You can now attack enemies";
        }
        else if (newItem.type == RobotPartType.LeftLeg || newItem.type == RobotPartType.RightLeg)
        {
            info = "You can now move faster";
        }

        itemNotification.PopulateData(info, newItem.cardSprite);
    }

    private void PickedItem(ShipItemData newItem)
    {
        item.gameObject.SetActive(true);
        item.sprite = newItem.sprite;
        item.SetNativeSize();

        itemContainerBetweenSlider.ResetToBeginning();
        itemContainerBetweenSlider.PlayForward();

        if (shipState.receivedParts.Value == 0)
        {
            string info = "Bring this to your ship";
            itemNotification.PopulateData(info, newItem.cardSprite);
        }
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
