using UnityEngine;
using System;

public class AutoScaler : MonoBehaviour
{
    public Transform targetCharacter; // 角色的SkinnedMeshRenderer
    public GameObject cubeObject; // 用于参考大小的Cube

    void Start()
    {
        if (targetCharacter == null || cubeObject == null)
        {
            Debug.LogError("请在Inspector窗口中分配目标角色和Cube对象！");
            return;
        }

        AdjustCharacterScale();
    }

    void Update()
    {
        // 每帧都检测Cube的大小并调整角色的比例
        AdjustCharacterScale();
    }

    void OnEnable()
    {
        AdjustCharacterScale();
    }

    void AdjustCharacterScale()
    {
        // 获取Cube的大小（包围盒的大小）
        Bounds cubeBounds = CalculateBounds(cubeObject.transform);
        Bounds targetBounds = CalculateBounds(targetCharacter.transform);

        // 计算角色应该缩放的比例
        float scaleX = targetBounds.size.x / cubeBounds.size.x;
        float scaleY = targetBounds.size.y / cubeBounds.size.y;
        float scaleZ = targetBounds.size.z / cubeBounds.size.z;
        float Scale = Math.Max(scaleX, scaleY);
        Scale = Math.Max(Scale, scaleZ) / 2.4f;
        // 应用新的比例到角色
        this.transform.localScale = new Vector3(Scale, Scale, Scale);
    }

    Bounds CalculateBounds(Transform target)
    {
        // 获取Cube的所有渲染器
        Renderer[] renderers = target.GetComponentsInChildren<Renderer>();

        // 计算包围盒
        Bounds bounds = renderers[0].bounds;
        foreach (Renderer renderer in renderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }

        return bounds;
    }
}