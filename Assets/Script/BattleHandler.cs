﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public static class BattleHandler {
    enum EntityTurn { Player, AI }
    enum Combo { None, Suite, Pairs, AllTheSame }
    static EntityTurn currentTurn;
    static Player playerData;
    static Player enemyData;

    static List<Card> lastSelectionReceived;
    static Combo comboReceived;

    static void Init()
    {
        currentTurn = (EntityTurn)Random.Range(0, 2);
    }

    public static void StartBattle(Player _playerData, Player _enemyData)
    {
        playerData = _playerData;
        enemyData = _enemyData;

        Init();
    }

    // Use this method to send the cards the player/AI selected
    public static void SendCardSelection(List<Card> _cardSelection)
    {
        lastSelectionReceived = _cardSelection;
    }

    static void CardResolution()
    {
        ComboAnalysis();
        ApplyCardEffects();
    }

    static void ComboAnalysis()
    {
        if (lastSelectionReceived == null || lastSelectionReceived.Count == 0)
        {
            Debug.LogWarning("Selection received is empty!!!!!");
            return;
        }

        int[] combinationsPlayed = new int[lastSelectionReceived.Count];
        int i = 0;
        foreach (Card c in lastSelectionReceived)
        {
            combinationsPlayed[i] = c.combinationPlayed;
            i++;
        }

        combinationsPlayed = combinationsPlayed.OrderBy(j => j).ToArray();
        bool isASuite = true;
        bool containsAPair = false;
        bool areAllTheSame = true;

        for (int j = 0; j < combinationsPlayed.Length - 1; j++)
        {
            if (combinationsPlayed[j] != combinationsPlayed[j + 1])
            {
                areAllTheSame = false;
                if (combinationsPlayed[j] != combinationsPlayed[j + 1] - 1)
                {
                    isASuite = false;
                }
            }
            else
                containsAPair = true;
        }

        if (areAllTheSame)
            comboReceived = Combo.AllTheSame;
        else if (containsAPair)
            comboReceived = Combo.Pairs;

        if (isASuite) comboReceived = Combo.Suite;

        if (!isASuite && !containsAPair && !areAllTheSame)
            comboReceived = Combo.None;
    }

    static void ApplyCardEffects()
    {
        int rawDamage = 0;
        foreach (Card c in lastSelectionReceived)
        {
            if (c.GetType() == typeof(SwordCard))
                rawDamage += ((SwordCard)c).damage;

            if (c.GetType() == typeof(ShieldCard))
            {
                if (currentTurn == EntityTurn.Player)
                    playerData.currentTurnDefenseValue += ((ShieldCard)c).defenseValue;
                else
                    enemyData.currentTurnDefenseValue += ((ShieldCard)c).defenseValue;
            }
        }

        DamageOpponentCards(rawDamage);
    }

    static void DamageOpponentCards(int _rawDamage)
    {
        int effectiveDamage = (int)(_rawDamage * GetDamageMultiplierFromCombo(comboReceived));

        if (currentTurn == EntityTurn.Player)
            enemyData.TakeDamage(effectiveDamage);
        else
            playerData.TakeDamage(effectiveDamage);
    }

    static float GetDamageMultiplierFromCombo(Combo _combo)
    {
        switch (_combo)
        {
            case Combo.Pairs:
                return 1.2f;
            case Combo.AllTheSame:
                return 2.5f;
            case Combo.Suite:
                return 2.0f;
            case Combo.None:
                return 1.0f;
        }

        return 1.0f;
    }
}