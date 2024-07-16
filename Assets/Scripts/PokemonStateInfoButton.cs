using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class PokemonStateInfoButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public BattlePokemonInfoUI ReferenceUI;
    public BattlePokemonsInfoUI ReferenceGroupUI;
    public bool IsPlayer;
    public GameObject targetUI; // 目标UI
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(IsPlayer)
        {
            ReferenceUI.SetBattlePokemon(BattleManager.StaticManager.GetBattlePokemons()[0]);
        }
        else
        {
            ReferenceUI.SetBattlePokemon(BattleManager.StaticManager.GetBattlePokemons()[1]);
            if(ReferenceGroupUI)
            {
                ReferenceGroupUI.UpdateUI();
            }
        }
        if (eventData.pointerEnter.gameObject == targetUI)
        {
            ReferenceUI.gameObject.SetActive(true);
            if(ReferenceGroupUI)
            {
                ReferenceGroupUI.gameObject.SetActive(true);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerEnter.gameObject == targetUI)
        {
            ReferenceUI.gameObject.SetActive(false);
            if(ReferenceGroupUI)
            {
                ReferenceGroupUI.gameObject.SetActive(false);
            }
        }
    }
}
