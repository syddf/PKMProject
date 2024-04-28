using UnityEngine;
using TMPro;

public class FixedTextSize : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public float fixedHeight = 26f; // 设置一个固定的高度

    void Start()
    {
        // 设置文本框的初始大小
        RectTransform rectTransform = textMeshPro.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, fixedHeight);

        // 禁用自动大小调整功能
        textMeshPro.enableAutoSizing = false;
    }
}