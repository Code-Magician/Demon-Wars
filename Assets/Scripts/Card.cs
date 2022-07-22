using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public CardData cardData = null;

    public Text titleText = null;
    public Text descriptionText = null;

    public Image CostImage = null;
    public Image DamageImage = null;

    public Image cardImage = null;
    public Image frameImage = null;
    public Image burnImage = null;
    public bool isInPlayersHand;


    //initializes all the data based on cardData given;
    public void initializeData()
    {
        if (cardData == null)
            Debug.LogError("Card Data is Empty.");

        titleText.text = cardData.titleText;
        descriptionText.text = cardData.descriptionText;

        // putting image based on card data from gameplay to costimage in card script.
        CostImage.sprite = GamePlay.instance.costNumber[cardData.Cost];
        DamageImage.sprite = GamePlay.instance.damageNumber[cardData.Damage];

        cardImage.sprite = cardData.cardImage;
        frameImage.sprite = cardData.cardFrame;
    }
}
