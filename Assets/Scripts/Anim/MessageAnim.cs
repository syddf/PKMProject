using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
public class MessageAnim : MonoBehaviour
{
    public GameObject HideWhenMessageUI;
    public string MessageText;
    public TextMeshProUGUI TextObj;
    public float Duration = 1f; // 完成移动所需的时间，假设游戏以60fps运行，则30帧相当于0.5秒
    private float ElapsedTime = 0; // 已过时间

    void Start()
    {
        
    }

    void OnEnable() 
    {
        ElapsedTime = 0;
        HideWhenMessageUI.SetActive(false);
    }

    void Update()
    {
        ElapsedTime += Time.deltaTime; // 更新已过时间
        float fraction = ElapsedTime / Duration; // 计算已完成的动画比例

        if (fraction <= 1) // 确保fraction不会超过1
        {
            int StringLength = (int)(MessageText.Length * fraction);
            TextObj.text = MessageText.Substring(0, StringLength);
        }
        else
        {
            TextObj.text = MessageText;
        }
    }
}