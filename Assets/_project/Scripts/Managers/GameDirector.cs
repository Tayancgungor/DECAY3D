using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameDirector : MonoBehaviour
{
    public  LevelManager levelManager;
    public FXManager fxManager;
    public AudioManager audioManager;
    public UIManager uIManager;
    public TimerManager timerManager;
    public Player player;

    public GameState gameState;
    private void Start()
    {
        uIManager.ShowMainMenu();
    }
    private void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            RestartLevel();
        }

        if (Keyboard.current.oKey.wasPressedThisFrame)
        {
            LoadNextLevel();
        }

        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            LoadPreviousLevel();
        }
    }

    private void LoadPreviousLevel()
    {
        levelManager.currentLevelNo--;
        if (levelManager.currentLevelNo < 1 ) 
        {
            levelManager.currentLevelNo = 1;
        }
        RestartLevel();
    }

    public void LoadNextLevel()
    {
        levelManager.currentLevelNo++;
        if (levelManager.currentLevelNo >= levelManager.levelPrefabs.Count)
        {
            levelManager.currentLevelNo = levelManager.levelPrefabs.Count;
        }
        RestartLevel();
    }

    public void RestartLevel()
    {
        gameState = GameState.Gameplay;
        levelManager.RestartLevelManager();
        player.RestartPlayer();
        audioManager.PlayAmbientSound();
        timerManager.StartTimer(levelManager.GetCurrentLevelTime());
        uIManager.ShowInGameUI();
    }

    public void LevelCompleted()
    {
        gameState = GameState.WinUI;
        audioManager.StopAmbientSound();
        audioManager.PlayVictoryAS();
        uIManager.ShowWinUI(3);
        uIManager.HideInGameUI();
    }

    public void LevelFailed()
    {
        levelManager.StopLevel();
        gameState = GameState.LoseUI;
        audioManager.StopAmbientSound();
        audioManager.PlayFailAS();
        uIManager.ShowFailUI(2);
        uIManager.HideInGameUI();

    }
}

public enum GameState
{
    MainMenu,
    Gameplay,
    WinUI,
    LoseUI,
    Inventory,
    EscapeMenu,

}
