using UnityEngine;
using UnityEditor;
using System.IO;

public class CustomAssetContextMenu : Editor
{
    private static bool AssetExistsAtPath(string assetPath)
    {
        string guid = AssetDatabase.AssetPathToGUID(assetPath);
        return !string.IsNullOrEmpty(guid);
    }

    private static string FindTexturesContainingStrings(string textureFolder, string string1, string string2)
    {
        // 获取目录下的所有文件路径
        string[] filePaths = Directory.GetFiles(textureFolder);

        foreach (string filePath in filePaths)
        {
            // 将文件路径转换为Unity的相对路径
            string assetPath = filePath.Replace(Application.dataPath, "Assets");

            // 获取文件名（不带扩展名）
            string fileName = Path.GetFileNameWithoutExtension(assetPath);

            // 检查文件名是否包含string1和string2
            if (fileName.Contains(string1) && fileName.Contains(string2))
            {
                return filePath;
            }
        }
        return "";
    }

    [MenuItem("Assets/Custom Tools/SetNormalTextureType")]
    private static void SetTextureTypeToNormalMap()
    {
        Object selectedObject = Selection.activeObject;
        string path = AssetDatabase.GetAssetPath(selectedObject);
        string directoryPath = System.IO.Path.GetDirectoryName(path);
        // 获取目录下的所有文件路径
        string[] filePaths = Directory.GetFiles(directoryPath);

        foreach (string filePath in filePaths)
        {
            // 将文件路径转换为Unity的相对路径
            string assetPath = filePath.Replace(Application.dataPath, "Assets");

            // 加载纹理
            TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;

            // 检查纹理是否存在以及是否包含'nrm'在其名称中
            if (textureImporter != null && Path.GetFileNameWithoutExtension(assetPath).ToLower().Contains("_nrm"))
            {
                // 将TextureType设置为Normal map
                textureImporter.textureType = TextureImporterType.NormalMap;

                // 重新导入纹理以应用更改
                AssetDatabase.ImportAsset(assetPath);

                Debug.Log("已将纹理 " + assetPath + " 的TextureType设置为Normal map");
            }
        }

        Debug.Log("纹理类型设置完成");
    }

    [MenuItem("Assets/Custom Tools/SetNormalTextureType", true)]
    private static bool ValidateSetTextureTypeToNormalMap()
    {
        // 在这里可以添加自定义的验证逻辑
        // 例如，只有当选择的资源满足特定条件时才显示工具 1
        return true; // 默认情况下，始终显示
    }

    private static void DeleteCamerasAndLightsInChildren(Transform parent)
    {
        // 遍历所有子对象
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Transform child = parent.GetChild(i);

            // 检查子对象是否为 Camera 或 Light
            if (child.GetComponent<Camera>() != null || child.GetComponent<Light>() != null)
            {
                // 如果是，则删除该子对象
                DestroyImmediate(child.gameObject);
            }
            else
            {
                // 否则，递归调用以继续检查其子对象
                DeleteCamerasAndLightsInChildren(child);
            }
        }
    }

    // 添加一个菜单项，用于在资源上右键单击时调用的函数
    [MenuItem("Assets/Custom Tools/AutoCreateMaterial")]
    private static void AutoCreateMaterial()
    {
        SetTextureTypeToNormalMap();
        // 获取当前选中的对象
        Object selectedObject = Selection.activeObject;

        string objPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        string currentDirectoryPath = System.IO.Path.GetDirectoryName(objPath);

        // 创建Materials目录和Textures目录
        string materialsDirectoryPath = currentDirectoryPath + "/Materials";
        string texturesDirectoryPath = currentDirectoryPath + "/Textures";
        if (!AssetDatabase.IsValidFolder(materialsDirectoryPath))
        {
            AssetDatabase.CreateFolder(currentDirectoryPath, "Materials");
        }
        if (!AssetDatabase.IsValidFolder(texturesDirectoryPath))
        {
            AssetDatabase.CreateFolder(currentDirectoryPath, "Textures");
        }

        // 获取当前目录下的所有文件路径
        string[] filePaths = Directory.GetFiles(currentDirectoryPath);

        foreach (string filePath in filePaths)
        {
            // 将文件路径转换为Unity的相对路径
            string assetPath = filePath.Replace(Application.dataPath, "Assets");

            if (assetPath.EndsWith(".png") || assetPath.EndsWith(".jpg"))
            {
                string textureName = Path.GetFileName(assetPath);
                string newTexturePath = texturesDirectoryPath + "/" + textureName;
                AssetDatabase.MoveAsset(assetPath, newTexturePath);
                Debug.Log("已移动贴图：" + assetPath + " 到 " + newTexturePath);
            }
        }

        // 检查是否有对象被选中，并且是否是一个 GameObject
        if (selectedObject != null && selectedObject is GameObject)
        {
            GameObject selectedGameObject = selectedObject as GameObject;
            Transform[] children = selectedGameObject.GetComponentsInChildren<Transform>();

            // 遍历每个子对象
            foreach (Transform child in children)
            {
                // 获取子对象上的 Renderer 组件
                Renderer renderer = child.GetComponent<Renderer>();

                // 如果子对象有 Renderer 组件，获取其材质并复制到当前目录下
                if (renderer != null && renderer.sharedMaterial != null)
                {
                    Material[] materials = renderer.sharedMaterials;
                    // 遍历每个材质
                    foreach (Material material in materials)
                    {
                        if (material != null)
                        {
                            // 在当前目录下创建材质副本
                            string path = AssetDatabase.GetAssetPath(selectedGameObject);
                            string directory = System.IO.Path.GetDirectoryName(path);
                            string newMaterialPath = directory + "/Materials/" + material.name + ".mat";

                            if (true)
                            {
                                // 克隆材质
                                Material newMaterial = new Material(material);

                                // 将新材质的Shader设置为Custom/NewSurfaceShader
                                Shader newShader = Shader.Find("Custom/NewSurfaceShader");
                                if (newShader != null)
                                {
                                    newMaterial.shader = newShader;
                                }
                                else
                                {
                                    Debug.LogWarning("未找到自定义Shader：Custom/NewSurfaceShader");
                                }

                                int dotIndex = material.name.IndexOf('.');
                                string trueName = dotIndex != -1 ? material.name.Substring(0, dotIndex) : material.name;
                                string originName = trueName;
                                if (trueName == "l_eye" || trueName == "r_eye")
                                {
                                    trueName = "eye_";
                                }
                                string nrmTex = FindTexturesContainingStrings(texturesDirectoryPath, originName == "r_eye" ? "eye2_" : trueName, "_nrm");
                                Texture nrmTexture = AssetDatabase.LoadAssetAtPath<Texture>(nrmTex);
                                if (nrmTexture != null)
                                {
                                    newMaterial.SetTexture("_NormTex", nrmTexture);
                                }

                                string albTex = FindTexturesContainingStrings(texturesDirectoryPath, trueName, "_alb");
                                Texture albTexture = AssetDatabase.LoadAssetAtPath<Texture>(albTex);
                                if (albTexture != null)
                                {
                                    newMaterial.SetTexture("_MainTex", albTexture);
                                }

                                string aoTex = FindTexturesContainingStrings(texturesDirectoryPath, trueName, "_ao");
                                Texture aoTexture = AssetDatabase.LoadAssetAtPath<Texture>(aoTex);
                                if (aoTexture != null)
                                {
                                    newMaterial.SetTexture("_AOTex", aoTexture);
                                }

                                string lymTex = FindTexturesContainingStrings(texturesDirectoryPath, trueName, "_lym");
                                Texture lymTexture = AssetDatabase.LoadAssetAtPath<Texture>(lymTex);
                                if (lymTexture != null)
                                {
                                    newMaterial.SetTexture("_LayerTex", lymTexture);
                                }

                                string mskTex = FindTexturesContainingStrings(texturesDirectoryPath, trueName, "_msk");
                                Texture mskTexture = AssetDatabase.LoadAssetAtPath<Texture>(mskTex);
                                if (mskTexture != null)
                                {
                                    newMaterial.SetTexture("_MaskTex", mskTexture);
                                }

                                string rgnTex = FindTexturesContainingStrings(texturesDirectoryPath, trueName, "_rgn");
                                Texture rgnTexture = AssetDatabase.LoadAssetAtPath<Texture>(rgnTex);
                                if (rgnTexture != null)
                                {
                                    newMaterial.SetTexture("_RoughnessTex", rgnTexture);
                                }

                                AssetDatabase.CreateAsset(newMaterial, newMaterialPath);
                            }
                            else
                            {
                                Debug.LogWarning("已经存在材质：" + material.name);
                            }
                        }
                    }
                }
            }
        }

        if (selectedObject != null && selectedObject is GameObject)
        {
            GameObject selectedGameObject = selectedObject as GameObject;

            GameObject instantiatedObject = Instantiate(selectedGameObject);
            instantiatedObject.name = selectedGameObject.name + "_Instance";

            Renderer[] renderers = instantiatedObject.GetComponentsInChildren<Renderer>(true);
            foreach (Renderer renderer in renderers)
            {
                // 获取 Renderer 使用的所有 Material
                Material[] materials = renderer.sharedMaterials;
                for (int i = 0; i < materials.Length; i++)
                {
                    if (materials[i] != null)
                    {
                        // 获取材质的名称
                        string materialName = materials[i].name;

                        // 构建材质的完整路径
                        string materialPath = Path.Combine(materialsDirectoryPath, materialName + ".mat");

                        // 加载材质
                        Material newMaterial = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
                        if (newMaterial != null)
                        {
                            // 替换材质
                            materials[i] = newMaterial;
                        }
                        else
                        {
                            Debug.LogWarning("未找到材质：" + materialName + "，无法替换");
                        }
                    }
                }
                renderer.sharedMaterials = materials;
            }


            DeleteCamerasAndLightsInChildren(instantiatedObject.transform);

            string prefabDirectoryPath = currentDirectoryPath + "/Prefabs";
            if (!AssetDatabase.IsValidFolder(prefabDirectoryPath))
            {
                AssetDatabase.CreateFolder(currentDirectoryPath, "Prefabs");
            }

            string prefabPath = prefabDirectoryPath + "/" + selectedGameObject.name + ".prefab";

            // 检查 prefab 是否已存在，如果存在则删除旧的
            if (File.Exists(prefabPath))
            {
                AssetDatabase.DeleteAsset(prefabPath);
            }

            // 将对象保存为 prefab
            PrefabUtility.SaveAsPrefabAsset(instantiatedObject, prefabPath);

            DestroyImmediate(instantiatedObject);
        }

    }

    // 用于确定是否可以在资源上右键单击时显示指定的菜单项
    [MenuItem("Assets/Custom Tools/AutoCreateMaterial", true)]
    private static bool ValidateAutoCreateMaterial()
    {
        // 在这里可以添加自定义的验证逻辑
        // 例如，只有当选择的资源满足特定条件时才显示工具 1
        return true; // 默认情况下，始终显示
    }

    [MenuItem("Assets/Custom Tools/CopyMaterialProperty")]
    private static void CopyMaterialProperty()
    {
        Object selectedObject = Selection.activeObject;
        if (selectedObject != null && selectedObject is Material)
        {
            Material source = selectedObject as Material;
            if(source.name.Contains("eye"))
            {
                string objPath = AssetDatabase.GetAssetPath(Selection.activeObject);
                string currentDirectoryPath = System.IO.Path.GetDirectoryName(objPath);
                string[] filePaths = Directory.GetFiles(currentDirectoryPath);

                foreach (string filePath in filePaths)
                {
                    // 将文件路径转换为Unity的相对路径
                    string assetPath = filePath.Replace(Application.dataPath, "Assets");

                    if (assetPath.EndsWith(".mat"))
                    {
                        string matName = Path.GetFileName(assetPath);
                        if(matName.Contains("eye") && !matName.Contains(source.name))
                        {
                            Material target = AssetDatabase.LoadAssetAtPath<Material>(assetPath);
                            target.SetColor("_Layer1Color", source.GetColor("_Layer1Color"));
                            target.SetColor("_Layer2Color", source.GetColor("_Layer2Color"));
                            target.SetColor("_Layer3Color", source.GetColor("_Layer3Color"));
                            target.SetColor("_MaskColor", source.GetColor("_MaskColor"));
                            target.EnableKeyword("_MASKENABLE_ON");
                            target.EnableKeyword("_LAYERMASKENABLE_ON");
                        }
                    }
                }
            }
        }
    }
}
