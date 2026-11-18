using DG.Tweening;
using System;
using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    public CoinManager coinManager;
    public UpgradeManager upgradeManager;
    public UpgradeUnit attackUU;
    public UpgradeUnit defenceUU;
    public UpgradeUnit lootUU;
    private CanvasGroup _canvasGroup;
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    public void Show()
    {
        gameObject.SetActive(true);
        _canvasGroup.DOFade(1f, .2f);
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        attackUU.SetUnitText(upgradeManager.attackUpgradeCount, 100);
        defenceUU.SetUnitText(upgradeManager.defenceUpgradeCount, 100);
        lootUU.SetUnitText(upgradeManager.lootUpgradeCount, 100);
        if (coinManager.totalCoinCount < 100)
        {
            attackUU.SetButtonInteractable(false);
            defenceUU.SetButtonInteractable(false);
            lootUU.SetButtonInteractable(false);
        }
        else
        {
            attackUU.SetButtonInteractable(true);
            defenceUU.SetButtonInteractable(true);
            lootUU.SetButtonInteractable(true);
        }
    }

    public void Hide()
    {
        _canvasGroup.DOFade(0f, 0.5f).OnComplete(() => gameObject.SetActive(false));

    }
    public void ExitButtonPressed()
    {
        Hide();
    }
    public void UpgradeAttackButtonPressed()
    {
        coinManager.SpendCoins(100);
        upgradeManager.UpgradeAttack();
        UpdateButtons();
    }
    public void UpgradeDefenceButtonPressed()
    {
        coinManager.SpendCoins(100);
        upgradeManager.UpgradeDefence();
        UpdateButtons();
    }
    public void UpgradeLootButtonPressed()
    {
        coinManager.SpendCoins(100);
        upgradeManager.UpgradeLoot();
        UpdateButtons();
    }   
}
