using UnityEngine;
using Cinemachine;

public class RotateAroundTarget : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachineVirtualCamera;
    public float rotationSpeed = 10f;
    public int startMin;
    public int startMax;
    private CinemachineOrbitalTransposer orbitalTransposer;

    public Transform OriginCamera;
    private bool Reset;
    void OnEnable()
    {
        this.transform.position = OriginCamera.position;
        this.transform.rotation = OriginCamera.rotation;
        this.transform.localScale = OriginCamera.localScale;
        if (orbitalTransposer != null)
        {
            System.Random rnd = new System.Random();
            int rand = rnd.Next(startMin, startMax);
            orbitalTransposer.m_XAxis.Value = rand;
        }
    }
    void Start()
    {
        // 获取CinemachineOrbitalTransposer组件
        orbitalTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        this.transform.position = OriginCamera.position;
        this.transform.rotation = OriginCamera.rotation;
        this.transform.localScale = OriginCamera.localScale;
    }

    void Update()
    {
        if (orbitalTransposer != null)
        {
            // 绕目标旋转
            orbitalTransposer.m_XAxis.Value += rotationSpeed * Time.deltaTime;
        }
    }
}