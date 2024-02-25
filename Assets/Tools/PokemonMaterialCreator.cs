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
        // ��ȡĿ¼�µ������ļ�·��
        string[] filePaths = Directory.GetFiles(textureFolder);

        foreach (string filePath in filePaths)
        {
            // ���ļ�·��ת��ΪUnity�����·��
            string assetPath = filePath.Replace(Application.dataPath, "Assets");

            // ��ȡ�ļ�����������չ����
            string fileName = Path.GetFileNameWithoutExtension(assetPath);

            // ����ļ����Ƿ����string1��string2
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
        // ��ȡĿ¼�µ������ļ�·��
        string[] filePaths = Directory.GetFiles(directoryPath);

        foreach (string filePath in filePaths)
        {
            // ���ļ�·��ת��ΪUnity�����·��
            string assetPath = filePath.Replace(Application.dataPath, "Assets");

            // ��������
            TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;

            // ��������Ƿ�����Լ��Ƿ����'nrm'����������
            if (textureImporter != null && Path.GetFileNameWithoutExtension(assetPath).ToLower().Contains("_nrm"))
            {
                // ��TextureType����ΪNormal map
                textureImporter.textureType = TextureImporterType.NormalMap;

                // ���µ���������Ӧ�ø���
                AssetDatabase.ImportAsset(assetPath);

                Debug.Log("�ѽ����� " + assetPath + " ��TextureType����ΪNormal map");
            }
        }

        Debug.Log("���������������");
    }

    [MenuItem("Assets/Custom Tools/SetNormalTextureType", true)]
    private static bool ValidateSetTextureTypeToNormalMap()
    {
        // �������������Զ������֤�߼�
        // ���磬ֻ�е�ѡ�����Դ�����ض�����ʱ����ʾ���� 1
        return true; // Ĭ������£�ʼ����ʾ
    }

    private static void DeleteCamerasAndLightsInChildren(Transform parent)
    {
        // ���������Ӷ���
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Transform child = parent.GetChild(i);

            // ����Ӷ����Ƿ�Ϊ Camera �� Light
            if (child.GetComponent<Camera>() != null || child.GetComponent<Light>() != null)
            {
                // ����ǣ���ɾ�����Ӷ���
                DestroyImmediate(child.gameObject);
            }
            else
            {
                // ���򣬵ݹ�����Լ���������Ӷ���
                DeleteCamerasAndLightsInChildren(child);
            }
        }
    }

    // ���һ���˵����������Դ���Ҽ�����ʱ���õĺ���
    [MenuItem("Assets/Custom Tools/AutoCreateMaterial")]
    private static void AutoCreateMaterial()
    {
        SetTextureTypeToNormalMap();
        // ��ȡ��ǰѡ�еĶ���
        Object selectedObject = Selection.activeObject;

        string objPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        string currentDirectoryPath = System.IO.Path.GetDirectoryName(objPath);

        // ����MaterialsĿ¼��TexturesĿ¼
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

        // ��ȡ��ǰĿ¼�µ������ļ�·��
        string[] filePaths = Directory.GetFiles(currentDirectoryPath);

        foreach (string filePath in filePaths)
        {
            // ���ļ�·��ת��ΪUnity�����·��
            string assetPath = filePath.Replace(Application.dataPath, "Assets");

            if (assetPath.EndsWith(".png") || assetPath.EndsWith(".jpg"))
            {
                string textureName = Path.GetFileName(assetPath);
                string newTexturePath = texturesDirectoryPath + "/" + textureName;
                AssetDatabase.MoveAsset(assetPath, newTexturePath);
                Debug.Log("���ƶ���ͼ��" + assetPath + " �� " + newTexturePath);
            }
        }

        // ����Ƿ��ж���ѡ�У������Ƿ���һ�� GameObject
        if (selectedObject != null && selectedObject is GameObject)
        {
            GameObject selectedGameObject = selectedObject as GameObject;
            Transform[] children = selectedGameObject.GetComponentsInChildren<Transform>();

            // ����ÿ���Ӷ���
            foreach (Transform child in children)
            {
                // ��ȡ�Ӷ����ϵ� Renderer ���
                Renderer renderer = child.GetComponent<Renderer>();

                // ����Ӷ����� Renderer �������ȡ����ʲ����Ƶ���ǰĿ¼��
                if (renderer != null && renderer.sharedMaterial != null)
                {
                    Material[] materials = renderer.sharedMaterials;
                    // ����ÿ������
                    foreach (Material material in materials)
                    {
                        if (material != null)
                        {
                            // �ڵ�ǰĿ¼�´������ʸ���
                            string path = AssetDatabase.GetAssetPath(selectedGameObject);
                            string directory = System.IO.Path.GetDirectoryName(path);
                            string newMaterialPath = directory + "/Materials/" + material.name + ".mat";

                            if (true)
                            {
                                // ��¡����
                                Material newMaterial = new Material(material);

                                // ���²��ʵ�Shader����ΪCustom/NewSurfaceShader
                                Shader newShader = Shader.Find("Custom/NewSurfaceShader");
                                if (newShader != null)
                                {
                                    newMaterial.shader = newShader;
                                }
                                else
                                {
                                    Debug.LogWarning("δ�ҵ��Զ���Shader��Custom/NewSurfaceShader");
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
                                Debug.LogWarning("�Ѿ����ڲ��ʣ�" + material.name);
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
                // ��ȡ Renderer ʹ�õ����� Material
                Material[] materials = renderer.sharedMaterials;
                for (int i = 0; i < materials.Length; i++)
                {
                    if (materials[i] != null)
                    {
                        // ��ȡ���ʵ�����
                        string materialName = materials[i].name;

                        // �������ʵ�����·��
                        string materialPath = Path.Combine(materialsDirectoryPath, materialName + ".mat");

                        // ���ز���
                        Material newMaterial = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
                        if (newMaterial != null)
                        {
                            // �滻����
                            materials[i] = newMaterial;
                        }
                        else
                        {
                            Debug.LogWarning("δ�ҵ����ʣ�" + materialName + "���޷��滻");
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

            // ��� prefab �Ƿ��Ѵ��ڣ����������ɾ���ɵ�
            if (File.Exists(prefabPath))
            {
                AssetDatabase.DeleteAsset(prefabPath);
            }

            // �����󱣴�Ϊ prefab
            PrefabUtility.SaveAsPrefabAsset(instantiatedObject, prefabPath);

            DestroyImmediate(instantiatedObject);
        }

    }

    // ����ȷ���Ƿ��������Դ���Ҽ�����ʱ��ʾָ���Ĳ˵���
    [MenuItem("Assets/Custom Tools/AutoCreateMaterial", true)]
    private static bool ValidateAutoCreateMaterial()
    {
        // �������������Զ������֤�߼�
        // ���磬ֻ�е�ѡ�����Դ�����ض�����ʱ����ʾ���� 1
        return true; // Ĭ������£�ʼ����ʾ
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
                    // ���ļ�·��ת��ΪUnity�����·��
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
