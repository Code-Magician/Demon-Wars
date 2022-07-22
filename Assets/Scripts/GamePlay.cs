using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePlay : MonoBehaviour
{
    // singleton object
    static public GamePlay instance = null;

    public Deck playerDeck = new Deck();
    public Deck enemyDeck = new Deck();
    public List<CardData> cards = new List<CardData>();

    public Hand playerHand = new Hand();
    public Hand enemyHand  = new Hand();

    public Player player = null;
    public Player enemy = null;

    // isme cost and damage number images store hongi....
    public Sprite[] damageNumber = new Sprite[10];
    public Sprite[] costNumber = new Sprite[10];

    public GameObject cardPrefab = null;
    public Canvas canvas = null;

    //images of all the attack cards
    public Sprite fireEffectImage    = null;
    public Sprite iceEffectImage     = null;
    public Sprite multiFireEffectImage = null;
    public Sprite multiIceEffectImage = null;
    public Sprite FireAndIceEffectImage = null;
    public Sprite destructBallEffectImage = null;
    public Image enemySkipTurn = null;

    //enemy and player animation prefabs
    public GameObject EffectFromPlayer = null;
    public GameObject EffectFromEnemy = null;

    //tells if we can drag a card or not
    public bool isPlayable;

    //whose turn
    public bool playersTurn = true;
    public Text turnText = null;

    // monster images
    public Sprite fireMonster;
    public Sprite IceMonster;

    //score
    public Text ScoreText = null;
    public int demonsKilled = 0;
    public int score = 0;

    //die audio 
    public AudioSource playerDieAudio = null;
    public AudioSource enemyDieAudio = null;
    

    private void Awake()
    {
        instance = this;

        setUpNewEnemy();
        playerDeck.create();
        enemyDeck.create();

        StartCoroutine(DealHands());
    }

    //skips turn
    public void SkipTurn() {
        if(playersTurn && isPlayable)
        {
            NextPlayersTurn();
        }
    }

    // exits game
    public void QuitGame()
    {
        SceneManager.LoadScene(2);
    }

    //making this function coroutine to add timing 
    internal IEnumerator DealHands()
    {
        yield return new WaitForSeconds(1);
        for(int i = 0; i<3; i++)
        {
            playerDeck.dealCards(playerHand);
            enemyDeck.dealCards(enemyHand);
            yield return new WaitForSeconds(1);
        }
        isPlayable = true ;
    }

    internal bool useCard(Card card, Player usingOnPlayer, Hand fromHandOf)
    {
        //vaid or not
        // cast
        // remove
        // deal replacement card

        if(!isCardValid(card, usingOnPlayer, fromHandOf))
            return false;

        isPlayable = false;

        castCard(card, usingOnPlayer, fromHandOf);

        player.GlowImage.gameObject.SetActive(false);
        enemy.GlowImage.gameObject.SetActive(false);

        fromHandOf.burnCard(card);

        return false;
    }

    //agr card null nahi hai aur player yaa enemy ka mana >= card ki cost hai to card is valid...
    internal bool isCardValid(Card card, Player usingOnPlayer, Hand fromHandOf)
    {
        if (card == null)
            return false;

        if (fromHandOf.isplayer)
        {
            if(card.cardData.Cost <= player.mana)
            {
                if(usingOnPlayer.isPlayer && card.cardData.isDefenceCard)
                    return true;
                if(!usingOnPlayer.isPlayer && !card.cardData.isDefenceCard)
                    return true;
            }
            return false;
        }
        else // from enemy
        {
            if (card.cardData.Cost <= enemy.mana)
            {
                if (!usingOnPlayer.isPlayer && card.cardData.isDefenceCard)
                    return true;
                if (usingOnPlayer.isPlayer && !card.cardData.isDefenceCard)
                    return true;
            }

            return false;
        }

    }

    //takes card and casts it and sets animation for that casting
    internal void castCard(Card card, Player usingOnPlayer, Hand fromHandOf)
    {
        if (card.cardData.isMirrorCard)
        {
            usingOnPlayer.setMirror(true);
            usingOnPlayer.playMirrorSound();
            NextPlayersTurn();
            isPlayable = true;
        }
        else
        {
            if (card.cardData.isDefenceCard)
            {
                usingOnPlayer.playHealSound();
                StartCoroutine(castingDefenceEffect(card, usingOnPlayer));
            }
            else // casting attack 
            {
                castingAttackEffect(card, usingOnPlayer);
            }
            if(fromHandOf.isplayer)
                score += card.cardData.Damage;
            updateScore();
        }

        //updating mana balls of player and enemy...
        if (fromHandOf.isplayer)
        {
            player.mana -= card.cardData.Cost;
            player.updateManaBalls();
        }
        else
        {
            enemy.mana -= card.cardData.Cost;
            enemy.updateManaBalls();
        }
    }

    // casts an attack effect sets image to card prefab and makes it animate....
    // and sounds bhi yhi play hongi attacks ki
    internal void castingAttackEffect(Card card, Player usingOnPlayer)
    {
        GameObject effectobj = null;

        if (usingOnPlayer.isPlayer)
            effectobj = Instantiate(EffectFromEnemy, canvas.gameObject.transform);
        else
            effectobj = Instantiate(EffectFromPlayer, canvas.gameObject.transform);

        Effect effect = effectobj.GetComponent<Effect>();
        if (effect)
        {
            effect.target = usingOnPlayer;
            effect.sourseCard = card;

            switch (card.cardData.damagetype)
            {
                case CardData.DamageType.Fire:
                    if (card.cardData.isMultiCard)
                        effect.effectImage.sprite = multiFireEffectImage;
                    else
                        effect.effectImage.sprite = fireEffectImage;
                    effect.playFireBallSound();
                    break;

                case CardData.DamageType.Ice:
                    if (card.cardData.isMultiCard)
                        effect.effectImage.sprite = multiIceEffectImage;
                    else
                        effect.effectImage.sprite = iceEffectImage;
                    effect.playIceSound();
                    break;

                case CardData.DamageType.Both:
                    effect.effectImage.sprite = FireAndIceEffectImage;
                    effect.playIceSound();
                    effect.playFireBallSound();
                    break;
                case CardData.DamageType.Destruct:
                    effect.effectImage.sprite = destructBallEffectImage;
                    effect.playDestructAudio();
                    break;
            }
        }
    }



    //casting defence cards....
    internal IEnumerator castingDefenceEffect(Card card, Player usingOnPlayer)
    {
        usingOnPlayer.health += card.cardData.Damage;

        if (usingOnPlayer.health > usingOnPlayer.maxHealth)
            usingOnPlayer.health = usingOnPlayer.maxHealth;

        updateHealths();
        yield return new WaitForSeconds(0.5f);
        NextPlayersTurn();
        isPlayable = true;
    }


    //updates health of enemy and player and checks if game is over or not ...
    internal void updateHealths()
    {
        player.updateHealth();
        enemy.updateHealth();
        

        if(player.health <= 0)
        {
            StartCoroutine(YouLose());
        }
        
        if(enemy.health <= 0)
        {
            demonsKilled++;
            score += 10;
            updateScore();
            StartCoroutine(newEnemy());
        }
    }

    internal IEnumerator newEnemy()
    {
        enemy.gameObject.SetActive(false);
        enemyHand.clearHand();
        yield return new WaitForSeconds(1);
        setUpNewEnemy();
        enemy.gameObject.SetActive(true);
        StartCoroutine(DealHands());
    }

    private void setUpNewEnemy()
    {
        enemy.mana = 0;
        enemy.health = 5;
        enemy.updateHealth();
        enemy.isFireMonster = true;
        enemy.playerImage.sprite = fireMonster;
        if (Random.Range(0, 2) == 1)
        {
            enemy.isFireMonster = false;
            enemy.playerImage.sprite = IceMonster;
        }
    }
    
    private IEnumerator YouLose()
    {
        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(2);
    }
    
    //changes turn text and adds one mana to the player whose turn it is
    internal void NextPlayersTurn()
    {
        playersTurn = !playersTurn;
        bool isEnemyDead = false;
        if (playersTurn)
        {   
            if(player.mana < 5)
                player.mana++;
        }
        else
        {
            if (enemy.health > 0)
            {
                if (enemy.mana < 5)
                    enemy.mana++;
            }
            else
                isEnemyDead = true;
        }

        // change turn text


        if (isEnemyDead)
        {
            playersTurn = !playersTurn;
            if (player.mana < 5)
                player.mana++;
        }
        else {
            turnTextChange();
            if (!playersTurn)
                MonsterTurn();
        }
        
        player.updateManaBalls();
        enemy.updateManaBalls();
    }

    internal void turnTextChange()
    {
        if (playersTurn)
            turnText.text = "Your Turn";
        else
            turnText.text = "Enemy's Turn";
    }

    private void MonsterTurn()
    {
        Card card = AIchooseCard();

        StartCoroutine(MonsterCast(card));
    }

    // chooses random card for player and checks if that card is valid or not...
    private Card AIchooseCard()
    {   
        List<Card> availableCards = new List<Card>();
        for (int i = 0; i < 3; i++)
        {

            if (isCardValid(enemyHand.handCards[i], enemy, enemyHand))
                availableCards.Add(enemyHand.handCards[i]);
            else if (isCardValid(enemyHand.handCards[i], player, enemyHand))
                availableCards.Add(enemyHand.handCards[i]);
        }

        if (availableCards.Count == 0)
            return null;

        int rand = Random.Range(0, availableCards.Count);

        return availableCards[rand];
    }

    //casts card
    private IEnumerator MonsterCast(Card card)
    {
        yield return new WaitForSeconds(1);

        if (card)
        {
            // turn card
            turnCard(card);
            yield return new WaitForSeconds(2);

            // use card
            if (card.cardData.isDefenceCard)
                useCard(card, enemy, enemyHand);
            else
                useCard(card, player, enemyHand);

            yield return new WaitForSeconds(1);

            // deal new card
            enemyDeck.dealCards(enemyHand);
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            // show skip button
            enemySkipTurn.gameObject.SetActive(true);
            yield return new WaitForSeconds(2);
            NextPlayersTurn();
            //hide skip button...
            enemySkipTurn.gameObject.SetActive(false);
        }
    }


    //flips card using flip trigger made in card object
    internal void turnCard(Card card)
    {
        Animator animator = card.GetComponentInChildren<Animator>();
        if (animator)
        {
            animator.SetTrigger("Flip");
        }
        else { Debug.LogError("Gameplay Funtion->turnCard else statement"); }
    }

    private void updateScore()
    {
        ScoreText.text = "Demons Killed : " +demonsKilled.ToString() + "     Score : " + score.ToString();
    }
    internal void playPlayerDieSound()
    {
        playerDieAudio.Play();
    }
    internal void playEnemyDieSound()
    {
        enemyDieAudio.Play();
    }
}
