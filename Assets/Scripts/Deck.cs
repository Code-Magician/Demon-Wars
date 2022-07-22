using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Deck
{
    public List<CardData> cardDatas = new List<CardData>();
    public void create()
    {
        //saare 34 cards ek list mein daalna hai 
        List<CardData> cardDataInOrder = new List<CardData>();

        foreach (CardData x in GamePlay.instance.cards)
        {
            for (int i = 0; i < x.NumberInDeck; i++)
                cardDataInOrder.Add(x);
        }

        // randamize this created list
        for (int i = 0; i < cardDataInOrder.Count; i++)
        {
            int randomIndex = Random.Range(0, cardDataInOrder.Count);

            CardData temp = cardDataInOrder[i];
            cardDataInOrder[i] = cardDataInOrder[randomIndex];
            cardDataInOrder[randomIndex] = temp;

        }

        //finally putting all randamized list into cardDatas created first...
        cardDatas = cardDataInOrder;
    }


    // gives random card
    private CardData RandomCard()
    {
        CardData result = null;

        if (cardDatas.Count == 0)
            create();

        result = cardDatas[0];
        cardDatas.RemoveAt(0);

        return result;
    }

    // creates new card with animation....
    private Card createNewCard(Vector3 position, string animename)
    {
        GameObject newCard = GameObject.Instantiate(GamePlay.instance.cardPrefab, GamePlay.instance.canvas.gameObject.transform);

        newCard.transform.position = position;
        Card card = newCard.GetComponent<Card>();

        if (card)
        {
            card.cardData = RandomCard();
            card.initializeData();

            Animator animator = newCard.GetComponentInChildren<Animator>();
            if (animator)
            {
                animator.CrossFade(animename, 0f);
                
            }
            else
            {
                Debug.LogError("Animator Not Found...");
            }

            return card;
        }
        else
        {
            Debug.LogError("Card Not Found...");
            return null;
        }
    }


    // deals card
    internal void dealCards(Hand hand)
    {
        for (int i = 0; i < 3; i++)
        {
            if (hand.handCards[i] == null)
            {
                hand.handCards[i] = createNewCard(hand.positions[i].position, hand.animeString[i]);
                

                if (hand.isplayer)
                {
                    hand.handCards[i].isInPlayersHand = true;
                    GamePlay.instance.player.playCardSound();
                }
                else
                    GamePlay.instance.enemy.playCardSound();

                return;
            }
        }

    }
}
