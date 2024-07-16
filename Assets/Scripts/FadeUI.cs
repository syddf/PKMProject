using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEngine.UI;

public class FadeUI : MonoBehaviour
{
    public float fadeDuration = 0.5f; // 淡入淡出的持续时间
    public CanvasGroup canvasGroup; // UI元素的CanvasGroup组件

    private Coroutine fadeCoroutine; // 用于存储淡入淡出协程的引用
    public bool AutoFadeIn = true;
    public void Start()
    {
        canvasGroup.alpha = 0.0f;
    }
    // 在启用时执行淡入效果
    private void OnEnable()
    {
        if(AutoFadeIn)
        {
            canvasGroup.alpha = 0.0f;
            FadeIn();
        }
    }

    public void TransitionAnimation()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(TransitionAnim(canvasGroup));
    }

    // 淡入UI
    public void FadeIn()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(Fade(canvasGroup, canvasGroup.alpha, 1f, false));
    }

    // 淡出UI
    public void FadeOutAndDisable()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(Fade(canvasGroup, canvasGroup.alpha, 0f, true));
    }

    // 实现淡入淡出的协程
    private IEnumerator Fade(CanvasGroup canvasGroup, float startAlpha, float endAlpha, bool disableAfterFade)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = endAlpha;

        // 如果需要，在淡出完成后禁用UI元素
        if (disableAfterFade && endAlpha == 0f)
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator TransitionAnim(CanvasGroup canvasGroup)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0.0f, 1.0f, elapsedTime / fadeDuration);
            yield return null;
        }
        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1.0f, 0.0f, elapsedTime / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0.0f;
    }
}