using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainerListEntry : MonoBehaviour
{
    public Image Border;
    public TeamEditUI TeamEditWindow;

    public void Reset()
    {
        Border.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }
    public void OnClick()
    {
        TeamEditWindow.OnTrainerClick();
        Border.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
    }
}
