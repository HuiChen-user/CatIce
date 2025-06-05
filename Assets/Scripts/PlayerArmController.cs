using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArmController : MonoBehaviour
{
    [Header("必须在 Inspector 中分配")]
    public Transform arm;             // Arm 父节点，Pivot 在根部
    public Transform handLinePivot;   // 空物体，放在手根部位置，不动
    public Transform handLine;        // 真正带 SpriteRenderer 的物体，Pivot 在 Sprite 中心
    public Transform hand;
    public Transform handRoot;

    [Header("伸缩参数")]
    public float extendLength = 2f;   // 要把手臂伸到世界坐标中多长
    public float extendDuration = 0.1f;
    public float retractDelay = 0.5f;
    public float retractDuration = 0.1f;

    // 私有字段：缓存原始数据
    private SpriteRenderer sr;               // HandLine 上的 SpriteRenderer
    private float spriteOriginalHeight;      // Sprite 在 localScale=1 时，在世界坐标下的高度
    private float initialLocalPosY;          // 当 localScale=1 时手动对齐的 localPosition.y（＝–spriteOriginalHeight/2）
    private Coroutine activeCoroutine;       // 当前正在跑的伸缩协程

    

    void Start()
    {
        if (arm == null || handLinePivot == null || handLine == null)
        {
            Debug.LogError("请把 arm、handLinePivot、handLine 都拖到脚本里。");
            enabled = false;
            return;
        }

        sr = handLine.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogError("HandLine 需要有一个 SpriteRenderer！");
            enabled = false;
            return;
        }

        // 1. 记录 Sprite 在 localScale=1 时，在世界坐标下的实际高度
        //    注意：只有当 handLine.localScale = Vector3.one 且 handLine.localPosition 已对齐时，bounds 才是正确值
        spriteOriginalHeight = sr.bounds.size.y;

        // 2. 记录当 localScale=1 时 handLine.localPosition.y 应该是多少（通常是 –spriteOriginalHeight/2）
        //    这里假设你在场景里已经把 handLine 的 localPosition.y 设为了 –spriteOriginalHeight/2
        initialLocalPosY = handLine.localPosition.y;

        // （可选校验）如果你想自动对齐，也可以在代码里强制调整一次：
        //    initialLocalPosY = -spriteOriginalHeight / 2f;
        //    handLine.localPosition = new Vector3(handLine.localPosition.x, initialLocalPosY, handLine.localPosition.z);
    }

    void Update()
    {
        Vector3 handRootEA=handRoot.eulerAngles;
        hand.transform.eulerAngles=new Vector3(handRootEA.x,handRootEA.y,handRootEA.z-90);
        hand.transform.position = handRoot.transform.position;
        if (Input.GetMouseButtonDown(0))
        {
            // 1. 把鼠标位置从屏幕坐标转到世界坐标
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0f;

            // 2. 计算方向向量，并让 arm 朝向鼠标（同样要根据你贴图朝向做偏移，比如 –90° 或 +90°）
            Vector3 dir = mouseWorld - handLinePivot.position;
            if (dir.sqrMagnitude < 0.00001f) return;

            float angleDeg = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f;
            // 如果你的 HandLine 贴图默认朝 –Y，再把 –90° 改成 +90° 就行
            arm.rotation = Quaternion.Euler(0f, 0f, angleDeg);

            // 3. 停止当前协程（如果有），再重新开启
            if (activeCoroutine != null)
            {
                StopCoroutine(activeCoroutine);
            }
            activeCoroutine = StartCoroutine(ExtendThenRetract());
        }
    }

    private System.Collections.IEnumerator ExtendThenRetract()
    {
        // —— 伸长阶段 —— 
        // 目标世界长度 extendLength，要算出对应的 localScale.y
        // spriteOriginalHeight = sr.bounds.size.y（当 localScale=1 时）
        float desiredScaleY = extendLength / spriteOriginalHeight;

        // 从当前 localScale.y 平滑插值到 desiredScaleY
        float startScaleY = handLine.localScale.y;
        float elapsed = 0f;
        if (extendDuration <= 0f)
        {
            // 瞬时伸长
            SetHandLineScaleAndPos(desiredScaleY);
        }
        else
        {
            while (elapsed < extendDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / extendDuration);
                float curScale = Mathf.Lerp(startScaleY, desiredScaleY, t);
                SetHandLineScaleAndPos(curScale);
                yield return null;
            }
            // 确保最后值精确
            SetHandLineScaleAndPos(desiredScaleY);
        }

        // —— 等待 retractDelay 秒 —— 
        yield return new WaitForSeconds(retractDelay);

        // —— 收回阶段 —— 
        elapsed = 0f;
        if (retractDuration <= 0f)
        {
            SetHandLineScaleAndPos(1f);
        }
        else
        {
            while (elapsed < retractDuration)
            {
                elapsed += Time.deltaTime;
                float t2 = Mathf.Clamp01(elapsed / retractDuration);
                float curScale2 = Mathf.Lerp(desiredScaleY, 1f, t2);
                SetHandLineScaleAndPos(curScale2);
                yield return null;
            }
            SetHandLineScaleAndPos(1f);
        }

        activeCoroutine = null;
    }

    /// <summary>
    /// 同时设置 handLine.localScale.y 并校正 handLine.localPosition.y，
    /// 保证手根部（handLinePivot）不动。
    /// </summary>
    private void SetHandLineScaleAndPos(float newScaleY)
    {
        // 1. 设置缩放
        Vector3 ls = handLine.localScale;
        ls.y = newScaleY;
        handLine.localScale = ls;

        // 2. 设置 localPosition.y
        //    当 localScale=1 时，initialLocalPosY = –spriteOriginalHeight/2；
        //    当 localScale=newScaleY 时，Sprite 的中心相对 handLinePivot 下移 newScaleY 倍，
        //    所以 newLocalPosY = initialLocalPosY * newScaleY
        Vector3 lp = handLine.localPosition;
        lp.y = initialLocalPosY * newScaleY;
        handLine.localPosition = lp;
    }
}
