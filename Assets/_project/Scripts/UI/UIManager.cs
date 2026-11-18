using System;
using System.Reflection;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public GameDirector gameDirector;
    public MainMenu mainMenu;
    public WinUI winUI;
    public FailUI failUI;
    public EscapeMenu escapeMenu;
    public TimerUI timerUI;
    public GetHitUI getHitUI;
    public MessageUI messageUI;
    public InventoryUI inventoryUI;
    public GrenadeUI grenadeUI;
    public CoinUI coinUI;
    public UpgradeUI upgradeUI;


    public void ShowMainMenu()
    {
        gameDirector.gameState = GameState.MainMenu;
        mainMenu.Show();
        winUI.Hide();
        failUI.Hide();
        escapeMenu.Hide();
        coinUI.Show();
        upgradeUI.Hide();
        HideInGameUI();
    }
    private void Update()
    {
        if (Keyboard.current.escapeKey.isPressed)
        {
            ShowEscapeMenu();
        }
    }
    public void ShowEscapeMenu()
    {
        Time.timeScale = 0;
        escapeMenu.Show(0);
        gameDirector.gameState = GameState.EscapeMenu;
    }
    public void ShowWinUI(float delay)
    {
        winUI.Show(delay);
    }
    public void ShowFailUI(float delay)
    {
        failUI.Show(delay);
    }
    public void ShowInGameUI() 
    {
        timerUI.Show();
        getHitUI.Show();
        messageUI.Show();
        inventoryUI.Show();
        grenadeUI.Show();
    }
    public void HideInGameUI()
    {
        timerUI.Hide();
        getHitUI.Hide();
        messageUI.Hide();
        inventoryUI.Hide();
        grenadeUI.Hide();
    }

    // Button Callbacks

    public void ResumeButtonPressed()
    {
        Time.timeScale = 1;
        escapeMenu.Hide();
        Invoke(nameof(ChangeGameStateToGameplay), 0.1f);
    }
    void ChangeGameStateToGameplay()
    {
        gameDirector.gameState = GameState.Gameplay;
    }
    public void MainMenuButtonPressed()
    {
        Time.timeScale = 1;
        escapeMenu.Hide();
        gameDirector.gameState = GameState.MainMenu;
        ShowMainMenu();
    }
    public void UpgradeButtonPressed()
    {
        upgradeUI.Show();
    }
    public void StartGameButtonPressed()
    {
        mainMenu.Hide();
        gameDirector.RestartLevel();
    }
    public void QuitButtonPressed()
    {
        Application.Quit();
    }
    public void LoadNextLevelButtonPressed()
    {
        winUI.Hide();
        gameDirector.LoadNextLevel();
        upgradeUI.Hide();

    }
    public void RetryButtonPressed()
    {
        failUI.Hide();
        gameDirector.RestartLevel();
        upgradeUI.Hide();
    }

}
