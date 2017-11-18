﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public GameObject HandPlayer;
    public GameObject DeckPlayer;

    public GameObject HandEnemy;
    public GameObject DeckEnemy;

    public GameObject cardPlayerPrefab;
    public GameObject cardEnemyPrefab;

    public static UIManager instance;

    // Player Hand
    bool updateHandPlayerPosition = false;
    Vector3[] positionHandPlayer;
    float timerPlayer = 0.0f;

    // Enemy Hand
    bool updateHandEnemyPosition = false;
    Vector3[] positionHandEnemy;
    float timerEnemy = 0.0f;

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        InitHandPlayerPosition();
        InitHandEnemyPosition();
    }

    public void Update()
    {
        if (updateHandPlayerPosition)
            PlayInitHandPlayerPosition();

        if (updateHandEnemyPosition)
            PlayInitHandEnemyPosition();
    }

    public void PlayerInitHand(List<Card> handCards)
    {
        for (int i = 0; i < handCards.Count; i++)
        {
            GameObject instCard = Instantiate(cardPlayerPrefab, DeckPlayer.gameObject.transform);
            instCard.transform.localPosition = Vector3.zero;
            instCard.transform.localScale = Vector3.one;
            instCard.transform.parent = HandPlayer.transform;
            instCard.GetComponent<CardInstance>().cardData = handCards[i];
            instCard.GetComponent<CardInstance>().RefreshValues();
        }
    }

    public void EnemyInitHand(List<Card> handCards)
    {
        for (int i = 0; i < handCards.Count; i++)
        {
            GameObject instCard = Instantiate(cardEnemyPrefab, DeckEnemy.gameObject.transform);
            instCard.transform.localPosition = Vector3.zero;
            instCard.transform.localScale = Vector3.one;
            instCard.transform.parent = HandEnemy.transform;
            instCard.GetComponent<CardInstance>().cardData = handCards[i];
            instCard.GetComponent<CardInstance>().RefreshValues();
        }
    }


    public void InitHandPlayerPosition()
    {
        positionHandPlayer = new Vector3[Player.MAX_CARDS_IN_HAND];
        float spaceBetweenCards = cardPlayerPrefab.GetComponent<RectTransform>().rect.width / 7.0f;
        for (int i = 0; i < 4; i++)
        {
            positionHandPlayer[i] = HandPlayer.transform.position + (i * cardPlayerPrefab.GetComponent<RectTransform>().rect.width * Vector3.right) + (i > 0 ? (spaceBetweenCards * Vector3.right) : Vector3.zero);
        }

        updateHandPlayerPosition = true;

    }

    public void InitHandEnemyPosition()
    {
        // TODO enemy card in hand
        positionHandEnemy = new Vector3[Player.MAX_CARDS_IN_HAND];
        float spaceBetweenCards = cardEnemyPrefab.GetComponent<RectTransform>().rect.width / 7.0f;
        for (int i = 0; i < 4; i++)
        {
            positionHandEnemy[i] = HandEnemy.transform.position + (i * cardEnemyPrefab.GetComponent<RectTransform>().rect.width * Vector3.left) + (i > 0 ? (spaceBetweenCards * Vector3.left) : Vector3.zero);
        }

        updateHandEnemyPosition = true;

    }

    public void PlayInitHandPlayerPosition()
    {

        timerPlayer += Time.deltaTime;
        for (int i = 0; i < HandPlayer.transform.childCount; i++)
        {
            HandPlayer.transform.GetChild(i).localPosition = Vector3.Lerp(HandPlayer.transform.GetChild(i).localPosition, positionHandPlayer[i], Mathf.Clamp(timerPlayer * 0.2f, 0, 1));
            if (HandPlayer.transform.GetChild(HandPlayer.transform.childCount - 1).localPosition == positionHandPlayer[HandPlayer.transform.childCount - 1])
            {
                updateHandPlayerPosition = false;
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
            if (HandEnemy.transform.GetChild(HandEnemy.transform.childCount - 1).localPosition == positionHandEnemy[HandEnemy.transform.childCount - 1])
            {
                updateHandEnemyPosition = false;
                timerEnemy = 0.0f;
            }
        }

    }
}
