using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitPokemonComponets : MonoBehaviour
{    
    private Rigidbody rb;
    private BoxCollider boxCollider;
    private Renderer[] renderers;
    public float BattleWaitSpeed = 1;
    public Vector3 BoundsSize;
    public List<GameObject> HideWhenTouched = new List<GameObject>();
    private Animator Anim;
    public Transform CenterPosition;
    public Transform TouchHitPosition;
    public bool Levitate = false;
    public float yOffset = 0.5f; // 调整的Y轴偏移量

    public bool MegaUpgrade;
    public bool SpecialBounding;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        renderers = GetComponentsInChildren<Renderer>();
        AdjustColliderBounds();
        rb = GetComponent<Rigidbody>();
        Anim = GetComponent<Animator>();
        if(BattleWaitSpeed == 0)
            BattleWaitSpeed = 1;
        Anim.speed = BattleWaitSpeed;
        if (Levitate)
        {
            rb.useGravity = false;  
        }
    }
    void OnEnable()
    {
        if(MegaUpgrade)
        {
            this.transform.localPosition = new Vector3(0.0f, 0.0f, 1.48f);
            if(SpecialBounding)
            {
                this.transform.localPosition = new Vector3(0.0f, 0.0f, 0.2f);
            }
        }
        else
        {
            if(SpecialBounding)
            {
                this.transform.localPosition = new Vector3(0.0f, 0.0f, -1.48f);
            }
            else
            {
                this.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            }
        }
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
            //if(renderers[i].gameObject.active == true)
            {
                combinedBounds.Encapsulate(renderers[i].bounds);
            }
        }
        
        // 设置BoxCollider的大小和中心点
        // boxCollider.center = combinedBounds.center - transform.position;
        boxCollider.size = combinedBounds.size;

        if(SpecialBounding)
        {
            boxCollider.size = new Vector3
            (boxCollider.size.x / this.transform.lossyScale.x, 
            boxCollider.size.y / this.transform.lossyScale.y, 
            boxCollider.size.z / this.transform.lossyScale.z);
            boxCollider.center = new Vector3(0, -1, 0);
        }
        else
        {
            boxCollider.center = new Vector3(0, 0, -0.5f * boxCollider.size.z);
        }
        BoundsSize = combinedBounds.size;
    }

    // Update is called once per frame    
    void Update()
    {
        if (Levitate)
        {
            // 如果距离地面高度大于停止高度，则继续下落
            if (this.gameObject.transform.position.y > yOffset)
            {
                // 按照指定速度向下移动
                rb.velocity = Vector3.down * 1.0f;
            }
            else
            {
                // 下落到指定高度后停止下落
                rb.velocity = Vector3.zero;
            }
        }
    }
}
