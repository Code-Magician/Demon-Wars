using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 initalPosition;
    public void OnBeginDrag(PointerEventData eventData)
    {
        initalPosition = transform.position;
        // added canvas grounp in card prefab and jb ye false hoga to card jb baaki cheezo p drop hoga and un 
        // cheezo m drop ki script hogi to vo chl jaaegi....
        Card card = GetComponent<Card>();
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        Card card = GetComponent<Card>();

        if (!GamePlay.instance.isPlayable)
            return;

        bool overcard = false;


        if(card.isInPlayersHand)
            transform.position += (Vector3)eventData.delta;

        

        // eventData.hovered is array off all the object that are bering hovered...
        foreach (GameObject x in eventData.hovered)
        {
            
            BurnCard burnCard = x.GetComponent<BurnCard>();
            if (burnCard != null)
            {
                card.burnImage.gameObject.SetActive(true);
            }
            else
                card.burnImage.gameObject.SetActive(false);

                
            Player player = x.GetComponent<Player>();
            if(player != null)
            {
                if (GamePlay.instance.isCardValid(card, player, GamePlay.instance.playerHand))
                {
                    player.GlowImage.gameObject.SetActive(true);
                    overcard = true;
                }
            }
            
        }

        if (!overcard)
        {
            GamePlay.instance.player.GlowImage.gameObject.SetActive(false);
            GamePlay.instance.enemy.GlowImage.gameObject.SetActive(false);
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = initalPosition;
        // phrse blockraycast ko true krdia....
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
