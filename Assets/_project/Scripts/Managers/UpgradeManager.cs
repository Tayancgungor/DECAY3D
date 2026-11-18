using UnityEngine;
using UnityEngine.InputSystem;

public class UpgradeManager : MonoBehaviour
{
    public int attackUpgradeCount;
    public int defenceUpgradeCount;
    public int lootUpgradeCount;


    public void SetStartingUpgrades(int attack, int defence, int loot)
    {
        attackUpgradeCount = attack;
        defenceUpgradeCount = defence;
        lootUpgradeCount = loot;
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
    public void UpgradeLoot()
    {
        lootUpgradeCount++;
        PlayerPrefs.SetInt("LootUpgradeCount", lootUpgradeCount);
    }
    private void Update()
    {
        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            UpgradeAttack();
        }
    }
}
