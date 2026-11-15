using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public UIManager uiManager;
    private bool _isInventoryOpen;
    private CanvasGroup _canvasGroup;
    public List<Button> items;

    private const float ClosedY = 200f;
    private const float ItemSpacing = 160f;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        CloseInventory(true);
    }

    private void Update()
    {
        if (uiManager.gameDirector.gameState == GameState.Gameplay && Keyboard.current.iKey.wasPressedThisFrame)
        {
            InventoryButtonPressed();
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
        _canvasGroup.DOKill();
        CloseInventory(true);
        _canvasGroup.DOFade(1f, .2f);
    }

    public void Hide()
    {
        _canvasGroup.DOKill();
        _canvasGroup.DOFade(0f, 0.5f).OnComplete(() => gameObject.SetActive(false));
    }

    public void InventoryButtonPressed()
    {
        if (_isInventoryOpen)
        {
            CloseInventory(false);
        }
        else
        {
            OpenInventory();
        }
    }

    private void OpenInventory()
    {
        _isInventoryOpen = true;

        for (int i = 0; i < items.Count; i++)
        {
            items[i].interactable = true;
            var rectTransform = items[i].GetComponent<RectTransform>();
            rectTransform.DOKill();
            rectTransform.DOAnchorPos3DY(ClosedY - (i + 1) * ItemSpacing, .2f);
            FadeItem(items[i], 1f, .2f);
        }
    }

    private void CloseInventory(bool instant)
    {
        _isInventoryOpen = false;

        for (int i = 0; i < items.Count; i++)
        {
            items[i].interactable = false;
            var rectTransform = items[i].GetComponent<RectTransform>();
            rectTransform.DOKill();

            if (instant)
            {
                var pos = rectTransform.anchoredPosition3D;
                pos.y = ClosedY;
                rectTransform.anchoredPosition3D = pos;
            }
            else
            {
                rectTransform.DOAnchorPos3DY(ClosedY, .2f);
            }

            FadeItem(items[i], 0f, instant ? 0f : .2f);
        }
    }

    private void FadeItem(Button item, float alpha, float duration)
    {
        foreach (var graphic in item.GetComponentsInChildren<Graphic>(true))
        {
            graphic.DOKill();

            if (duration <= 0f)
            {
                var color = graphic.color;
                color.a = alpha;
                graphic.color = color;
            }
            else
            {
                graphic.DOFade(alpha, duration);
            }
        }
    }

    public void MachineGunButtonPressed()
    {
        uiManager.gameDirector.player.SwitchToMachineGun();
        CloseInventory(false);
    }

    public void ShotgunButtonPressed()
    {
        uiManager.gameDirector.player.SwitchToShotgun();
        CloseInventory(false);
    }
}
