using UnityEngine;
using Cinemachine;

public class RotateAroundTarget : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachineVirtualCamera;
    public float rotationSpeed = 10f;

    private CinemachineOrbitalTransposer orbitalTransposer;

    void Start()
    {
        // 获取CinemachineOrbitalTransposer组件
        orbitalTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
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