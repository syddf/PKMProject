#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.Playables;

public class SkillAnimationSetter : EditorWindow
{
    [MenuItem("Tools/Set Skill Animations")]
    static void OpenWindow()
    {
        SkillAnimationSetter window = (SkillAnimationSetter)EditorWindow.GetWindow(typeof(SkillAnimationSetter));
        window.Show();
    }

    void OnGUI()
    {
        if (GUILayout.Button("Set Skill Animations"))
        {
            SetSkillAnimations();
        }
    }

    void SetSkillAnimations()
    {
        string directoryPath = "Assets/Animations/Skills/Prefabs";
        string[] prefabPaths = Directory.GetFiles("Assets/Scripts/Skills/Skills", "*.prefab", SearchOption.AllDirectories);

        foreach (string prefabPath in prefabPaths)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            BaseSkill skill = prefab.GetComponent<BaseSkill>();

            if (skill != null)
            {
                string animationPath = Path.Combine(directoryPath, Path.GetFileNameWithoutExtension(prefabPath));
                GameObject animPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(animationPath + ".prefab");
                if(animPrefab == null)
                {
                    Debug.LogWarning("Prefab does not have anim: " + prefabPath);
                }
                skill.SkillAnimation = animPrefab.GetComponent<PlayableDirector>();
            }
            else
            {
                Debug.LogWarning("Prefab does not contain BaseSkill component: " + prefabPath);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Skill Animations set for all prefabs.");
    }
}
#endif