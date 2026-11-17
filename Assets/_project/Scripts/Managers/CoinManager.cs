using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CoinManager : MonoBehaviour
{
    public int totalCoinCount;
    public CoinUI coinUI;
    public void SetStartingCoinCount(int v)
    {
        totalCoinCount = v;
        UpdateCoinUI();
    }

    private void Update()
    {
        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            CoinCollected(1);
        }
        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            SpendCoins(1);
        }
    }

    public void CoinCollected(int value)
    {
        totalCoinCount += value;
        UpdateCoinUI();
        PlayerPrefs.SetInt("CurrentCoinCount", totalCoinCount);
    }
    public void SpendCoins(int value)
    {
        totalCoinCount -= value;
        UpdateCoinUI();
        PlayerPrefs.SetInt("CurrentCoinCount", totalCoinCount);
    }
    public void UpdateCoinUI()
    {
        coinUI.SetCoinCount(totalCoinCount);
    }


}
