using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorAnimationReceiver : ParameterizedSignalReceiver
{
    private Material SelfMaterial;
    public float Duration = 1f;
    private float ElapsedTime = 0; // 已过时间
    private float PrevV = 0.5f;
    private float TargetV = 0.0f;
    private bool Play = false;

    public void Start()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        SelfMaterial = meshRenderer.material;
    }
    public void Update()
    {
        if(!Play)
        {
            return;
        }
        ElapsedTime += Time.deltaTime; // 更新已过时间
        float fraction = ElapsedTime / Duration; // 计算已完成的动画比例

        if (fraction <= 1) // 确保fraction不会超过1
        {
            SelfMaterial.SetFloat("_FloorV", TargetV * fraction + (1.0f - fraction) * PrevV);
        }
        else
        {
            SelfMaterial.SetFloat("_FloorV", TargetV);
            Play = false;
        }
    }
    public override void Process(SignalWithParams InSignal)
    {
        string Text = InSignal.GetParamValue("FloorAnimationType");
        if(Text == "Dark")
        {
            PrevV = SelfMaterial.GetFloat("_FloorV");
            TargetV = 0.0f;
            Play = true;
            ElapsedTime = 0.0f;
        }
        else if(Text == "Reset")
        {
            TargetV = PrevV;
            PrevV = SelfMaterial.GetFloat("_FloorV");
            Play = true;
            ElapsedTime = 0.0f;
        }
    }
}
