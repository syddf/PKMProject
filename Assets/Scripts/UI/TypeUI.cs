using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeUI : MonoBehaviour
{
    public Color[] TypeColors = new Color[18];
    public Sprite[] TypeImages = new Sprite[18];
    public Image IconImage;
    public void SetType(EType type)
    {
        if(type == EType.None)
        {
            return;
        }
        this.GetComponent<Image>().color = TypeColors[(int)type];
        if (IconImage != null)
        {
            IconImage.sprite = TypeImages[(int)type];
        }
    }
}
