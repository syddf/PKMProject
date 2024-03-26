using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections.Generic;
using System.IO;
using System.Linq;
public class AnimatorCreator : MonoBehaviour
{
    [MenuItem("Assets/Custom Tools/CloneAndReplaceAnimatorClips", true)]
    private static bool ValidateCloneAndReplaceClips()
    {
        return true;
    }

    public static string GetKeyName(string InName)
    {
        var clipNameParts = InName.Split('_');
        var newName = string.Join("_", clipNameParts.Skip(4));
        return newName;
    }

    [MenuItem("Assets/Custom Tools/CloneAndReplaceAnimatorClips")]
    public static void CloneAndReplaceClips()
    {
        string sourcePath = "Assets/Models/Animator.controller"; // 源Animator路径
        string destinationPath =  AssetDatabase.GetAssetPath(Selection.activeObject); // 目标Animator路径
        string animationsPath = Path.Combine(destinationPath, "Animation");

        AnimatorController sourceAnimator = AssetDatabase.LoadAssetAtPath<AnimatorController>(sourcePath);
        if (sourceAnimator == null)
        {
            Debug.LogError("Cannot find the source Animator Controller. Please check the path.");
            return;
        }

        var overrideController = new AnimatorOverrideController();
        overrideController.runtimeAnimatorController = sourceAnimator;
        
        // Generate a unique path for the new AnimatorOverrideController
        string overridePath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(destinationPath, sourceAnimator.name + "Override.controller"));
        AssetDatabase.CreateAsset(overrideController, overridePath);

        // Find all AnimationClips in the "Animations" directory
        var allAnimations = AssetDatabase.FindAssets("t:AnimationClip", new[] { animationsPath })
                                         .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                                         .Select(path => AssetDatabase.LoadAssetAtPath<AnimationClip>(path))
                                         .ToList();

        // Prepare a list to collect overrides
        List<KeyValuePair<AnimationClip, AnimationClip>> overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();

        // Get the current list of overrides from the override controller
        overrideController.GetOverrides(overrides);

        for (int i = 0; i < overrides.Count; i++)
        {
            var originalClip = overrides[i].Key;
            var overriddenClip = overrides[i].Value;
            
            // Only replace if the original clip is not null
            if (originalClip != null)
            {
                var clipNameParts = originalClip.name.Split('_');
                if (clipNameParts.Length >= 5)
                {
                    var newName = GetKeyName(originalClip.name); // Use part of the name after the fourth "_"
                    var newClip = allAnimations.FirstOrDefault(clip => GetKeyName(clip.name) == newName);
                    if (newClip != null)
                    {
                        overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(originalClip, newClip);
                    }
                }
            }
        }

        // Apply the overrides
        overrideController.ApplyOverrides(overrides);
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        var usedClips = overrides.Select(o => o.Value).Distinct().ToList();
        foreach (var clip in allAnimations)
        {
            if (!usedClips.Contains(clip))
            {
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(clip));
            }
        }

        string prefabPath = Path.Combine(destinationPath, "Prefabs");
        prefabPath = Path.Combine(prefabPath, "pkmModel.prefab");
        var prefabRoot = PrefabUtility.LoadPrefabContents(prefabPath);
        string meshesPath =Path.Combine(destinationPath, "Mesh");
        // Attempt to get or add an Animator component
        var animator = prefabRoot.GetComponent<Animator>();
        animator.runtimeAnimatorController = overrideController;

        SkinnedMeshRenderer[] renderers = prefabRoot.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var renderer in renderers)
        {
            string meshPath = Path.Combine(meshesPath, renderer.sharedMesh.name + ".asset");
            Mesh newMesh = AssetDatabase.LoadAssetAtPath<Mesh>(meshPath);
            if (newMesh != null)
            {
                renderer.sharedMesh = newMesh;
                Debug.Log($"Replaced mesh for {renderer.gameObject.name} with {newMesh.name}");
            }
            else
            {
                Debug.LogWarning($"Mesh not found: {meshPath}");
            }
        }


        // Apply changes and save the Prefab
        PrefabUtility.SaveAsPrefabAsset(prefabRoot, prefabPath);
        PrefabUtility.UnloadPrefabContents(prefabRoot);    

        Debug.Log("Animator Controller cloned and clips replaced successfully.");
    }
}