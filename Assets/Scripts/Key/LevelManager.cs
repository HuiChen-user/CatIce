using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("场景中所有 KeyHole")]
    public List<KeyHole> allKeyHoles = new List<KeyHole>();

    [Header("绑定门的控制脚本")]
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

        

        // 给每个 KeyHole 添加一个监听：匹配后调用 OnOneHoleMatched()
        foreach (KeyHole hole in allKeyHoles)
        {
            hole.onKeyMatched.AddListener(OnOneHoleMatched);
        }
    }

    /// <summary>
    /// 某个 KeyHole 匹配后，会调用到这里
    /// </summary>
    private void OnOneHoleMatched(KeyHole hole)
    {
        matchedCount++;
        Debug.Log($"LockHole {hole.name} 已匹配，总进度 {matchedCount}/{totalHoleCount}");

        if (matchedCount >= totalHoleCount)
        {
            // 全部锁孔都匹配完毕，打开门
            if (doorController != null)
            {
                doorController.OpenDoor();
            }
        }
    }

    /// <summary>
    ///重置关卡
    /// </summary>
    public void ResetAllHoles()
    {
        matchedCount = 0;
        foreach (KeyHole hole in allKeyHoles)
        {
            hole.IsMatched = false;
            //若 Key 被锁定在中心，还需让 Key 重新回到初始位置,如果有重置关卡再写好了
        }
    }
}
