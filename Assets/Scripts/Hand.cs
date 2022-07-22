using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hand 
{
    public Card[]       handCards       = new Card[3];
    public Transform[]  positions        = new Transform[3];
    public string[]     animeString     = new string[3];

    public bool isplayer;

    public void burnCard(Card card) {

        for(int i = 0; i<3; i++)
        {
            if(handCards[i] == card)
            {
                GameObject.Destroy(handCards[i].gameObject);
                handCards[i] = null;

                if (isplayer)
                    GamePlay.instance.playerDeck.dealCards(this);
                else
                    GamePlay.instance.enemyDeck.dealCards(this);
                break;
            }
        }
    }

   internal void clearHand()
    {
        for(int i=0; i < 3; i++)
        {
            GameObject.Destroy(handCards[i].gameObject);
            handCards[i] = null;
        }
    }
}
