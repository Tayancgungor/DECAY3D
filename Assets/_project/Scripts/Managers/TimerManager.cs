using System;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public GameDirector gameDirector;
    public TimerUI timerUI;
    public Player player;
    private float _levelTime;
    private float _totalLevelTime;

    public void StartTimer(float levelTime)
    {
        _levelTime = levelTime;
        _totalLevelTime = levelTime;
    }

    private void Update()
    {
        if (gameDirector.gameState  != GameState.Gameplay)
        {
            return;
        }
        _levelTime -= Time.deltaTime;
        timerUI.SetFillBar(_levelTime, _totalLevelTime);

        if (_levelTime <=0)
        {
            player.PlayAlternativeFailAnimation();
            gameDirector.LevelFailed();
        }
    }
}
