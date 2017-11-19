using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


public static class BattleHandler {
    public enum EntityTurn { Player, AI }
    public enum Combo { None, Suite, Pairs, AllTheSame, Size }
    enum BattleState { Win, Lose, Continue }
    private static EntityTurn currentTurn;
    static Player playerData;
    static Player enemyData;

    static List<Card> lastSelectionReceived;
    public static Combo comboReceived;

    static List<Card> initialPlayerDeckStatus;
    static List<Card> initialEnemyDeckStatus;
    static BattleState currentState = BattleState.Continue;

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
                    while(GameManager.instance.CurrentPlayer.playerData.playerCards.Count < Player.MAX_CARDS_IN_HAND && GameManager.instance.CurrentPlayer.playerData.playerDeck.Count > 0)
                    {
                        GameManager.instance.CurrentPlayer.DrawCard();
                        UIManager.instance.UpdateHandPlayerPosition();
                    }
                    break;
                case EntityTurn.AI:
                    UIManager.instance.ChangeTurn("Bob");
                    while (GameManager.instance.CurrentEnemy.enemyData.playerCards.Count < Player.MAX_CARDS_IN_HAND && GameManager.instance.CurrentEnemy.enemyData.playerDeck.Count > 0)
                    {
                        GameManager.instance.CurrentEnemy.DrawCard();
                        UIManager.instance.UpdateHandEnemyPosition();
                    }
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

        initialPlayerDeckStatus = new List<Card>();
        initialEnemyDeckStatus = new List<Card>();

        
        foreach (Card c in _playerData.playerDeck)
            initialPlayerDeckStatus.Add(Utils.CopyCard(c));
        foreach (Card c in _playerData.playerCards)
            initialPlayerDeckStatus.Add(Utils.CopyCard(c));

        foreach (Card c in _enemyData.playerDeck)
            initialEnemyDeckStatus.Add(Utils.CopyCard(c));

        foreach (Card c in _enemyData.playerCards)
            initialEnemyDeckStatus.Add(Utils.CopyCard(c));
        
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
        if (currentState != BattleState.Continue)
            return;

        currentState = CheckForBattleEnd();
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
        CurrentTurn = EntityTurn.Player;
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
        playerData.playerDeck = new List<Card>();
        foreach (Card c in initialPlayerDeckStatus)
            playerData.playerDeck.Add(Utils.CopyCard(c));

        GameObject.Find("PersistentPlayerData").GetComponent<PersistentPlayerData>().PlayerData = playerData;

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

    //public static Combo ComboAnalysis(List<Card> _cardsToAnalyzed)
    //{
    //    if (_cardsToAnalyzed == null || _cardsToAnalyzed.Count == 0)
    //    {
    //        Debug.LogWarning("Selection received is empty!!!!!");
    //        return Combo.None;
    //    }

    //    int[] combinationsPlayed = new int[_cardsToAnalyzed.Count];
    //    int i = 0;
    //    foreach (Card c in _cardsToAnalyzed)
    //    {
    //        combinationsPlayed[i] = c.combinationPlayed;
    //        i++;
    //    }

    //    bool isAnAscSuite = false;
    //    bool isADescSuite = false;
    //    bool isASuite = true;

    //    for (int j = 0; j < combinationsPlayed.Length - 1; j++)
    //    {
    //        if (combinationsPlayed[j + 1] == combinationsPlayed[j] + 1)
    //        {
    //            if (!isAnAscSuite && !isADescSuite)
    //                isAnAscSuite = true;

    //            if (isADescSuite) isASuite = false;
    //        }
    //        else if (combinationsPlayed[j + 1] == combinationsPlayed[j] - 1)
    //        {
    //            if (!isAnAscSuite && !isADescSuite)
    //                isADescSuite = true;

    //            if (isAnAscSuite) isASuite = false;
    //        }
    //        else
    //            isASuite = false;
    //    }

    //    combinationsPlayed = combinationsPlayed.OrderBy(j => j).ToArray();

    //    bool containsAPair = false;
    //    bool areAllTheSame = true;

    //    for (int j = 0; j < combinationsPlayed.Length - 1; j++)
    //    {
    //        if (combinationsPlayed[j] != combinationsPlayed[j + 1])
    //            areAllTheSame = false;
    //        else
    //            containsAPair = true;
    //    }

    //    if (areAllTheSame)
    //        return Combo.AllTheSame;
    //    else if (containsAPair)
    //        return Combo.Pairs;

    //    if (isASuite) return Combo.Suite;

    //    if (!isASuite && !containsAPair && !areAllTheSame)
    //        return Combo.None;
    //}

    static void ApplyCardEffects()
    {
        int rawDamage = 0;
        int rawHeal = 0;
        int rawDefense = 0;
        float comboMultiplier = GetMultiplierFromCombo(comboReceived);

        for (int i = 0; i < lastSelectionReceived.Count; i++)
        {
            if (lastSelectionReceived[i].GetType() == typeof(SwordCard))
                rawDamage += ((SwordCard)lastSelectionReceived[i]).damage;

            if (lastSelectionReceived[i].GetType() == typeof(ShieldCard))
                rawDefense += ((ShieldCard)lastSelectionReceived[i]).defenseValue;              

            if (lastSelectionReceived[i].GetType() == typeof(HealCard))
                rawHeal += ((HealCard)lastSelectionReceived[i]).healValue;

            if (lastSelectionReceived[i].GetType() == typeof(SwapCard))
            {
                if (i < lastSelectionReceived.Count - 1)
                {
                    if (lastSelectionReceived[i + 1].GetType() == typeof(SwordCard))
                    {
                        int value = ((SwordCard)lastSelectionReceived[i + 1]).damage;
                        lastSelectionReceived[i + 1] = new ShieldCard(lastSelectionReceived[i + 1].combinationValues[0], lastSelectionReceived[i + 1].combinationValues[1], lastSelectionReceived[i + 1].combinationValues[2], lastSelectionReceived[i + 1].combinationValues[3], lastSelectionReceived[i + 1].CardHealth);
                        ((ShieldCard)lastSelectionReceived[i + 1]).defenseValue = value;
                    }
                    if (lastSelectionReceived[i + 1].GetType() == typeof(ShieldCard))
                    {
                        int value = ((ShieldCard)lastSelectionReceived[i + 1]).defenseValue;
                        lastSelectionReceived[i + 1] = new SwordCard(lastSelectionReceived[i + 1].combinationValues[0], lastSelectionReceived[i + 1].combinationValues[1], lastSelectionReceived[i + 1].combinationValues[2], lastSelectionReceived[i + 1].combinationValues[3], lastSelectionReceived[i + 1].CardHealth);
                        ((SwordCard)lastSelectionReceived[i + 1]).damage = value;
                    }
                }
            }

            if (lastSelectionReceived[i].GetType() == typeof(ComboCard))
            {
                comboMultiplier = GetMultiplierFromCombo((Combo)Random.Range(0, (int)Combo.Size));
            }

        }

        int effectiveDamage = (int)(rawDamage * comboMultiplier);
        DamageOpponentCards(effectiveDamage);

        int effectiveDefense = (int)(rawDefense * comboMultiplier);
        if (CurrentTurn == EntityTurn.Player)
            playerData.currentTurnDefenseValue = effectiveDefense;
        else
            enemyData.currentTurnDefenseValue = effectiveDefense;

        int effectiveHeal = (int)(rawHeal * comboMultiplier);
        HealCards(effectiveHeal);


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

    static void HealCards(int _effectiveHeal)
    {
        if (CurrentTurn == EntityTurn.Player)
            playerData.HealCards(_effectiveHeal);
        else
            enemyData.HealCards(_effectiveHeal);
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
        if (playerData.playerCards.Count < 3 && playerData.playerDeck.Count == 0
            || (OnlyShieldsLeft(playerData.playerCards) && playerData.playerDeck.Count == 0))
            return BattleState.Lose;
        if (enemyData.playerCards.Count < 3 && enemyData.playerDeck.Count == 0
            || (OnlyShieldsLeft(enemyData.playerCards) && enemyData.playerDeck.Count == 0))
            return BattleState.Win;

        return BattleState.Continue;
    }

    static bool OnlyShieldsLeft(List<Card> _hand)
    {
        foreach (Card c in _hand)
            if (c.GetType() != typeof(ShieldCard))
                return false;
        return true;
    }

    static void WinProcess()
    {
        GameManager.instance.WinPanel.SetActive(true);
        UIManager.instance.HandPlayer.SetActive(false);
        UIManager.instance.HandEnemy.SetActive(false);

        List<Card> deckRebuild = new List<Card>();
        foreach (Card c in playerData.playerDeck)
            deckRebuild.Add(Utils.CopyCard(c));

        foreach (Card c in playerData.playerCards)
            deckRebuild.Add(Utils.CopyCard(c));
            
        List<Card> computedLoot = ComputeLoot();

        UIManager.instance.LootInit(computedLoot);
        UIManager.instance.UpdateLootPosition(computedLoot.Count);
        foreach (Card c in computedLoot)
            deckRebuild.Add(Utils.CopyCard(c));

        playerData.playerDeck = new List<Card>();
        foreach (Card c in deckRebuild)
            playerData.playerDeck.Add(Utils.CopyCard(c));

        GameObject.Find("PersistentPlayerData").GetComponent<PersistentPlayerData>().PlayerData = playerData;

        Reset();
    }

    static List<Card> ComputeLoot()
    {
        List<Card> returnLoot = new List<Card>();
        for (int i = 0; i < enemyData.lootQuantity; i++)
        {
            Card card = initialEnemyDeckStatus[Random.Range(0, initialEnemyDeckStatus.Count)];
            returnLoot.Add(Utils.CopyCard(card));
        }
        return returnLoot;
    }

    static void LoseProcess()
    {
        UIManager.instance.HandEnemy.SetActive(false);
        UIManager.instance.HandPlayer.SetActive(false);
        UIManager.instance.AttDefEnemy.SetActive(false);
        UIManager.instance.AttDefPlayer.SetActive(false);
        UIManager.instance.ComboEnemy.SetActive(false);
        UIManager.instance.ComboPlayer.SetActive(false);
        UIManager.instance.PlayerInfo.SetActive(false);
        UIManager.instance.EnemyInfo.SetActive(false);
        UIManager.instance.processInBetweenTurn = true;
        GameManager.instance.LosePanel.SetActive(true);

    }

    static void Reset()
    {
        lastSelectionReceived = new List<Card>();
        comboReceived = Combo.None;
        currentState = BattleState.Continue;
        playerData = null;
        enemyData = null;
    }
}
