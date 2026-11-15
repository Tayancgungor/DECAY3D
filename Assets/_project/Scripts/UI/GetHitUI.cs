using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GetHitUI : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    public Image gradientImage;
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    public void Show()
    {
        gameObject.SetActive(true);
        _canvasGroup.DOFade(1f, .2f);
    }
    public void Hide()
    {
        _canvasGroup.DOFade(0f, 0.5f).OnComplete(() => gameObject.SetActive(false));

    }
    public void ShowGetHitGradient()
    {
        gradientImage.DOKill();
        gradientImage.color = new Color(1f, 0f, 0f, 0f);
        gradientImage.DOFade(.15f, .1f).SetLoops(2, LoopType.Yoyo);
    }
}
