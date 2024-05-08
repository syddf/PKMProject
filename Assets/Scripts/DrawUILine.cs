using UnityEngine;
using UnityEngine.UI;

public class DrawUILine : MonoBehaviour
{
    public Image lineImage;
    public RectTransform startPointUI;
    public RectTransform endPointUI;

    void Update()
    {
        // 获取起点和终点的位置，并绘制线条
        Vector2 startPoint = startPointUI.position;
        Vector2 endPoint = endPointUI.position;
        DrawLine(startPoint, endPoint);
    }

    void DrawLine(Vector2 start, Vector2 end)
    {
        // 计算线条的长度和角度
        float length = Vector2.Distance(start, end);
        float angle = Mathf.Atan2(end.y - start.y, end.x - start.x) * Mathf.Rad2Deg;

        // 调整线条的位置、长度和角度
        lineImage.rectTransform.sizeDelta = new Vector2(length, lineImage.rectTransform.sizeDelta.y);
        lineImage.rectTransform.pivot = new Vector2(0, 0.5f);
        lineImage.rectTransform.position = start;
        lineImage.rectTransform.rotation = Quaternion.Euler(0, 0, angle);
    }
}