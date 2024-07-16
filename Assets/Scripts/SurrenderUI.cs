using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurrenderUI : MonoBehaviour
{
    public GameObject SurrenderButton;
    public GameObject ConfirmObject;
    public void OnEnable()
    {
        SurrenderButton.SetActive(true);
        ConfirmObject.SetActive(false);
    }
}
