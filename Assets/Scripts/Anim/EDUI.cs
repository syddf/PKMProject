using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
public class EDUI : MonoBehaviour
{
    public int TotalFrame = 30;
    private int Counter;
    public CanvasGroup canvasGroup; // UI元素的CanvasGroup组件
    private float PrevAlpha;
    private float NextAlpha;
    public float TargetAlpha = 1.0f;
    private bool InTransition;
    
    public void Start()
    {
        canvasGroup.alpha = 0.0f;
        Counter = 0;
        InTransition = false;
    }

    public void FadeIn()
    {
        PrevAlpha = canvasGroup.alpha;
        NextAlpha = TargetAlpha;
        InTransition = true;
        Counter = 0;
    }

    public void FadeOut()
    {
        PrevAlpha = canvasGroup.alpha;
        NextAlpha = 0;
        InTransition = true;
        Counter = 0;
    }

    public void Update()
    {
        if(InTransition)
        {
            float Ratio = (float)Counter / (float)TotalFrame;
            float CurAlpha = PrevAlpha * (1.0f - Ratio) + NextAlpha * Ratio;
            canvasGroup.alpha = CurAlpha;
            Counter++;
            if(Counter > TotalFrame)
            {
                InTransition = false;
                canvasGroup.alpha = NextAlpha;
                Counter = 0;
            }
        }
    }

}
