using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Camera : MonoBehaviour
{
    [Header("—— 目标与参数 ——")]
    [Tooltip("玩家 Transform，用于后续按 Y 轴跟随")]
    public Transform player;

    [Tooltip("初始向 (0, -10, -10) 平移所需时长（秒）")]
    public float moveDuration = 1f;

    [Tooltip("抖动持续时长（秒）")]
    public float shakeDuration = 1f;

    [Tooltip("抖动幅度，单位世界坐标")]
    public float shakeMagnitude = 0.5f;

    [Tooltip("抖动平滑衰减标记（若想让抖动逐渐靠近零，可设为 true）")]
    public bool shakeDiminish = false;

    [Tooltip("在抖动过程中是否也跟随玩家 X/Y（一般抖动时不跟随，故默认 false）")]
    public bool shakeFollowPlayer = false;

    // 内部使用变量
    private Vector3 originalPosition;     // 原始摄像机位置，假设为 (0,0,-10)
    private bool followPlayerY = false;   // 进入跟随玩家 Y 轴的标记

    void Start()
    {
        // 记录摄像机初始位置。一般在 2D 项目里，主摄像机的初始位置就是 (0,0,-10)。
        originalPosition = transform.position;

        // 启动协程：平移 → 抖动 → 回到原位 → 开启跟随模式
        StartCoroutine(CameraSequence());
    }

    /// <summary>
    /// 整个摄像机流程：移动→抖动→回到原位→开启跟随
    /// </summary>
    private IEnumerator CameraSequence()
    {
        // 1. 平移到 (0, -10, -10)
        Vector3 targetDown = new Vector3(0f, -7f, -10f);
        yield return StartCoroutine(MoveCameraOverTime(targetDown, moveDuration));

        // 2. 抖动 1 秒
        yield return StartCoroutine(ShakeCamera(shakeDuration, shakeMagnitude, shakeDiminish));

        // 3. 平移回原始位置 (0, 0, -10)
        yield return StartCoroutine(MoveCameraOverTime(originalPosition, moveDuration));

        // 4. 开启跟随玩家 Y 轴
        followPlayerY = true;
    }

    /// <summary>
    /// 将摄像机从当前位置平滑移动到 targetPos，用时 duration（秒）
    /// </summary>
    private IEnumerator MoveCameraOverTime(Vector3 targetPos, float duration)
    {
        Vector3 startPos = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            // 这里用 Lerp 让摄像机匀速或匀加速/匀减速插值，可按需改用 SmoothDamp
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        // 确保最终位置完全到达
        transform.position = targetPos;
    }

    /// <summary>
    /// 简单的摄像机抖动效果：在抖动期间，每帧给一个随机偏移
    /// </summary>
    /// <param name="duration">抖动总时长（秒）</param>
    /// <param name="magnitude">抖动幅度</param>
    /// <param name="diminish">是否让抖动幅度随时间线性衰减</param>
    private IEnumerator ShakeCamera(float duration, float magnitude, bool diminish)
    {
        float elapsed = 0f;
        Vector3 basePos = transform.position; // 抖动开始时的基准位置

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float percentComplete = elapsed / duration;

            // 计算当前抖动幅度，如果需要衰减则随着时间剩余线性减少
            float currentMag = magnitude;
            if (diminish)
            {
                currentMag = Mathf.Lerp(magnitude, 0f, percentComplete);
            }

            // 随机 x 和 y 的偏移
            float offsetX = (Random.value * 2f - 1f) * currentMag;
            float offsetY = (Random.value * 2f - 1f) * currentMag;

            Vector3 offset = new Vector3(offsetX, offsetY, 0f);

            if (shakeFollowPlayer && player != null)
            {
                // 如果抖动过程中也想同时让摄像机大体跟随玩家，可以在这里叠加玩家 Y 轴逻辑
                float followY = player.position.y;
                transform.position = new Vector3(basePos.x, followY, basePos.z) + offset;
            }
            else
            {
                // 仅在基准点附近抖动，不跟随玩家
                transform.position = basePos + offset;
            }

            yield return null;
        }

        // 抖动结束后，重置到基准位置（以防随机抖动没精确回到原位）
        transform.position = basePos;
    }

    void LateUpdate()
    {
        // 在每帧最后阶段，如果进入了跟随模式，就让摄像机的 Y 与玩家保持一致
        if (followPlayerY && player != null)
        {
            Vector3 pos = transform.position;
            pos.y = player.position.y;
            // 注意保持 z=-10
            pos.z = originalPosition.z;
            transform.position = pos;
        }
    }
}
