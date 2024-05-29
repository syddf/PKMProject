using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonController : MonoBehaviour
{
    public Toggle toggleUI;

    // 在 Unity 编辑器中将 ToggleUI 拖放到这个字段中
    public void SetToggleValue()
    {
        toggleUI.isOn = !toggleUI.isOn;
    }
}
