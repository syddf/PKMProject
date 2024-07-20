#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class SaveAsSkillAnimPrefab : MonoBehaviour
{
    [MenuItem("GameObject/Save As Skill Anim Prefab", false, 0)]
    static void SaveSelectedGameObjectAsPrefab()
    {
        // 获取选中的 GameObject
        GameObject selectedObject = Selection.activeGameObject;

        if(selectedObject == null)
        {
            Debug.LogWarning("No GameObject selected.");
            return;
        }

        // 指定保存 Prefab 的路径
        string prefabPath = "Assets/Animations/Skills/Prefabs/";

        // 检查是否已存在同名 Prefab
        string prefabName = selectedObject.name + ".prefab";
        string selectedPrefabPath = prefabPath + prefabName;
        GameObject existingPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(selectedPrefabPath);

        if(existingPrefab != null)
        {
            // 如果已存在同名 Prefab，直接更新 Prefab
            PrefabUtility.SaveAsPrefabAsset(selectedObject, selectedPrefabPath);
            Debug.Log("Prefab updated at: " + selectedPrefabPath);
        }
        else
        {
            // 如果不存在同名 Prefab，创建新的 Prefab
            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(selectedObject, selectedPrefabPath);

            if(prefab != null)
            {
                // 将生成的 Prefab 设置为当前选中对象
                Selection.activeGameObject = prefab;
                Debug.Log("Prefab saved at: " + selectedPrefabPath);
            }
            else
                Debug.LogError("Failed to save prefab.");
        }
    }
}
#endif