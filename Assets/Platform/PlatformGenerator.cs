using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public GameObject[] platformPrefabs; // 不同的平臺預製體
    public Transform player; // 玩家物件
    public Transform parentContainer; // 容納平臺的容器
    public SpriteMask backgroundMask; // 引用遮罩物件
    public float platformPadding = 1f; // 平臺與邊界的間距
    public float generationInterval = 2f; // 平臺生成的間隔時間
    private float nextGenerationTime = 0f; // 下一次生成平臺的時間
    public int maxPlatforms = 10; // 最大平臺數量
    private int currentPlatformsCount = 0; // 當前已生成的平臺數量

    // 定義事件，當平台生成後觸發
    public delegate void PlatformGeneratedHandler(GameObject platform);
    public static event PlatformGeneratedHandler OnPlatformGenerated;

    void Start()
    {
        if (platformPrefabs.Length == 0)
        {
            Debug.LogError("沒有設置任何平臺預製體！");
            return;
        }
        // 立即生成一次平臺
        GeneratePlatforms();
    }

    void Update()
    {
        // 每隔一定時間生成平臺
        if (Time.time >= nextGenerationTime && currentPlatformsCount < maxPlatforms)
        {
            GeneratePlatforms();
            nextGenerationTime = Time.time + generationInterval;
        }
        // 清理超出視野的平臺
        RemoveOldPlatforms();
    }

    void RemoveOldPlatforms()
    {
        foreach (Transform platform in parentContainer)
        {
            if (platform.position.y < player.position.y - 10f)
            {
                Destroy(platform.gameObject);
                currentPlatformsCount--;
            }
        }
    }

    void GeneratePlatforms()
    {
        float maskTop = backgroundMask.transform.position.y + (backgroundMask.bounds.size.y / 2f);
        float maskBottom = backgroundMask.transform.position.y - (backgroundMask.bounds.size.y / 2f);
        float maskLeft = backgroundMask.transform.position.x - (backgroundMask.bounds.size.x / 2f);
        float maskRight = backgroundMask.transform.position.x + (backgroundMask.bounds.size.x / 2f);

        GameObject selectedPlatformPrefab = platformPrefabs[Random.Range(0, platformPrefabs.Length)];
        GameObject newPlatform = Instantiate(selectedPlatformPrefab, parentContainer);

        SpriteRenderer renderer = newPlatform.GetComponent<SpriteRenderer>();
        float platformWidth = renderer.bounds.size.x;
        float platformHeight = renderer.bounds.size.y;

        float xMin = maskLeft + platformWidth / 2f + platformPadding;
        float xMax = maskRight - platformWidth / 2f - platformPadding;
        float xPosition = Random.Range(xMin, xMax);

        // **改變這一行**
        float yPosition = maskTop - (currentPlatformsCount * (platformHeight + platformPadding));  // 依序從上往下排列

        // 確保不超過遮罩範圍
        if (yPosition < maskBottom + platformHeight / 2f)
        {
            Debug.Log("已達到遮罩底部，停止生成平台");
            return;
        }

        newPlatform.transform.position = new Vector3(xPosition, yPosition, 0f);
        currentPlatformsCount++;

        // 通知平台生成事件
        OnPlatformGenerated?.Invoke(newPlatform);
    }


}
