using DG.Tweening;
using UnityEngine;

public class EscapeMenu : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    public void Show(float delay)
    {
        gameObject.SetActive(true);
        _canvasGroup.DOFade(1f, .2f).SetDelay(delay).SetUpdate(true);
    }
    public void Hide()
    {
        _canvasGroup.DOFade(0f, 0.5f).OnComplete(() => gameObject.SetActive(false));

    }
}
