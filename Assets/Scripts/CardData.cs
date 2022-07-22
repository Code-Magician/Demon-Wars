using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "CardGame/Card")]
public class CardData : ScriptableObject
{
    public enum DamageType
    {
        Fire, Ice, Both, Destruct
    }


    public string titleText;

    [TextArea]
    public string descriptionText;

    public DamageType damagetype;

    public int Cost;
    public int Damage;

    public Sprite cardImage;
    public Sprite cardFrame;

    public int NumberInDeck;

    public bool isDefenceCard = false;
    public bool isMirrorCard = false;
    public bool isMultiCard = false;
    public bool isDestruct = false;
}
