using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
using UnityEngine.UI;

public class EDMessage : MonoBehaviour
{
    private string MessageText;
    public Text TextObj;

    private int Counter = 0; 
    public int TotalFrame = 60;
    private bool InAnim = false;

    void Start()
    {
        
    }

    void OnEnable() 
    {
        Counter = 0;
        TextObj.text = "";
        InAnim = false;
    }

    public void ShowMessage(string Message)
    {
        MessageText = Message;
        Counter = 0;
        InAnim = true;

    }

    void Update()
    {
        if(InAnim)
        {
            Counter++;
            float fraction = (float)Counter / (float)TotalFrame; // 计算已完成的动画比例

            if (fraction <= 1) // 确保fraction不会超过1
            {
                int StringLength = (int)(MessageText.Length * fraction);
                TextObj.text = MessageText.Substring(0, StringLength);
            }
            else
            {
                TextObj.text = MessageText;
                InAnim = false;
                Counter = 0;
            }
        }
    }
}
