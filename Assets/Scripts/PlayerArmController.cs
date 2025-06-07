using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArmController : MonoBehaviour
{
    [Header("������ Inspector �з���")]
    public Transform arm;             // Arm ���ڵ㣬Pivot �ڸ���
    public Transform handLinePivot;   // �����壬�����ָ���λ�ã�����
    public Transform handLine;        // ������ SpriteRenderer �����壬Pivot �� Sprite ����
    public Transform hand;
    public Transform handRoot;
    public HookAttachment ha;
    private bool isExtendThenRetractRun=false;

    [Header("��������")]
    public float extendLength = 2f;   // Ҫ���ֱ��쵽���������ж೤
    public float extendDuration = 0.1f;
    public float retractDelay = 0.5f;
    public float retractDuration = 0.1f;

    // ˽���ֶΣ�����ԭʼ����
    private SpriteRenderer sr;               // HandLine �ϵ� SpriteRenderer
    private float spriteOriginalHeight;      // Sprite �� localScale=1 ʱ�������������µĸ߶�
    private float initialLocalPosY;          // �� localScale=1 ʱ�ֶ������ localPosition.y�����CspriteOriginalHeight/2��
    private Coroutine activeCoroutine;       // ��ǰ�����ܵ�����Э��

    public static bool canMove = true;
    public static bool canExtend=true;

    void Start()
    {
        ha=hand.GetComponent<HookAttachment>();
        if (arm == null || handLinePivot == null || handLine == null)
        {
            Debug.LogError("��� arm��handLinePivot��handLine ���ϵ��ű��");
            enabled = false;
            return;
        }

        sr = handLine.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogError("HandLine ��Ҫ��һ�� SpriteRenderer��");
            enabled = false;
            return;
        }

        // 1. ��¼ Sprite �� localScale=1 ʱ�������������µ�ʵ�ʸ߶�
        //    ע�⣺ֻ�е� handLine.localScale = Vector3.one �� handLine.localPosition �Ѷ���ʱ��bounds ������ȷֵ
        spriteOriginalHeight = sr.bounds.size.y;

        // 2. ��¼�� localScale=1 ʱ handLine.localPosition.y Ӧ���Ƕ��٣�ͨ���� �CspriteOriginalHeight/2��
        //    ����������ڳ������Ѿ��� handLine �� localPosition.y ��Ϊ�� �CspriteOriginalHeight/2
        initialLocalPosY = handLine.localPosition.y;

        // ����ѡУ�飩��������Զ����룬Ҳ�����ڴ�����ǿ�Ƶ���һ�Σ�
        //    initialLocalPosY = -spriteOriginalHeight / 2f;
        //    handLine.localPosition = new Vector3(handLine.localPosition.x, initialLocalPosY, handLine.localPosition.z);
    }

    void Update()
    {
        if (ha.isGrabbing)
        {
            canExtend = false;
        }
        if(!ha.isGrabbing&&!isExtendThenRetractRun)
        {
            canExtend=true;
        }
        Vector3 handRootEA=handRoot.eulerAngles;
        hand.transform.eulerAngles=new Vector3(handRootEA.x,handRootEA.y,handRootEA.z-90);
        hand.transform.position = handRoot.transform.position;
        if (Input.GetMouseButtonDown(0)&&canExtend)
        {
            // 1. �����λ�ô���Ļ����ת����������
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0f;

            // 2. ���㷽������������ arm ������꣨ͬ��Ҫ��������ͼ������ƫ�ƣ����� �C90�� �� +90�㣩
            Vector3 dir = mouseWorld - handLinePivot.position;
            if (dir.sqrMagnitude < 0.00001f) return;

            float angleDeg = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f;
            // ������ HandLine ��ͼĬ�ϳ� �CY���ٰ� �C90�� �ĳ� +90�� ����
            arm.rotation = Quaternion.Euler(0f, 0f, angleDeg);

            // 3. ֹͣ��ǰЭ�̣�����У��������¿���
            if (activeCoroutine != null)
            {
                StopCoroutine(activeCoroutine);
            }
            activeCoroutine = StartCoroutine(ExtendThenRetract());
        }
    }

    private System.Collections.IEnumerator ExtendThenRetract()
    {
        AudioEvent.RaiseOnPlayAudio(AudioType.Hand);
        canExtend = false;
        isExtendThenRetractRun = true;
        canMove = false;
        // ���� �쳤�׶� ���� 
        // Ŀ�����糤�� extendLength��Ҫ�����Ӧ�� localScale.y
        // spriteOriginalHeight = sr.bounds.size.y���� localScale=1 ʱ��
        float desiredScaleY = extendLength / spriteOriginalHeight;

        // �ӵ�ǰ localScale.y ƽ����ֵ�� desiredScaleY
        float startScaleY = handLine.localScale.y;
        float elapsed = 0f;
        if (extendDuration <= 0f)
        {
            // ˲ʱ�쳤
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
            // ȷ�����ֵ��ȷ
            SetHandLineScaleAndPos(desiredScaleY);
        }

        // ���� �ȴ� retractDelay �� ���� 
        yield return new WaitForSeconds(retractDelay);

        // ���� �ջؽ׶� ���� 
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
        canMove = true;
        canExtend = true;
        
        isExtendThenRetractRun = false;
    }

    /// <summary>
    /// ͬʱ���� handLine.localScale.y ��У�� handLine.localPosition.y��
    /// ��֤�ָ�����handLinePivot��������
    /// </summary>
    private void SetHandLineScaleAndPos(float newScaleY)
    {
        // 1. ��������
        Vector3 ls = handLine.localScale;
        ls.y = newScaleY;
        handLine.localScale = ls;

        // 2. ���� localPosition.y
        //    �� localScale=1 ʱ��initialLocalPosY = �CspriteOriginalHeight/2��
        //    �� localScale=newScaleY ʱ��Sprite ��������� handLinePivot ���� newScaleY ����
        //    ���� newLocalPosY = initialLocalPosY * newScaleY
        Vector3 lp = handLine.localPosition;
        lp.y = initialLocalPosY * newScaleY;
        handLine.localPosition = lp;
    }
}
