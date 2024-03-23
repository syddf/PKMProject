using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokemonSpritesManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static Dictionary<int, Sprite> Sprites = new Dictionary<int, Sprite>();
    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("UI/PokemonSprites");

        foreach (Sprite sprite in sprites)
        {
            int Index = Convert.ToInt32(sprite.name); // 输出每个Sprite的名称，以此确认它们已被加载
            Sprites.Add(Index, sprite);
        }
    }
}
