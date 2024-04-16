using UnityEngine;
using UnityEngine.EventSystems;

public class ShowUIOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject displayUI; // 要显示的UI
    public GameObject targetUI; // 目标UI
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerEnter.gameObject == targetUI)
        {
            displayUI.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerEnter.gameObject == targetUI)
        {
            displayUI.SetActive(false);
        }
    }
}