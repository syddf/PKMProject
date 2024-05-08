using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIFadeOut : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    private Coroutine fadeOutCoroutine;

    // 在调用该方法时执行UI的淡出效果
    public void FadeOutUI(float duration)
    {
        this.gameObject.SetActive(true);
        if (fadeOutCoroutine != null)
        {
            // 如果之前有淡出协程在运行，先停止它
            StopCoroutine(fadeOutCoroutine);
        }

        // 重置CanvasGroup的alpha值为1
        canvasGroup.alpha = 1f;

        // 开始新的淡出协程
        fadeOutCoroutine = StartCoroutine(FadeOut(canvasGroup, duration));
    }

    IEnumerator FadeOut(CanvasGroup canvasGroup, float duration)
    {
        float startAlpha = canvasGroup.alpha;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, timeElapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = 0f; // 确保最终Alpha值为0
    }
}