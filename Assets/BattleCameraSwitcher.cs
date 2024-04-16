using UnityEngine;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;

public class BattleCameraSwitcher : MonoBehaviour
{
    public List<CinemachineVirtualCamera> cameraList = new List<CinemachineVirtualCamera>(); // 相机列表
    private bool switchEnabled = true; // 控制相机切换是否启用

    private IEnumerator switchCoroutine;
    private CinemachineVirtualCamera lastCamera;
    private void Start()
    {
        // 开始相机切换协程
        switchCoroutine = SwitchCameraRoutine();
        StartCoroutine(switchCoroutine);
    }

    // 控制相机切换的协程
    private IEnumerator SwitchCameraRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(8f); // 等待3秒

            if (switchEnabled)
            {
                // 选择一个随机相机并激活
                ActivateRandomCamera();
            }
        }
    }

    // 选择一个随机相机并激活
    private void ActivateRandomCamera()
    {
        if (cameraList.Count == 0)
        {
            Debug.LogWarning("Camera list is empty.");
            return;
        }

        // 随机选择一个相机
        int randomIndex = Random.Range(0, cameraList.Count);
        CinemachineVirtualCamera randomCamera = cameraList[randomIndex];
        if(randomCamera == lastCamera)
        {
            randomCamera = cameraList[(randomIndex + 1) % cameraList.Count];
        }
        // 激活随机相机
        SetActiveCamera(randomCamera);
    }

    // 激活相机
    private void SetActiveCamera(CinemachineVirtualCamera camera)
    {
        // 禁用其他相机
        foreach (CinemachineVirtualCamera cam in cameraList)
        {
            cam.gameObject.SetActive(false);
        }

        // 激活目标相机
        camera.gameObject.SetActive(true);
        lastCamera = camera;
    }

    // 关闭相机切换功能
    public void DisableSwitching()
    {
        switchEnabled = false;
        StopCoroutine(switchCoroutine);
        foreach (CinemachineVirtualCamera cam in cameraList)
        {
            cam.gameObject.SetActive(false);
        }
    }

    // 启用相机切换功能
    public void EnableSwitching()
    {
        switchEnabled = true;
        switchCoroutine = SwitchCameraRoutine();
        StartCoroutine(switchCoroutine);
    }
}
