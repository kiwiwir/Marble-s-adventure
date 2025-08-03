using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScrollerAnim : MonoBehaviour
{
public RectTransform background;
    public float fromX = -1500f;
    public float toX_FirstFive = 3000f;
    public float toX_Final = 1500f;

    public float totalTimeFirstFive = 16.5f;
    public int firstFiveLoops = 5;

    private void Start()
    {
        StartCoroutine(RunFenceScroll());
    }

    private IEnumerator RunFenceScroll()
    {
        // --- ETAP 1: 5 przelotów w 16.5s, coraz wolniej ---
        float totalTime = totalTimeFirstFive;
        float baseDuration = totalTime / SumFirstN(firstFiveLoops); // coraz dłuższy czas na każdy przelot

        for (int i = 1; i <= firstFiveLoops; i++)
        {
            float duration = baseDuration * i;
            yield return StartCoroutine(MoveOnce(fromX, toX_FirstFive, duration));
            background.anchoredPosition = new Vector2(fromX, background.anchoredPosition.y);
        }

        // --- ETAP 2: 6. ostatni przelot
        yield return StartCoroutine(MoveOnce(fromX, toX_Final, baseDuration * (firstFiveLoops + 1), true));
        // Po tym ruchu sprite zostaje w miejscu.
    }

    private IEnumerator MoveOnce(float startX, float endX, float duration, bool slowDown = false)
    {
        float elapsed = 0f;
        Vector2 start = new Vector2(startX, background.anchoredPosition.y);
        Vector2 end = new Vector2(endX, background.anchoredPosition.y);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            
            if (slowDown)
            {
                // Efekt zwalniania – EaseOutQuad
                t = 1 - Mathf.Pow(1 - t, 2);
            }

            background.anchoredPosition = Vector2.Lerp(start, end, t);
            yield return null;
        }
    }

    private int SumFirstN(int n)
    {
        // Suma 1 + 2 + 3 + ... + n = n(n+1)/2
        return n * (n + 1) / 2;
    }
}
