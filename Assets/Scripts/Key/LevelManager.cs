using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("���������� KeyHole")]
    public List<KeyHole> allKeyHoles = new List<KeyHole>();

    [Header("���ŵĿ��ƽű�")]
    public DoorController doorController;

    private int totalHoleCount = 0;
    private int matchedCount = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {

        totalHoleCount = allKeyHoles.Count;
        matchedCount = 0;

        

        // ��ÿ�� KeyHole ���һ��������ƥ������ OnOneHoleMatched()
        foreach (KeyHole hole in allKeyHoles)
        {
            hole.onKeyMatched.AddListener(OnOneHoleMatched);
        }
    }

    /// <summary>
    /// ĳ�� KeyHole ƥ��󣬻���õ�����
    /// </summary>
    private void OnOneHoleMatched(KeyHole hole)
    {
        matchedCount++;
        Debug.Log($"LockHole {hole.name} ��ƥ�䣬�ܽ��� {matchedCount}/{totalHoleCount}");

        if (matchedCount >= totalHoleCount)
        {
            // ȫ�����׶�ƥ����ϣ�����
            if (doorController != null)
            {
                doorController.OpenDoor();
            }
        }
    }

    /// <summary>
    ///���ùؿ�
    /// </summary>
    public void ResetAllHoles()
    {
        matchedCount = 0;
        foreach (KeyHole hole in allKeyHoles)
        {
            hole.IsMatched = false;
            //�� Key �����������ģ������� Key ���»ص���ʼλ��,��������ùؿ���д����
        }
    }
}
