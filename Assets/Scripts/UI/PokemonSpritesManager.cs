using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokemonSpritesManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static Dictionary<int, Sprite> PKMSprites = new Dictionary<int, Sprite>();
    public static Dictionary<string, Sprite> ItemSprites = new Dictionary<string, Sprite>();
    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        Sprite[] PKMsprites = Resources.LoadAll<Sprite>("UI/PokemonSprites");

        foreach (Sprite sprite in PKMsprites)
        {
            int Index = Convert.ToInt32(sprite.name); // 输出每个Sprite的名称，以此确认它们已被加载
            PKMSprites.Add(Index, sprite);
        }

        Sprite[] Itemsprites = Resources.LoadAll<Sprite>("UI/ItemSprites");

        foreach (Sprite sprite in Itemsprites)
        {
            ItemSprites.Add(sprite.name, sprite);
        }
    }
}
