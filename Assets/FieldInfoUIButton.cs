using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class FieldInfoUIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public BattleFiledUIInfo ReferenceUI;
    public GameObject targetUI; // 目标UI
    public void OnPointerEnter(PointerEventData eventData)
    {
        ReferenceUI.UpdateUI();
        if (eventData.pointerEnter.gameObject == targetUI)
        {
            ReferenceUI.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerEnter.gameObject == targetUI)
        {
            ReferenceUI.gameObject.SetActive(false);
        }
    }
}
