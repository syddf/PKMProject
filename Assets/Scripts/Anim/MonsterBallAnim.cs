using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MonsterBallAnim : MonoBehaviour
{
    public Transform target; // 目标点Transform
    public AnimationCurve yCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 0)); // Y轴动画曲线
    private Vector3 startPosition; // 开始位置
    private float totalDistance; // 初始和目标位置之间的距离
    private float duration = 1f; // 完成移动所需的时间，假设游戏以60fps运行，则30帧相当于0.5秒
    private float elapsedTime = 0; // 已过时间
    private float startY = 0;
    private float rotation = 360;

    void Start()
    {
        startPosition = transform.position; // 记录开始位置
        startY = startPosition.y;
        totalDistance = Vector3.Distance(transform.position, target.position); // 计算总距离
    }

    void OnEnable() 
    {
        elapsedTime = 0;
    }

    void Update()
    {
        if (target != null)
        {
            elapsedTime += Time.deltaTime; // 更新已过时间
            float fraction = elapsedTime / duration; // 计算已完成的动画比例

            if (fraction <= 1) // 确保fraction不会超过1
            {
                // 使用动画曲线计算Y轴的偏移量
                float yValue = yCurve.Evaluate(fraction);

                // 计算当前位置
                Vector3 currentPosition = Vector3.Lerp(startPosition, new Vector3(target.position.x, startPosition.y, target.position.z), fraction);
                currentPosition.y = startY + yValue; // 应用Y轴的偏移量
                transform.eulerAngles = new Vector3(rotation * fraction, transform.eulerAngles.y, transform.eulerAngles.z);
                transform.position = currentPosition; // 更新物体位置
            }
            else
            {
                // 确保物体精确到达目标点，特别是当fraction稍微超过1时
                //transform.position = new Vector3(target.position.x, startY + yCurve.Evaluate(1), target.position.z);
                transform.position = startPosition;
                this.gameObject.SetActive(false);
            }
        }
    }
}