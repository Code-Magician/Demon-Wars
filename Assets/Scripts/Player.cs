using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDropHandler
{
    public Image playerImage = null;
    public Image HealthImage = null;
    public Image MirrorImage = null;
    public Image GlowImage = null;
    public Image HitAttackImage = null;

    public int maxHealth = 5;
    public int health = 5;
    public int mana = 1;

    public bool isPlayer;
    public bool isFireMonster;

    public GameObject[] manaballs = new GameObject[5];

    public Animator animator = null;

    public AudioSource cardAudio = null;
    public AudioSource mirrorAudio = null;
    public AudioSource smashMirrorAudio = null;
    public AudioSource healAudio = null;
    

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        updateHealth();
        updateManaBalls();
    }

    internal void playHitAnimation()
    {
        if(animator != null)
        {
            animator.SetTrigger("Hit");
            return;
        }

        Debug.LogError("Player Animator Null");
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (GamePlay.instance.isPlayable)
        {
            GameObject obj = eventData.pointerDrag;
            if (obj != null)
            {
                Card card = obj.GetComponent<Card>();

                if (card != null)
                {
                    GamePlay.instance.useCard(card, this, GamePlay.instance.playerHand);
                }
                else
                    Debug.LogError("No card available...");
            }
            else
                Debug.LogError("No Obj available...");
        }
    }

    internal void updateHealth()
    {
        if(health>=0 && health <= GamePlay.instance.costNumber.Length)
        {
            if (isPlayer)
                HealthImage.sprite = GamePlay.instance.costNumber[health];
            else
                HealthImage.sprite = GamePlay.instance.damageNumber[health];
        }
        else
        {
            Debug.LogWarning("Invalid Health Number.." + health);
        }
    }

    //sets mirror image active
    internal void setMirror(bool on)
    {
        MirrorImage.gameObject.SetActive(on);
    }

    internal bool hasMirror()
    {
        return MirrorImage.gameObject.activeInHierarchy;
    }

    //updates mana ball display
    internal void updateManaBalls()
    {
        for(int i = 0; i<5; i++)
        {
            if (mana > i)
                manaballs[i].SetActive(true);
            else
                manaballs[i].SetActive(false);
        }
    }

    internal void playCardSound()
    {
        cardAudio.Play();
    }

    internal void playMirrorSound()
    {
        mirrorAudio.Play();
    }

    internal void playSmashSound()
    {
        smashMirrorAudio.Play();
    }

    internal void playHealSound()
    {
        healAudio.Play();
    }

   


}
