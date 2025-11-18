using UnityEngine;
using UnityEngine.UI;

public class ReloaderBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    private void Awake()
    {
        if (_slider == null)
            _slider = GetComponent<Slider>();

        // 시작할 땐 숨겨두기
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        SetProgress(0f);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 0 ~ 1 사이 값으로 채우기
    /// </summary>
    public void SetProgress(float normalized)
    {
        if (_slider == null) return;
        _slider.value = Mathf.Clamp01(normalized);
    }
}
