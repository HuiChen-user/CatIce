using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Camera : MonoBehaviour
{
    [Header("���� Ŀ������� ����")]
    [Tooltip("��� Transform�����ں����� Y �����")]
    public Transform player;

    [Tooltip("��ʼ�� (0, -10, -10) ƽ������ʱ�����룩")]
    public float moveDuration = 1f;

    [Tooltip("��������ʱ�����룩")]
    public float shakeDuration = 1f;

    [Tooltip("�������ȣ���λ��������")]
    public float shakeMagnitude = 0.5f;

    [Tooltip("����ƽ��˥����ǣ������ö����𽥿����㣬����Ϊ true��")]
    public bool shakeDiminish = false;

    [Tooltip("�ڶ����������Ƿ�Ҳ������� X/Y��һ�㶶��ʱ�����棬��Ĭ�� false��")]
    public bool shakeFollowPlayer = false;

    // �ڲ�ʹ�ñ���
    private Vector3 originalPosition;     // ԭʼ�����λ�ã�����Ϊ (0,0,-10)
    private bool followPlayerY = false;   // ���������� Y ��ı��

    void Start()
    {
        // ��¼�������ʼλ�á�һ���� 2D ��Ŀ���������ĳ�ʼλ�þ��� (0,0,-10)��
        originalPosition = transform.position;

        // ����Э�̣�ƽ�� �� ���� �� �ص�ԭλ �� ��������ģʽ
        StartCoroutine(CameraSequence());
    }

    /// <summary>
    /// ������������̣��ƶ����������ص�ԭλ����������
    /// </summary>
    private IEnumerator CameraSequence()
    {
        // 1. ƽ�Ƶ� (0, -10, -10)
        Vector3 targetDown = new Vector3(0f, -7f, -10f);
        yield return StartCoroutine(MoveCameraOverTime(targetDown, moveDuration));

        // 2. ���� 1 ��
        yield return StartCoroutine(ShakeCamera(shakeDuration, shakeMagnitude, shakeDiminish));

        // 3. ƽ�ƻ�ԭʼλ�� (0, 0, -10)
        yield return StartCoroutine(MoveCameraOverTime(originalPosition, moveDuration));

        // 4. ����������� Y ��
        followPlayerY = true;
    }

    /// <summary>
    /// ��������ӵ�ǰλ��ƽ���ƶ��� targetPos����ʱ duration���룩
    /// </summary>
    private IEnumerator MoveCameraOverTime(Vector3 targetPos, float duration)
    {
        Vector3 startPos = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            // ������ Lerp ����������ٻ��ȼ���/�ȼ��ٲ�ֵ���ɰ������ SmoothDamp
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        // ȷ������λ����ȫ����
        transform.position = targetPos;
    }

    /// <summary>
    /// �򵥵����������Ч�����ڶ����ڼ䣬ÿ֡��һ�����ƫ��
    /// </summary>
    /// <param name="duration">������ʱ�����룩</param>
    /// <param name="magnitude">��������</param>
    /// <param name="diminish">�Ƿ��ö���������ʱ������˥��</param>
    private IEnumerator ShakeCamera(float duration, float magnitude, bool diminish)
    {
        float elapsed = 0f;
        Vector3 basePos = transform.position; // ������ʼʱ�Ļ�׼λ��

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float percentComplete = elapsed / duration;

            // ���㵱ǰ�������ȣ������Ҫ˥��������ʱ��ʣ�����Լ���
            float currentMag = magnitude;
            if (diminish)
            {
                currentMag = Mathf.Lerp(magnitude, 0f, percentComplete);
            }

            // ��� x �� y ��ƫ��
            float offsetX = (Random.value * 2f - 1f) * currentMag;
            float offsetY = (Random.value * 2f - 1f) * currentMag;

            Vector3 offset = new Vector3(offsetX, offsetY, 0f);

            if (shakeFollowPlayer && player != null)
            {
                // �������������Ҳ��ͬʱ����������������ң����������������� Y ���߼�
                float followY = player.position.y;
                transform.position = new Vector3(basePos.x, followY, basePos.z) + offset;
            }
            else
            {
                // ���ڻ�׼�㸽�����������������
                transform.position = basePos + offset;
            }

            yield return null;
        }

        // �������������õ���׼λ�ã��Է��������û��ȷ�ص�ԭλ��
        transform.position = basePos;
    }

    void LateUpdate()
    {
        // ��ÿ֡���׶Σ���������˸���ģʽ������������� Y ����ұ���һ��
        if (followPlayerY && player != null)
        {
            Vector3 pos = transform.position;
            pos.y = player.position.y;
            // ע�Ᵽ�� z=-10
            pos.z = originalPosition.z;
            transform.position = pos;
        }
    }
}
