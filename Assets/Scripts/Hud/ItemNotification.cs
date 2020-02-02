using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemNotification : MonoBehaviour
{
    public BetweenPosition betweenPosition;
    public BetweenAlpha betweenAlpha;

    public TextMeshProUGUI txtInfo; 
    public Image imgCard; 

    public void PopulateData(string text, Sprite sprite)
    {
        txtInfo.text = text;
        imgCard.sprite = sprite;

        betweenPosition.ResetToBeginning();
        betweenAlpha.ResetToBeginning();

        betweenPosition.PlayForward();

        gameObject.SetActive(true);
    }

    public void OnBetweenAlphaFinish()
    {
        gameObject.SetActive(false);
    }
}
