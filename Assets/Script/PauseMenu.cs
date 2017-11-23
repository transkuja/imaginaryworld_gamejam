using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public void BackToMenu()
    {
        Time.timeScale = 1.0f;
        BattleHandler.RestoreDecks();
        UIManager.instance.processInBetweenTurn = false;
        UIManager.instance.HandEnemy.SetActive(true);
        UIManager.instance.HandPlayer.SetActive(true);

        SceneManager.LoadScene(0);
    }

    public void Retry()
    {
        Time.timeScale = 1.0f;
        BattleHandler.RestoreDecks();
        UIManager.instance.processInBetweenTurn = false;
        UIManager.instance.HandEnemy.SetActive(true);
        UIManager.instance.HandPlayer.SetActive(true);
        //UIManager.instance.AttDefEnemy.SetActive(true);
        //UIManager.instance.AttDefPlayer.SetActive(true);
        //UIManager.instance.ComboEnemy.SetActive(true);
        //UIManager.instance.ComboPlayer.SetActive(true);
        gameObject.SetActive(false);
        SceneManager.LoadScene(1);
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
        gameObject.SetActive(false);
    }
}
