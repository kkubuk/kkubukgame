using System.Collections;
using UnityEngine;

public class SetWindowController : MonoBehaviour
{
    public RectTransform setWindow; // 설정창
    public float duration = 0.5f;   // 슬라이드 시간
    private bool isOpen = false;

    private Vector2 hiddenPos = new Vector2(0f, -1080);  // 아래 화면 밖
    private Vector2 visiblePos = new Vector2(0f, 0);    // 화면 안쪽

    void Start()
    {
        setWindow.anchoredPosition = hiddenPos;
    }

    public void OpenWindow()
    {
        if (!isOpen)
            StartCoroutine(SlideWindow(setWindow.anchoredPosition, visiblePos));
        isOpen = true;
    }

    public void CloseWindow()
    {
        if (isOpen)
            StartCoroutine(SlideWindow(setWindow.anchoredPosition, hiddenPos));
        isOpen = false;
    }

    IEnumerator SlideWindow(Vector2 from, Vector2 to)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            setWindow.anchoredPosition = Vector2.Lerp(from, to, t);
            yield return null;
        }
        setWindow.anchoredPosition = to;
    }
}
