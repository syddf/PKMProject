using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;
using System.Collections.Generic;
using System;

public interface IPokemonParser
{
    List<Dictionary<string, string>> Parse(string filePath);
}

public class PokemonParser : IPokemonParser
{
    public List<Dictionary<string, string>> Parse(string filePath)
    {
        List<Dictionary<string, string>> allPokemonData = new List<Dictionary<string, string>>();

        try
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                Dictionary<string, string> pokemonData = new Dictionary<string, string>();
                while ((line = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        // 空行表示一个 Pokemon 数据块的结束
                        if (pokemonData.Count > 0)
                        {
                            allPokemonData.Add(pokemonData);
                            pokemonData = new Dictionary<string, string>();
                        }
                    }
                    else
                    {
                        string[] parts = line.Split(new string[] { "::" }, StringSplitOptions.None);
                        if (parts.Length == 2)
                        {
                            string key = parts[0].Trim();
                            string value = parts[1].Trim();
                            pokemonData[key] = value;
                        }
                    }
                }

                // 添加最后一个 Pokemon 数据块
                if (pokemonData.Count > 0)
                {
                    allPokemonData.Add(pokemonData);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while parsing the file: " + ex.Message);
        }

        return allPokemonData;
    }
}
public class Pokemon
{
    public string SpeciesID { get; set; }
    public int Number { get; set; }
    public string Name { get; set; }
    public string Type0 { get; set; }
    public string Type1 { get; set; }
    public string Ability0 { get; set; }
    public string Ability1 { get; set; }
    public string AbilityH { get; set; }
    public int HP { get; set; }
    public int Atk { get; set; }
    public int SAtk { get; set; }
    public int Def { get; set; }
    public int Spd { get; set; }
    public int Spe { get; set; }
    public double Heightm { get; set; }
    public double Weightkg { get; set; }
    public string Prevo { get; set; }
    public string EvoType { get; set; }
    public string EvoLevel { get; set; }
    public string EvoItem { get; set; }
    public float MaleGenderProbability { get; set; }
    public float FemaleGenderProbability { get; set; }

    public void FillFromDictionary(Dictionary<string, string> data)
    {
        if (data.ContainsKey("SpeciesID")) SpeciesID = data["SpeciesID"];
        if (data.ContainsKey("Number")) Number = int.Parse(data["Number"]);
        if (data.ContainsKey("Name")) Name = data["Name"];
        if (data.ContainsKey("Type0")) Type0 = data["Type0"];
        if (data.ContainsKey("Type1")) Type1 = data["Type1"];
        if (data.ContainsKey("Ability0")) Ability0 = data["Ability0"];
        if (data.ContainsKey("Ability1")) Ability1 = data["Ability1"];
        if (data.ContainsKey("AbilityH")) AbilityH = data["AbilityH"];
        if (data.ContainsKey("HP")) HP = int.Parse(data["HP"]);
        if (data.ContainsKey("Atk")) Atk = int.Parse(data["Atk"]);
        if (data.ContainsKey("SAtk")) SAtk = int.Parse(data["SAtk"]);
        if (data.ContainsKey("Def")) Def = int.Parse(data["Def"]);
        if (data.ContainsKey("Spd")) Spd = int.Parse(data["Spd"]);
        if (data.ContainsKey("Spe")) Spe = int.Parse(data["Spe"]);
        if (data.ContainsKey("Heightm")) Heightm = double.Parse(data["Heightm"]);
        if (data.ContainsKey("Weightkg")) Weightkg = double.Parse(data["Weightkg"]);
        if (data.ContainsKey("Prevo")) Prevo = data["Prevo"];
        if (data.ContainsKey("EvoType")) EvoType = data["EvoType"];
        if (data.ContainsKey("EvoLevel")) EvoLevel = data["EvoLevel"];
        if (data.ContainsKey("EvoItem")) EvoItem = data["EvoItem"];
        if (data.ContainsKey("Gender"))
        {
            string[] genderProbabilities = data["Gender"].Split('-');
            if (genderProbabilities.Length == 2)
            {
                MaleGenderProbability = float.Parse(genderProbabilities[0]);
                FemaleGenderProbability = float.Parse(genderProbabilities[1]);
            }
        }
    }

    public void FillPokemonData(PokemonData data)
    {
        data.SpeciesID = SpeciesID;
        data.Number = Number;
        data.Name = Name;
        data.Type0 = Type0;
        data.Type1 = Type1;
        data.Ability0 = Ability0;
        data.Ability1 = Ability1;
        data.AbilityH = AbilityH;
        data.HP = HP;
        data.Atk = Atk;
        data.SAtk = SAtk;
        data.Def = Def;
        data.Spd = Spd;
        data.Spe = Spe;
        data.Heightm = Heightm;
        data.Weightkg = Weightkg;
        data.Prevo = Prevo;
        data.EvoType = EvoType;
        data.EvoLevel = EvoLevel;
        data.EvoItem = EvoItem;
        data.MaleGenderProbability = MaleGenderProbability;
        data.FemaleGenderProbability = FemaleGenderProbability;
    }
}


[CreateAssetMenu(fileName = "NewPokemonData", menuName = "Pokemon/PokemonData")]
public class PokemonData : ScriptableObject
{
    public string SpeciesID;
    public int Number;
    public string Name;
    public string Type0;
    public string Type1;
    public string Ability0;
    public string Ability1;
    public string AbilityH;
    public int HP;
    public int Atk;
    public int SAtk;
    public int Def;
    public int Spd;
    public int Spe;
    public double Heightm;
    public double Weightkg;
    public string Prevo;
    public string EvoType;
    public string EvoLevel;
    public string EvoItem;
    public float MaleGenderProbability;
    public float FemaleGenderProbability;
}
#if UNITY_EDITOR
public class CreatePokemonTool : EditorWindow
{
    private string filePath = "Assets/Datas/PokemonDex.txt"; // 指定文本文件的路径

    [MenuItem("Tools/Create Pokemon Resource")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CreatePokemonTool));
    }

    void OnGUI()
    {
        GUILayout.Label("Create Pokemon Resource", EditorStyles.boldLabel);

        filePath = EditorGUILayout.TextField("File Path", filePath);

        if (GUILayout.Button("Create Pokemon"))
        {
            CreatePokemonCollectionFromTextFile(filePath);
        }
    }

    void CreatePokemonCollectionFromTextFile(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError("File does not exist at path: " + path);
            return;
        }

        IPokemonParser parser = new PokemonParser();
        List<Dictionary<string, string>> allPokemonData = parser.Parse(path);

        PokemonDataCollection pokemonDataCollection = ScriptableObject.CreateInstance<PokemonDataCollection>();
        pokemonDataCollection.pokemonList = new List<PokemonData>();
        foreach (Dictionary<string, string> pokemonData in allPokemonData)
        {
            PokemonData pokemonDataAsset = ScriptableObject.CreateInstance<PokemonData>();
            Pokemon pokemon = new Pokemon();
            pokemon.FillFromDictionary(pokemonData);
            pokemon.FillPokemonData(pokemonDataAsset);

            string pkmAssetPath = path.Replace("/PokemonDex.txt", "/SinglePokemonData/") + pokemon.SpeciesID + ".asset";
            AssetDatabase.CreateAsset(pokemonDataAsset, pkmAssetPath);

            pokemonDataCollection.pokemonList.Add(pokemonDataAsset);
        }

        // Create a new asset at the specified path
        string assetPath = path.Replace(".txt", "_Collection.asset");
        AssetDatabase.CreateAsset(pokemonDataCollection, assetPath);
        AssetDatabase.SaveAssets();

        Debug.Log("Pokemon collection asset created at: " + assetPath);
    }
}
#endif