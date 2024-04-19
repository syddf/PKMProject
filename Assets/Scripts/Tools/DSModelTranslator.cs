using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections.Generic;
using System.IO;
using System.Linq;
public class DSModelTranslator : MonoBehaviour
{
    [MenuItem("Assets/Custom Tools/TranslateDSModel")]
    private static void OrganizeAndUpdatePrefab()
    {
        string selectedPath = AssetDatabase.GetAssetPath(Selection.activeObject);

        // 在选中的路径下创建Prefabs文件夹并处理.prefab文件
        string prefabsPath = Path.Combine(selectedPath, "Prefabs");
        if (!AssetDatabase.IsValidFolder(prefabsPath))
        {
            AssetDatabase.CreateFolder(selectedPath, "Prefabs");
        }

        // 移动Files文件夹下的内容，并保持层级结构
        string filesPath = Path.Combine(selectedPath, "Files");
        if (AssetDatabase.IsValidFolder(filesPath))
        {
            MoveDirectory(filesPath, selectedPath);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        // 遍历并移动.prefab文件
        var prefabGUIDs = AssetDatabase.FindAssets("t:Prefab", new[] { selectedPath });
        int counter = 1;
        foreach (var guid in prefabGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string destPath = Path.Combine(prefabsPath, "pkmModel.prefab");
            string prefabPath = AssetDatabase.GUIDToAssetPath(guid);
            UpdatePrefab(prefabPath, selectedPath);
            AssetDatabase.MoveAsset(path, destPath);
        }

        // 删除Files文件夹
        AssetDatabase.DeleteAsset(filesPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        string sourcePath = "Assets/Resources/Models/DSAnimator.controller"; // 源Animator路径
        string destinationPath =  AssetDatabase.GetAssetPath(Selection.activeObject); // 目标Animator路径
        string animationsPath = Path.Combine(destinationPath, "Animations");

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
                var newClip = allAnimations.FirstOrDefault(clip => clip.name == originalClip.name);
                if (newClip != null)
                {
                    overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(originalClip, newClip);
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
        string nprefabPath = Path.Combine(destinationPath, "Prefabs");
        nprefabPath = Path.Combine(nprefabPath, "pkmModel.prefab");
        var prefabOldRoot = PrefabUtility.LoadPrefabContents(nprefabPath);
        var prefabRoot = prefabOldRoot.transform.GetChild(0).gameObject;
        
        Rigidbody rb = prefabRoot.AddComponent<Rigidbody>();
        // 设置Rigidbody的一些属性，如下面示例所示，或根据需要进行调整
        rb.useGravity = true; // 启用重力
        rb.isKinematic = false; // 非运动学
        BoxCollider box = prefabRoot.AddComponent<BoxCollider>();
        box.size = new Vector3(1, 1, 1); // 设置碰撞器大小，可以根据实际需要调整
        DestroyImmediate(prefabRoot.GetComponent<Animator>());
        Animator anim = prefabRoot.AddComponent<Animator>();
        anim.runtimeAnimatorController = overrideController;
        prefabRoot.AddComponent<InitPokemonComponets>();
        prefabRoot.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        prefabRoot.transform.eulerAngles = new Vector3(-90.0f, 0.0f, 0.0f);
        var newRoot = Instantiate(prefabRoot);
        newRoot.name = prefabOldRoot.name;
        // Apply changes and save the Prefab
        PrefabUtility.SaveAsPrefabAsset(newRoot, nprefabPath);
        DestroyImmediate(newRoot); 
    }
    private static void MoveDirectory(string sourceDirPath, string destDirPath)
    {
        var directoryInfo = new DirectoryInfo(sourceDirPath);

        foreach (DirectoryInfo dirInfo in directoryInfo.GetDirectories())
        {
            string targetDirPath = Path.Combine(destDirPath, dirInfo.Name);
            if (!AssetDatabase.IsValidFolder(targetDirPath))
            {
                AssetDatabase.CreateFolder(destDirPath, dirInfo.Name);
            }

            // 递归处理子文件夹
            MoveDirectory(dirInfo.FullName, targetDirPath);
        }

        foreach (FileInfo fileInfo in directoryInfo.GetFiles("*", SearchOption.TopDirectoryOnly))
        {
            string sourcePath = "Assets" + fileInfo.FullName.Substring(Application.dataPath.Length).Replace('\\', '/');
            string targetPath = Path.Combine(destDirPath, fileInfo.Name).Replace('\\', '/');

            if (File.Exists(targetPath))
            {
                // 如果目标路径下已存在同名文件，生成一个唯一的新路径
                targetPath = AssetDatabase.GenerateUniqueAssetPath(targetPath);
            }

            AssetDatabase.MoveAsset(sourcePath, targetPath);
        }
    }

    private static void UpdatePrefab(string prefabPath, string selectedPath)
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        
        if (prefab != null)
        {
            string meshesPath = Path.Combine(selectedPath, "Meshes");
            string materialsPath = Path.Combine(selectedPath, "Materials");

            foreach (var renderer in prefab.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                // 更新Mesh
                if (renderer.sharedMesh != null)
                {
                    string meshPath = Path.Combine(meshesPath, renderer.sharedMesh.name + ".mesh");
                    Mesh newMesh = AssetDatabase.LoadAssetAtPath<Mesh>(meshPath);
                    if (newMesh != null)
                    {
                        renderer.sharedMesh = newMesh;
                    }
                }

                // 更新Materials
                var newMaterials = renderer.sharedMaterials.Select(material =>
                {
                    if (material != null)
                    {
                        string materialPath = Path.Combine(materialsPath, material.name + ".mat");
                        Material newMaterial = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
                        return newMaterial != null ? newMaterial : material;
                    }
                    return material;
                }).ToArray();

                renderer.sharedMaterials = newMaterials;
            }

            // 应用Prefab的更改
            PrefabUtility.SavePrefabAsset(prefab);
        }
    }
    [MenuItem("Assets/Custom Tools/TranslateDSModel", true)]
    private static bool OrganizeAssetsValidation()
    {
        return true;
    }
}
