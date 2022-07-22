using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Effect : MonoBehaviour
{
    public Player   target      = null;
    public Image    effectImage = null;
    public Card     sourseCard  = null;

    public AudioSource iceAudio = null;
    public AudioSource fireBallAudio = null;
    public AudioSource destructAudio = null;

    //called when animation of effect from player  is at it's last frame...
    public void EndTrigger()
    {
        bool bounce = false;

        if (target.hasMirror())
        {
            bounce = true;
            target.setMirror(false);
            target.playSmashSound();

            if (target.isPlayer)
            {
                GamePlay.instance.castingAttackEffect(sourseCard, GamePlay.instance.enemy);
            }
            else
            {
                GamePlay.instance.castingAttackEffect(sourseCard, GamePlay.instance.player);
            }

            
        }
        else
        {
            int damage = sourseCard.cardData.Damage;

            if (!target.isPlayer)
            {
                if (sourseCard.cardData.damagetype == CardData.DamageType.Fire && target.isFireMonster)
                    damage /= 2;
                if (sourseCard.cardData.damagetype == CardData.DamageType.Ice && !target.isFireMonster)
                    damage /= 2;
            }

            target.health -= damage;
            target.playHitAnimation();

            //check for death
            if(target.health <= 0)
            {
                target.health = 0;
                if (target.isPlayer)
                    GamePlay.instance.playPlayerDieSound();
                else
                    GamePlay.instance.playEnemyDieSound();
            }

            GamePlay.instance.updateHealths();

            if (!bounce)
                GamePlay.instance.NextPlayersTurn();

            GamePlay.instance.isPlayable = true;
        }

        Destroy(gameObject);
    }

    internal void playIceSound()
    {
        iceAudio.Play();
    }

    internal void playFireBallSound()
    {
        fireBallAudio.Play();
    }

    internal void playDestructAudio()
    {
        destructAudio.Play();
    }
}
