using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BurnCard : MonoBehaviour, IDropHandler
{
    public AudioSource burnAudio = null;
    public void OnDrop(PointerEventData eventData)
    {
        if (GamePlay.instance.isPlayable) {
            GameObject obj = eventData.pointerDrag;
            Card card = obj.GetComponent<Card>();

            if (card != null)
            {
                playBurnSound();
                GamePlay.instance.playerHand.burnCard(card);
                GamePlay.instance.NextPlayersTurn();
            }
            else
                Debug.LogError("No card available to burn...");
        }
        
    }

    internal void playBurnSound()
    {
        burnAudio.Play();
    }
}
