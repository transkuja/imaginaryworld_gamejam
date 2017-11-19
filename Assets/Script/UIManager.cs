﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public GameObject HandPlayer;
    public GameObject DeckPlayer;

    public GameObject HandEnemy;
    public GameObject DeckEnemy;

    public GameObject CardPlayedInitialPosition;

    public GameObject cardPlayerPrefab;
    public GameObject cardEnemyPrefab;

    public GameObject buttonFight;

    public GameObject turnText;
    private Vector3 turnTextOrigin;
    private Vector3 turnTextArrival;
    public bool moveTurnText = false;

    public static UIManager instance;

    public List<GameObject> tmpCardsPlayedInst = new List<GameObject>();

    // Player Hand
    bool updateHandPlayerPosition = false;
    Vector3[] positionHandPlayer;
    float timerPlayer = 0.0f;

    // Enemy Hand
    bool updateHandEnemyPosition = false;
    Vector3[] positionHandEnemy;
    float timerEnemy = 0.0f;

    // CardPlayed
    bool updateCardPlayedPosition = false;
    Vector3[] positionCardPlayed;
    float timerCardPlayed = 0.0f;

    public bool resolutionUi = false;
    public bool battleHandlerNeedNextTurn = false;
    public bool processInBetweenTurn = false;

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        Player.OnCardDamage += DamageCardAnimation;
        UpdateHandPlayerPosition();
        UpdateHandEnemyPosition();
        InitTmpCardPlayedPosition();
        turnTextOrigin = turnText.transform.position;
        turnTextArrival = turnTextOrigin + (0.4f* Vector3.right);
    }

    public void Update()
    {
        if (updateHandPlayerPosition)
            PlayInitHandPlayerPosition();

        if (updateHandEnemyPosition)
            PlayInitHandEnemyPosition();

        if (updateCardPlayedPosition)
            PlayInitTmpCardPlayedPosition();

        if (resolutionUi)
            CardResolutionUI();

        if (battleHandlerNeedNextTurn)
        {
            battleHandlerNeedNextTurn = false;
            BattleHandler.NextTurn();
        }

        if (moveTurnText)
            MoveTurnText();
    }

    private void OnDestroy()
    {
        Player.OnCardDamage -= DamageCardAnimation;
    }

    public void PlayerInitHand(List<Card> handCards)
    {
        for (int i = 0; i < handCards.Count; i++)
        {
            GameObject instCard = Instantiate(cardPlayerPrefab, DeckPlayer.gameObject.transform);
            instCard.transform.localPosition = Vector3.zero;
            //instCard.transform.localScale = Vector3.one;
            instCard.transform.SetParent(HandPlayer.transform);
            instCard.GetComponent<CardInstance>().CardData = handCards[i];
            instCard.GetComponent<CardInstance>().RefreshValues();
        }
    }

    public void EnemyInitHand(List<Card> handCards)
    {
        for (int i = 0; i < handCards.Count; i++)
        {
            GameObject instCard = Instantiate(cardEnemyPrefab, DeckEnemy.gameObject.transform);
            instCard.transform.localPosition = Vector3.zero;
            //instCard.transform.localScale = Vector3.one;
            instCard.transform.SetParent(HandEnemy.transform);
            
            instCard.GetComponent<CardInstance>().CardData = handCards[i];
            instCard.GetComponent<CardInstance>().IsHidden = true;
            instCard.GetComponent<CardInstance>().RefreshValues();


        }
    }


    public void UpdateHandPlayerPosition()
    {
        positionHandPlayer = new Vector3[GameManager.instance.CurrentPlayer.playerData.playerCards.Count];
        float spaceBetweenCards = cardPlayerPrefab.GetComponent<RectTransform>().rect.width / 15.0f;
        for (int i = 0; i < positionHandPlayer.Length; i++)
        {
            positionHandPlayer[i] = HandPlayer.transform.position + (i * cardPlayerPrefab.GetComponent<RectTransform>().rect.width * cardPlayerPrefab.GetComponent<RectTransform>().localScale.x * Vector3.right) + (i > 0 ? (spaceBetweenCards * Vector3.right*i) : Vector3.zero);
        }

        updateHandPlayerPosition = true;

    }

    public void UpdateHandEnemyPosition()
    {
        // TODO enemy card in hand
        positionHandEnemy = new Vector3[GameManager.instance.CurrentEnemy.enemyData.playerCards.Count];
        float spaceBetweenCards = cardEnemyPrefab.GetComponent<RectTransform>().rect.width / 15.0f;
        for (int i = 0; i < positionHandEnemy.Length; i++)
        {
            positionHandEnemy[i] = HandEnemy.transform.position + (i * cardEnemyPrefab.GetComponent<RectTransform>().rect.width * cardEnemyPrefab.GetComponent<RectTransform>().localScale.x * Vector3.left) + (i > 0 ? (spaceBetweenCards * Vector3.left*i) : Vector3.zero);
        }

        updateHandEnemyPosition = true;

    }

    public void PlayInitHandPlayerPosition()
    {

        timerPlayer += Time.deltaTime;
        for (int i = 0; i < HandPlayer.transform.childCount; i++)
        {
            HandPlayer.transform.GetChild(i).localPosition = Vector3.Lerp(HandPlayer.transform.GetChild(i).localPosition, positionHandPlayer[i], Mathf.Clamp(timerPlayer * 0.2f, 0, 1));
            if (Vector3.Distance(HandPlayer.transform.GetChild(HandPlayer.transform.childCount - 1).localPosition, positionHandPlayer[HandPlayer.transform.childCount - 1]) <= 0.2f )
            {
                updateHandPlayerPosition = false;
                for (int j = 0; j < HandPlayer.transform.childCount; j++)
                {
                    HandPlayer.transform.GetChild(j).GetComponent<CardInstance>().IsReady = true;
                }
                timerPlayer = 0.0f;
            }
        }
    }

    public void PlayInitHandEnemyPosition()
    {
        timerEnemy += Time.deltaTime;
        for (int i = 0; i < HandEnemy.transform.childCount; i++)
        {
            HandEnemy.transform.GetChild(i).localPosition = Vector3.Lerp(HandEnemy.transform.GetChild(i).localPosition, positionHandEnemy[i], Mathf.Clamp(timerEnemy * 0.2f, 0, 1));
            if (Vector3.Distance(HandEnemy.transform.GetChild(HandEnemy.transform.childCount - 1).localPosition, positionHandEnemy[HandEnemy.transform.childCount - 1]) <= 0.2f)
            {
                updateHandEnemyPosition = false;
                for (int j = 0; j < HandEnemy.transform.childCount; j++)
                {
                    HandEnemy.transform.GetChild(j).GetComponent<CardInstance>().IsReady = true;
                }
                timerEnemy = 0.0f;
            }
        }

    }

    public void InitTmpCardPlayedPosition()
    {
        positionCardPlayed = new Vector3[3];
        float spaceBetweenCards = cardPlayerPrefab.GetComponent<RectTransform>().rect.height / 15.0f;
        for (int i = 0; i < 3; i++)
        {
            positionCardPlayed[i] = CardPlayedInitialPosition.transform.position + (i * cardPlayerPrefab.GetComponent<RectTransform>().rect.height * cardPlayerPrefab.GetComponent<RectTransform>().localScale.x * Vector3.up) + (i > 0 ? (spaceBetweenCards * Vector3.up * i) : Vector3.zero);
        }
    }

    public void InitTmpCardPlayed(List<Card> tmpCardPlayed)
    {
        instance.tmpCardsPlayedInst.Clear();
        for (int i = 0; i < tmpCardPlayed.Count; i++)
        {
            GameObject instCard = Instantiate(cardPlayerPrefab, CardPlayedInitialPosition.gameObject.transform);
            instCard.transform.localPosition = Vector3.zero;
            //instCard.transform.localScale = Vector3.one;
            instCard.transform.SetParent(CardPlayedInitialPosition.transform);
            instCard.tag = "Untagged";
            // TODO : MUST DO A COPY CONSTRUCTOR
            instCard.GetComponent<CardInstance>().CardData = tmpCardPlayed[i];
            for (int j=0; j< 4; j++)
            {
                if (tmpCardPlayed[i].combinationValues[j] == instCard.GetComponent<CardInstance>().CardData.combinationPlayed)
                {
                    instCard.GetComponent<CardInstance>().gameObject.transform.GetChild(j).GetComponent<Outline>().effectColor = new Color(.42f, .97f, 0);
                }
           
            }
            instCard.GetComponent<CardInstance>().IsHidden = false;
            instCard.GetComponent<CardInstance>().RefreshValues();
            instance.tmpCardsPlayedInst.Add(instCard);
        }
        updateCardPlayedPosition = true;
    }

    public void PlayInitTmpCardPlayedPosition()
    {
        timerCardPlayed += Time.deltaTime;
        for (int i = 0; i < CardPlayedInitialPosition.transform.childCount; i++)
        {
            CardPlayedInitialPosition.transform.GetChild(i).localPosition = Vector3.Lerp(CardPlayedInitialPosition.transform.GetChild(i).localPosition, positionCardPlayed[i], Mathf.Clamp(timerCardPlayed * 0.2f, 0, 1));
            if (Vector3.Distance(CardPlayedInitialPosition.transform.GetChild(2).localPosition, positionCardPlayed[2]) <= 0.2f)
            {
                updateCardPlayedPosition = false;
                timerCardPlayed = 0.0f;
                resolutionUi = true;
            }
        }
    }

    public void DestroyTmpCardPlay()
    {
        for (int i = 0; i < instance.tmpCardsPlayedInst.Count; i++)
        {
            Destroy(instance.tmpCardsPlayedInst[i].gameObject);
            //instance.tmpCardsPlayedInst.RemoveAt(i);

        }

    }

    public void CardResolutionUI()
    {
        StartCoroutine(CardResotionCouroutine());
        resolutionUi = false;
    }

    public IEnumerator CardResotionCouroutine()
    {
        yield return new WaitForSeconds(1f);
        battleHandlerNeedNextTurn = true;
        yield return new WaitForSeconds(0.5f);

        processInBetweenTurn = false;
    }

    public void ChangeTurn(string text)
    {
        StartCoroutine(UIManager.instance.YourTurn(text));
    }

    public IEnumerator YourTurn(string text)
    {
        turnText.GetComponent<Text>().text = text +"'s Turn ...";
        turnText.transform.localPosition = turnTextOrigin;
        turnText.GetComponent<Text>().CrossFadeAlpha(1, 0, true);
        turnText.SetActive(true);
        moveTurnText = true;
        turnText.GetComponent<Text>().CrossFadeAlpha(0, 1, true);
        yield return new WaitForSeconds(2f);

    }

    public void MoveTurnText()
    {
        turnText.transform.position += Vector3.right  *0.2f* Time.deltaTime;

        if (Vector3.Distance(turnText.transform.position, turnTextArrival)< 0.2f)
        {

            moveTurnText = false;
            turnText.SetActive(false);
        }

    }

    void DamageCardAnimation(Player player, int cardIndex, bool destroyed)
    {
        if (player == GameManager.instance.CurrentPlayer.playerData)
        {
            // Add Animation instead of direct changes
            if (!destroyed)
            {
                HandPlayer.transform.GetChild(cardIndex).GetComponent<CardInstance>().RefreshValues();
            }
            else
            {
                DestroyImmediate(HandPlayer.transform.GetChild(cardIndex).gameObject);
                UpdateHandPlayerPosition();
            }
        }
        else if (player == GameManager.instance.CurrentEnemy.enemyData)
        {
            // Add Animation instead of direct changes
            if (!destroyed)
            {
                HandEnemy.transform.GetChild(cardIndex).GetComponent<CardInstance>().RefreshValues();
            }
            else
            {
                DestroyImmediate(HandEnemy.transform.GetChild(cardIndex).gameObject);
                UpdateHandEnemyPosition();
            }
        }
        else
        {
            Debug.LogError("Player not found");
            return;
        }

    }

    public void StartFightForPlayer()
    {

        processInBetweenTurn = true;
        List<Card> selectedCardData = new List<Card>();
        for (int i = 0; i < GameManager.instance.selectedCards.Count; i++)
        {
            GameManager.instance.selectedCards[i].CardData.combinationPlayed = GameManager.instance.dictionarySelectedCardsValues[GameManager.instance.selectedCards[i]];
            selectedCardData.Add(GameManager.instance.selectedCards[i].CardData);
        }
        BattleHandler.SendCardSelection(selectedCardData);

        // Deselecte
        for (int i = 0; i < GameManager.instance.selectedCards.Count; i++)
        {
            GameManager.instance.selectedCards[i].IsSelected = false;
            for (int j = 0; j < GameManager.instance.selectedCards[i].transform.childCount; j++)
            {
                GameManager.instance.selectedCards[i].transform.GetChild(j).GetComponent<Outline>().effectColor = Color.black;
            }

            GameManager.instance.selectedCards[i].transform.localPosition -= new Vector3(0, 60f, 0);
        }

        // Clear
        GameManager.instance.selectedCards.Clear();
        GameManager.instance.dictionarySelectedCardsValues.Clear();

        // Button fight
        GameManager.instance.ToogleButtonFight();

    }
}
