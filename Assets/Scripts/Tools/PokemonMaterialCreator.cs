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
        string[] filePaths = Directory.GetFiles(textureFolder);

        foreach (string filePath in filePaths)
        {
            string assetPath = filePath.Replace(Application.dataPath, "Assets");

            string fileName = Path.GetFileNameWithoutExtension(assetPath);

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
        string[] filePaths = Directory.GetFiles(directoryPath);

        foreach (string filePath in filePaths)
        {
            string assetPath = filePath.Replace(Application.dataPath, "Assets");

            TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;

            if (textureImporter != null && Path.GetFileNameWithoutExtension(assetPath).ToLower().Contains("_nrm"))
            {
                textureImporter.textureType = TextureImporterType.NormalMap;
                AssetDatabase.ImportAsset(assetPath);
            }
        }

    }

    [MenuItem("Assets/Custom Tools/SetNormalTextureType", true)]
    private static bool ValidateSetTextureTypeToNormalMap()
    {
        return true;
    }

    private static void DeleteCamerasAndLightsInChildren(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Transform child = parent.GetChild(i);
            if (child.GetComponent<Camera>() != null || child.GetComponent<Light>() != null)
            {
                DestroyImmediate(child.gameObject);
            }
            else
            {
                DeleteCamerasAndLightsInChildren(child);
            }
        }
    }

    [MenuItem("Assets/Custom Tools/AutoCreateMaterial")]
    private static void AutoCreateMaterial()
    {
        SetTextureTypeToNormalMap();

        Object selectedObject = Selection.activeObject;

        string objPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        string currentDirectoryPath = System.IO.Path.GetDirectoryName(objPath);

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

        string[] filePaths = Directory.GetFiles(currentDirectoryPath);

        foreach (string filePath in filePaths)
        {
            string assetPath = filePath.Replace(Application.dataPath, "Assets");

            if (assetPath.EndsWith(".png") || assetPath.EndsWith(".jpg"))
            {
                string textureName = Path.GetFileName(assetPath);
                string newTexturePath = texturesDirectoryPath + "/" + textureName;
                AssetDatabase.MoveAsset(assetPath, newTexturePath);
            }
        }

        if (selectedObject != null && selectedObject is GameObject)
        {
            GameObject selectedGameObject = selectedObject as GameObject;
            Transform[] children = selectedGameObject.GetComponentsInChildren<Transform>();

            foreach (Transform child in children)
            {
                Renderer renderer = child.GetComponent<Renderer>();

               if (renderer != null && renderer.sharedMaterial != null)
                {
                    Material[] materials = renderer.sharedMaterials;

                    foreach (Material material in materials)
                    {
                        if (material != null)
                        {
                            string path = AssetDatabase.GetAssetPath(selectedGameObject);
                            string directory = System.IO.Path.GetDirectoryName(path);
                            string newMaterialPath = directory + "/Materials/" + material.name + ".mat";

                            if (true)
                            {
                                Material newMaterial = new Material(material);
                                Shader newShader = Shader.Find("Custom/NewSurfaceShader");
                                if (newShader != null)
                                {
                                    newMaterial.shader = newShader;
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
                Material[] materials = renderer.sharedMaterials;
                for (int i = 0; i < materials.Length; i++)
                {
                    if (materials[i] != null)
                    {
                        string materialName = materials[i].name;
                        string materialPath = Path.Combine(materialsDirectoryPath, materialName + ".mat");
                        Material newMaterial = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
                        if (newMaterial != null)
                        {
                            materials[i] = newMaterial;
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

            if (File.Exists(prefabPath))
            {
                AssetDatabase.DeleteAsset(prefabPath);
            }

            PrefabUtility.SaveAsPrefabAsset(instantiatedObject, prefabPath);

            DestroyImmediate(instantiatedObject);
        }

    }

    [MenuItem("Assets/Custom Tools/AutoCreateMaterial", true)]
    private static bool ValidateAutoCreateMaterial()
    {
        return true;
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
                            UnityEditor.AssetDatabase.SaveAssets();
                            UnityEditor.AssetDatabase.Refresh();
                        }
                    }
                }
            }
        }
    }
}
