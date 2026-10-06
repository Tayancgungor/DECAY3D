using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    public TimerManager timerManager;
    private CanvasGroup _canvasGroup;
    public Image fillBar;
    public TextMeshProUGUI timerTMP;
    public TextMeshProUGUI urgentTMP;
    private float _previousRemainingTime;

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
    public void SetFillBar(float remainingTime, float totalTime)
    {
        fillBar.fillAmount = remainingTime / totalTime;
        timerTMP.text = Mathf.Round(remainingTime).ToString();
        if (Mathf.Round(remainingTime) < 6)
        {
            urgentTMP.gameObject.SetActive(true);
            timerTMP.gameObject.SetActive(false);
        }
        else
        {
            urgentTMP.gameObject.SetActive(false);
            timerTMP.gameObject.SetActive(true);
        }
        urgentTMP.text = Mathf.Round(remainingTime).ToString();
        if (Mathf.Round(remainingTime) != _previousRemainingTime)
        {

            if (Mathf.Round(remainingTime) < 6) 
            {
                urgentTMP.transform.localScale = Vector3.one;
                urgentTMP.transform.DOScale(Vector3.one * 1.5f, 0.2f).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    urgentTMP.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InBack);
                });
                timerManager.gameDirector.audioManager.PlayTimeTickAS();

            }
            _previousRemainingTime = Mathf.Round(remainingTime);
        }
    }
}


