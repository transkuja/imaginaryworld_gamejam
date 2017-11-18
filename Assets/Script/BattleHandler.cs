using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public static class BattleHandler {
    public enum EntityTurn { Player, AI }
    enum Combo { None, Suite, Pairs, AllTheSame }
    enum BattleState { Win, Lose, Continue }
    public static EntityTurn currentTurn;
    static Player playerData;
    static Player enemyData;

    static List<Card> lastSelectionReceived;
    static Combo comboReceived;

    // Use this method at the beginning of a battle to initialize the battle handler
    public static void StartBattle(Player _playerData, Player _enemyData)
    {
        Init();
        playerData = _playerData;
        enemyData = _enemyData;

        // ask the AI for the card selection
        if (currentTurn == EntityTurn.AI)
            Debug.Log("AI is playing ...");
        // ask the player
        else
            Debug.Log("Player is playing ...");

    }

    // Use this method to send the cards the player/AI selected
    public static void SendCardSelection(List<Card> _cardSelection)
    {
        lastSelectionReceived = _cardSelection;
        CardResolution();
    }


    // This function should be call when we want the turn to change
    public static void NextTurn()
    {
        BattleState currentState = CheckForBattleEnd();
        if (currentState == BattleState.Continue)
        {
            if (currentTurn == EntityTurn.AI)
                currentTurn = EntityTurn.Player;
            else
                currentTurn = EntityTurn.AI;
        }
        else if (currentState == BattleState.Win)
        {
            WinProcess();
        }
        else if (currentState == BattleState.Lose)
        {
            LoseProcess();
        }
    }

    static void Init()
    {
        currentTurn = (EntityTurn)Random.Range(0, 2);
        lastSelectionReceived = new List<Card>();
        comboReceived = Combo.None;
        playerData = null;
        enemyData = null;
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

        bool isASuite = true;
        for (int j = 0; j < combinationsPlayed.Length - 1; j++)
        {
            if (combinationsPlayed[j] != combinationsPlayed[j + 1])
            {
                if (combinationsPlayed[j] != combinationsPlayed[j + 1] - 1)
                    isASuite = false;
            }
        }

        combinationsPlayed = combinationsPlayed.OrderBy(j => j).ToArray();

        bool containsAPair = false;
        bool areAllTheSame = true;

        for (int j = 0; j < combinationsPlayed.Length - 1; j++)
        {
            if (combinationsPlayed[j] != combinationsPlayed[j + 1])
                areAllTheSame = false;
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

    static BattleState CheckForBattleEnd()
    {
        if (playerData.playerCards.Count == 0)
            return BattleState.Lose;
        if (enemyData.playerCards.Count == 0)
            return BattleState.Win;

        return BattleState.Continue;
    }

    static void WinProcess()
    {
        // TODO

        Reset();
    }

    static void LoseProcess()
    {
        // TODO

        Reset();
    }

    static void Reset()
    {
        lastSelectionReceived = new List<Card>();
        comboReceived = Combo.None;
        playerData = null;
        enemyData = null;
    }
}
