using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TileManager[] tileManager; 
    public static GameManager instance;
    public static Action onGameStarted;
    private bool isGameStarted;
    private float currentTimeScale;
    private int score;
    private int money;
    private int lastLoadedLevel;
    private void Awake()
    {
        instance = this;
        currentTimeScale = Time.timeScale;
        if (PlayerPrefs.HasKey("Money"))
        {
            money = PlayerPrefs.GetInt("Money");
        }
        else
        {
            PlayerPrefs.SetInt("Money", 0);
            PlayerPrefs.Save();
        }
        
    }
    private void Start()
    {
        UIManager.instance.ShowMoney(money.ToString());
    }
    private void Update()
    {
        if (isGameStarted)
        {
            score += 1;
            UIManager.instance.ShowScore(score.ToString());
        }
    }
    public void StartGame(int level=0)
    {
        isGameStarted = true;
        onGameStarted?.Invoke();
        Time.timeScale = 1f;
        
        if (level==0)
        {
            Instantiate(tileManager[lastLoadedLevel - 1]);
        }
        else
        {
            Instantiate(tileManager[level - 1]);
            lastLoadedLevel = level;
        }
        
    }
    public void PauseGame()
    {
        isGameStarted = false;
        Time.timeScale = 0f;
    }
    public void UnPauseGame()
    {
        isGameStarted = true;
        Time.timeScale = currentTimeScale;
    }
    public void EndGame(bool isWin = false)
    {
        isGameStarted = false;
        CheckBestScore();
        UIManager.instance.EndGame(isWin);
    }
    private void CheckBestScore()
    {
        if (PlayerPrefs.HasKey("BestScore"))
        {
            int tempBestScore = PlayerPrefs.GetInt("BestScore");
            if (tempBestScore > score)
            {
                UIManager.instance.ShowBestScore(tempBestScore.ToString());
            }
            else
            {
                UIManager.instance.ShowBestScore(score.ToString());
                PlayerPrefs.SetInt("BestScore", score);
                PlayerPrefs.Save();
            }
        }
        else
        {
            UIManager.instance.ShowBestScore(score.ToString());
            PlayerPrefs.SetInt("BestScore", score);
            PlayerPrefs.Save();
        }
    }
    public bool IsGameStarted()
    {
        return isGameStarted;
    }
}
