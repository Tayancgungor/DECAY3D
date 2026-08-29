using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameDirector : MonoBehaviour
{
    public  LevelManager levelManager;
    public Player player;

    private void Start()
    {
        RestartLevel();
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

    private void LoadNextLevel()
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
        levelManager.RestartLevelManager();
        player.RestartPlayer();
    }
}
