using UnityEngine;
using UnityEngine.InputSystem;

public class UpgradeManager : MonoBehaviour
{
    public int attackUpgradeCount;
    public int defenceUpgradeCount;
    public int coinUpgradeCount;


    public void SetStartingUpgrades(int attack, int defence, int coin)
    {
        attackUpgradeCount = attack;
        defenceUpgradeCount = defence;
        coinUpgradeCount = coin;
    }
    public void UpgradeAttack()
    {
        attackUpgradeCount++;
        PlayerPrefs.SetInt("AttackUpgradeCount", attackUpgradeCount);
    }
    public void UpgradeDefence()
    {
        defenceUpgradeCount++;
        PlayerPrefs.SetInt("DefenceUpgradeCount", defenceUpgradeCount);
    }
    public void UpgradeCoin()
    {
        coinUpgradeCount++;
        PlayerPrefs.SetInt("CoinUpgradeCount", coinUpgradeCount);
    }
    private void Update()
    {
        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            UpgradeAttack();
        }
    }
}
