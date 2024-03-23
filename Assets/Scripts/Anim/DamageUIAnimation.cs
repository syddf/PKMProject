using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class DamageUIAnimation : MonoBehaviour
{
    public Image HPImage;
    public TextMeshProUGUI HPText;
    public float Duration = 1.0f;
    private float Timer;
    private float StartWidth = 0;
    private int PreHP = 0;
    private int TarHP = 0;
    private BattlePokemon ReferencePokemon;
    private bool Play = false;
    public void Awake()
    {
        RectTransform rectTransform = HPImage.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            StartWidth = rectTransform.sizeDelta.x;
        }
    }
    public void OnEnable()
    {
        Timer = 0;
    }
    public void Update()
    {
        if(!Play)
        {
            return;
        }
        Timer += Time.deltaTime;
        float Ratio = Timer / Duration;
        int MaxHP = ReferencePokemon.GetMaxHP();
        int CurHP = TarHP;
        if(Ratio <= 1.0f)
        {
            CurHP = (int)(Ratio * TarHP + (1.0f - Ratio) * PreHP);
        }
        else
        {
            PreHP = TarHP;
            Play = false;
        }
        HPText.text = CurHP + "/" + MaxHP;
        RectTransform rectTransform = HPImage.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            float Width = ((float)CurHP / (float)MaxHP) * StartWidth;
            rectTransform.sizeDelta = new Vector2(Width, rectTransform.sizeDelta.y);
        }
    }

    public void SetPokemon(BattlePokemon InReferencePokemon)
    {
        ReferencePokemon = InReferencePokemon;
        PreHP = ReferencePokemon.GetHP();
    }
    public void NewDamage(int Damage, BattlePokemon InReferencePokemon)
    {
        ReferencePokemon = InReferencePokemon;
        Timer = 0;
        TarHP = PreHP - Damage;
        Play = true;
    }
}
