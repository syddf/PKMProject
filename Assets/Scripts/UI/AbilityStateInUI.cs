using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
public class AbilityStateInUI : MonoBehaviour
{
    public Transform StartTrans;
    public Transform EndTrans;
    public float Duration;
    public float Totaltime = 0.5f;
    public TextMeshProUGUI AbilityText;
    public Image PKMSprite;
    void Start()
    {
    }

    void OnEnable()
    {
        this.gameObject.transform.position = StartTrans.position;
        Duration = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Duration += Time.deltaTime;
        if(Duration < Totaltime)
        {
            float fraction = Duration / Totaltime;
            this.gameObject.transform.position = 
            Vector3.Lerp(StartTrans.position, EndTrans.position, fraction);
        }
    }
}
