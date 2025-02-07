using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int platformStepCount = 0; // 玩家踩過的平台數量

    // 新增 UI Text 來顯示步數
    public TextMeshProUGUI stepCounterText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // 訂閱平台生成事件
            PlatformGenerator.OnPlatformGenerated += HandlePlatformGenerated;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 當平台生成時處理（可以用來更新其他關卡邏輯）
    void HandlePlatformGenerated(GameObject platform)
    {
        Debug.Log("生成了一個新的平台：" + platform.name);
    }

    // 當玩家踩到平台時呼叫此方法
    public void IncrementPlatformStep()
    {
        platformStepCount++;
        Debug.Log("玩家已踩的平台數：" + platformStepCount);
        UpdateStepCounterUI();
    }

    // 更新 UI Text 上的步數顯示
    private void UpdateStepCounterUI()
    {
        if (stepCounterText != null)
        {
            stepCounterText.text = "地下樓層:" + platformStepCount.ToString();
        }
    }

    void OnDestroy()
    {
        // 記得取消訂閱事件，避免記憶體洩漏
        PlatformGenerator.OnPlatformGenerated -= HandlePlatformGenerated;
    }
}
