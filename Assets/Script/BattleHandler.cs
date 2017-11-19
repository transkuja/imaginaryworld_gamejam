using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


public static class BattleHandler {
    public enum EntityTurn { Player, AI }
    public enum Combo { None, Suite, Pairs, AllTheSame }
    enum BattleState { Win, Lose, Continue }
    private static EntityTurn currentTurn;
    static Player playerData;
    static Player enemyData;

    static List<Card> lastSelectionReceived;
    public static Combo comboReceived;

    static List<Card> initialPlayerDeckStatus;
    static List<Card> initialEnemyDeckStatus;

    public static EntityTurn CurrentTurn
    {
        get
        {
            return currentTurn;
        }

        set
        {
            switch(value)
            {
                case EntityTurn.Player:
                    UIManager.instance.ChangeTurn("Player");
                    break;
                case EntityTurn.AI:
                    UIManager.instance.ChangeTurn("Bob");
                    break;

            }
            currentTurn = value;
        }
    }

    // Use this method at the beginning of a battle to initialize the battle handler
    public static void StartBattle(Player _playerData, Player _enemyData)
    {
        Init();
        playerData = _playerData;
        enemyData = _enemyData;

        initialPlayerDeckStatus = new List<Card>(_playerData.playerDeck);
        initialEnemyDeckStatus = new List<Card>(_enemyData.playerDeck);

        // ask the AI for the card selection
        if (CurrentTurn == EntityTurn.AI)
            Debug.Log("AI is playing ...");
        // ask the player
        else
            Debug.Log("Player is playing ...");

    }

    // Use this method to send the cards the player/AI selected
    public static void SendCardSelection(List<Card> _cardSelection)
    {
        lastSelectionReceived = _cardSelection;
        UIManager.instance.InitTmpCardPlayed(lastSelectionReceived);
        CardResolution();
    }


    // This function should be call when we want the turn to change
    public static void NextTurn()
    {
        BattleState currentState = CheckForBattleEnd();
        if (currentState == BattleState.Continue)
        {
            if (CurrentTurn == EntityTurn.AI)
            {
                playerData.currentTurnDefenseValue = 0;
                CurrentTurn = EntityTurn.Player;
            }
            else
            {
                enemyData.currentTurnDefenseValue = 0;
                CurrentTurn = EntityTurn.AI;
            }
          
        }
        else if (currentState == BattleState.Win)
        {
            WinProcess();
        }
        else if (currentState == BattleState.Lose)
        {
            LoseProcess();
        }

        UIManager.instance.DestroyTmpCardPlay();
        GameManager.instance.handled = false;

    }

    static void Init()
    {
        CurrentTurn = (EntityTurn)Random.Range(0, 2);
        lastSelectionReceived = new List<Card>();
        comboReceived = Combo.None;
        playerData = null;
        enemyData = null;
    }

    public static void CardResolution()
    {
        ComboAnalysis();
        ApplyCardEffects();
    }

    public static void RestoreDecks()
    {
        playerData.playerDeck = initialPlayerDeckStatus;
        enemyData.playerDeck = initialEnemyDeckStatus;

        Reset();
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

        bool isAnAscSuite = false;
        bool isADescSuite = false;
        bool isASuite = true;

        for (int j = 0; j < combinationsPlayed.Length - 1; j++)
        {
            if (combinationsPlayed[j + 1] == combinationsPlayed[j] + 1)
            {
                if (!isAnAscSuite && !isADescSuite)
                    isAnAscSuite = true;

                if (isADescSuite) isASuite = false;
            }
            else if (combinationsPlayed[j + 1] == combinationsPlayed[j] - 1)
            {
                if (!isAnAscSuite && !isADescSuite)
                    isADescSuite = true;

                if (isAnAscSuite) isASuite = false;
            }
            else
                isASuite = false;
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
        if(CurrentTurn == EntityTurn.Player)
            UIManager.instance.RefreshComboPlayer(comboReceived);
        else
            UIManager.instance.RefreshComboEnemy(comboReceived);

    }

    static void ApplyCardEffects()
    {
        int rawDamage = 0;
        float comboMultiplier = GetMultiplierFromCombo(comboReceived);

        foreach (Card c in lastSelectionReceived)
        {
            if (c.GetType() == typeof(SwordCard))
                rawDamage += ((SwordCard)c).damage;

            if (c.GetType() == typeof(ShieldCard))
            {
                if (CurrentTurn == EntityTurn.Player)
                    playerData.currentTurnDefenseValue += (int)(((ShieldCard)c).defenseValue * comboMultiplier);              
                else
                    enemyData.currentTurnDefenseValue += (int)(((ShieldCard)c).defenseValue * comboMultiplier);
            }
        }
        int effectiveDamage = (int)(rawDamage * comboMultiplier);
        DamageOpponentCards(effectiveDamage);

        if (CurrentTurn == EntityTurn.Player)
            UIManager.instance.RefreshPlayerInfo(effectiveDamage, playerData.currentTurnDefenseValue);
        else
            UIManager.instance.RefreshEnemyInfo(effectiveDamage, enemyData.currentTurnDefenseValue);

    }

    static void DamageOpponentCards(int _effectiveDamage)
    {
        if (CurrentTurn == EntityTurn.Player)
            enemyData.TakeDamage(_effectiveDamage);
        else
            playerData.TakeDamage(_effectiveDamage);
    }

    static float GetMultiplierFromCombo(Combo _combo)
    {
        switch (_combo)
        {
            case Combo.Pairs:
                return 1.5f;
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
        // show loot on win panel
        // deck building panel here (add card from loot or not, remove card from deck or not)
        List<Card> loot = ComputeLoot();

        Reset();
    }

    static List<Card> ComputeLoot()
    {
        List<Card> returnLoot = new List<Card>();
        for (int i = 0; i < enemyData.lootQuantity; i++)
        {
            returnLoot.Add(initialEnemyDeckStatus[Random.Range(0, initialEnemyDeckStatus.Count)]);
        }
        return returnLoot;
    }

    static void LoseProcess()
    {
        UIManager.instance.processInBetweenTurn = true;
        GameManager.instance.LosePanel.SetActive(true);
    }

    static void Reset()
    {
        lastSelectionReceived = new List<Card>();
        comboReceived = Combo.None;
        playerData = null;
        enemyData = null;
    }
}
