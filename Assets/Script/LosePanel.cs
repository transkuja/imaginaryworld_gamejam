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
        UIManager.instance.HandEnemy.SetActive(true);
        UIManager.instance.HandPlayer.SetActive(true);
        UIManager.instance.AttDefEnemy.SetActive(true);
        UIManager.instance.AttDefPlayer.SetActive(true);
        UIManager.instance.ComboEnemy.SetActive(true);
        UIManager.instance.ComboPlayer.SetActive(true);
        gameObject.SetActive(false);
        SceneManager.LoadScene(1);
    }

    public void AddToDeck()
    {
        GameManager.instance.WinPanel.SetActive(false);
        SceneManager.LoadScene(1);
    }
}
