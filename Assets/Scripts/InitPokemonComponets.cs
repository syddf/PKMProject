using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitPokemonComponets : MonoBehaviour
{    
    private Rigidbody rb;
    private BoxCollider boxCollider;
    private Renderer[] renderers;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        renderers = GetComponentsInChildren<Renderer>();
        AdjustColliderBounds();
        rb = GetComponent<Rigidbody>();
    }
    void OnEnable()
    {
        this.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
    }
    void AdjustColliderBounds()
    {
        // 如果没有Renderer，直接退出
        if (renderers.Length == 0)
        {
            Debug.LogWarning("No renderers found to adjust collider bounds.");
            return;
        }

        // 初始化包围盒范围
        Bounds combinedBounds = renderers[0].bounds;
        
        // 合并所有Renderer的包围盒范围
        for (int i = 1; i < renderers.Length; i++)
        {
            combinedBounds.Encapsulate(renderers[i].bounds);
        }

        // 设置BoxCollider的大小和中心点
        // boxCollider.center = combinedBounds.center - transform.position;
        boxCollider.size = combinedBounds.size;
        boxCollider.center = new Vector3(0, 0, -0.5f * boxCollider.size.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
