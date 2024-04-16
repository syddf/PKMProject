using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float shakeTimer;
    public RectTransform uiElement;
    private Vector3 originalPosition;

    public void Start()
    {
        if(uiElement)
        {
            originalPosition = uiElement.position;
        }
    }
    private void Awake()
    {
        Instance = this;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        CinemachineBasicMultiChannelPerlin perlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = 0f;
    }

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin perlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        perlin.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }


    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin perlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                perlin.m_AmplitudeGain = 0f;
                if(uiElement)
                {
                    uiElement.position = originalPosition;
                }
            }

            // 获取相机震动的偏移量
            Vector3 shakeOffset = cinemachineVirtualCamera.transform.position - cinemachineVirtualCamera.Follow.position;
            System.Random rnd = new System.Random();
            int rand1 = rnd.Next(0, 5);
            int rand2 = rnd.Next(0, 5);
            // 更新UI元素的位置，将震动偏移量应用到UI元素上
            if(uiElement)
            {
                uiElement.position = new Vector3(uiElement.position.x + rand1 - 2, uiElement.position.y + rand2 - 2, uiElement.position.z);
            }
        }
    }

    private void LateUpdate()
    {

    }
}