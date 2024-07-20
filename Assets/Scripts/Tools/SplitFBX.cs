#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
public class SplitFBX : MonoBehaviour
{
    [MenuItem("Assets/Custom Tools/SplitFBX")]
    private static void ProcessPrefab()
    {
        Object selectedObject = Selection.activeObject;
        GameObject selectedGameObject = selectedObject as GameObject;
        SkinnedMeshRenderer[] skinnedMeshRenderers = selectedGameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        string prefabPath = AssetDatabase.GetAssetPath(selectedObject);
        string currentDirectoryPath = System.IO.Path.GetDirectoryName(prefabPath);
        foreach(SkinnedMeshRenderer renderer in skinnedMeshRenderers)
        {
            Mesh mesh = renderer.sharedMesh;
            string meshName = mesh.name;
            string meshPath = currentDirectoryPath + "\\..\\Mesh\\" + meshName + ".asset";
            
            Mesh newMesh = AssetDatabase.LoadAssetAtPath<Mesh>("Models\\1\\Mesh\\" + meshName + ".asset");
            Debug.Log(meshPath);
            if(newMesh != null)
            {
                Debug.Log("aa");
                renderer.sharedMesh = newMesh;
            }
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
#endif