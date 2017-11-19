using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LosePanel : MonoBehaviour {

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Retry()
    {
        BattleHandler.RestoreDecks();
        UIManager.instance.processInBetweenTurn = false;
        gameObject.SetActive(false);
    }
}
