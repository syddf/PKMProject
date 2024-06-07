using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine;
using UnityEditor;

public class CustomContextMenu : MonoBehaviour
{
    [MenuItem("GameObject/Auto Set Skill", false, 0)]
    static void CustomOption(MenuCommand menuCommand)
    {
        // Create and show the input window
        CustomInputWindow.Init();
    }
}

public class CustomInputWindow : EditorWindow
{
    private string inputString = "";

    public static void Init()
    {
        CustomInputWindow window = (CustomInputWindow)EditorWindow.GetWindow(typeof(CustomInputWindow));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Enter a string:");
        inputString = EditorGUILayout.TextField(inputString);

        if (GUILayout.Button("Submit"))
        {
            // Perform operations with the input string here
            Debug.Log("Input String: " + inputString);

            // Get the name of the selected GameObject
            GameObject selectedObject = Selection.activeGameObject;

            string[] words = inputString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int Index = 0;
            foreach (string word in words)
            {
                string[] guids = AssetDatabase.FindAssets("t:Prefab", new string[] { "Assets/Scripts/Skills/Skills"});

                List<string> foundPrefabs = new List<string>();
                foreach (string guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    if (prefab != null)
                    {
                        BaseSkill baseSkill = prefab.GetComponent<BaseSkill>();

                        if (baseSkill != null && baseSkill.GetSkillName() == word)
                        {
                            selectedObject.GetComponent<BagPokemon>().SkillPool[Index] = baseSkill;
                        }
                    }


                }
                Index++;
            }

            // Close the window
            this.Close();
        }
    }
}

