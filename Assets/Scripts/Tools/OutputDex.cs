#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(PokemonTrainer))]
public class CustomEditorScript : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("生成PNG图片"))
        {
            GeneratePNG();
        }
    }

    private void GeneratePNG()
    {
        string filePath = Path.Combine(Application.dataPath, "Dex.txt");
        GameObject selectedObject = Selection.activeGameObject;
        PokemonTrainer CurTrainer = selectedObject.GetComponent<PokemonTrainer>();
        // Check if the file already exists
        if (!File.Exists(filePath))
        {
            // Create a new file if it doesn't exist
            using (StreamWriter writer = File.CreateText(filePath))
            {
                
            }
        }

        // Append some text to the existing file
        using (StreamWriter writer = File.AppendText(filePath))
        {
            writer.WriteLine(CurTrainer.TrainerName);
            string TrainerSkillDesc = "训练家技能 " + CurTrainer.TrainerSkill.GetSkillName() + ":" + CurTrainer.TrainerSkill.GetSkillDescription();
            writer.WriteLine(TrainerSkillDesc);

            for(int Index = 0; Index < 6; Index++)
            {
                string PokemonDescription = 
                CurTrainer.BagPokemons[Index].GetPokemonName() + " 道具:" + CurTrainer.BagPokemons[Index].GetItem().ItemName + " 特性:" + CurTrainer.BagPokemons[Index].GetAbility(false).GetAbilityName() + " 技能:"
                +
                CurTrainer.BagPokemons[Index].SkillPool[0].GetSkillName() + " " +
                CurTrainer.BagPokemons[Index].SkillPool[1].GetSkillName() + " " +
                CurTrainer.BagPokemons[Index].SkillPool[2].GetSkillName() + " " +
                CurTrainer.BagPokemons[Index].SkillPool[3].GetSkillName() + " " +
                CurTrainer.BagPokemons[Index].SkillPool[4].GetSkillName() + " " +
                CurTrainer.BagPokemons[Index].SkillPool[5].GetSkillName() + " " +
                CurTrainer.BagPokemons[Index].SkillPool[6].GetSkillName() + " " +
                CurTrainer.BagPokemons[Index].SkillPool[7].GetSkillName();
                writer.WriteLine(PokemonDescription);
            }

            writer.WriteLine("");
        }
    }
}
#endif