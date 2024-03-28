using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
class BattleItem : MonoBehaviour
{    
    [SerializeField]
    private string ItemName;
    [SerializeField]
    private string Description;

    public string GetItemName()
    {
        return ItemName;
    }

    public string GetItemDescription()
    {
        return Description;
    }
}