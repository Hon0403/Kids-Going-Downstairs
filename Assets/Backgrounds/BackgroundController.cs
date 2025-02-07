using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageManager : MonoBehaviour
{
    // 背景物件的預置體列表
    public List<GameObject> backgroundPrefabs;

    // 背景滾動速度
    public float scrollSpeed = 2f;

    // 當前顯示的背景
    private GameObject currentBackground;
    // 下一個背景
    private GameObject nextBackground;
    // 背景的高度（用於滾動判斷）
    private float backgroundHeight;

    // 在遊戲開始時執行
    void Start()
    {
        // 如果背景列表中有預置體，初始化背景
        if (backgroundPrefabs.Count > 0)
        {
            InitializeBackgrounds();
        }
    }

    // 每一幀更新
    void Update()
    {
        // 讓背景滾動
        ScrollBackground();
    }

    // 初始化背景
    private void InitializeBackgrounds()
    {
        // 設定當前背景
        SetRandomBackground();
        // 創建下一個背景
        CreateNextBackground();
    }

    // 隨機設置當前背景
    private void SetRandomBackground()
    {
        if (backgroundPrefabs.Count > 0)
        {
            // 刪除當前背景
            DestroyCurrentBackground();

            // 隨機選擇一個背景預置體
            int randomIndex = Random.Range(0, backgroundPrefabs.Count);

            // 實例化背景並設置為場景中的原點位置（或你希望的起始位置）
            Vector3 startPosition = Vector3.zero;
            currentBackground = Instantiate(backgroundPrefabs[randomIndex], startPosition, Quaternion.identity);

            // 設置背景高度
            SetBackgroundHeight(currentBackground);
            // 創建下一個背景
            CreateNextBackground();
        }
    }

    // 根據索引設置當前背景
    public void SetBackground(int index)
    {
        if (index >= 0 && index < backgroundPrefabs.Count)
        {
            // 刪除當前背景
            DestroyCurrentBackground();

            // 根據索引實例化背景
            Vector3 startPosition = Vector3.zero;
            currentBackground = Instantiate(backgroundPrefabs[index], startPosition, Quaternion.identity);

            // 設置背景高度
            SetBackgroundHeight(currentBackground);
            // 創建下一個背景
            CreateNextBackground();
        }
    }

    // 背景滾動
    private void ScrollBackground()
    {
        if (currentBackground != null && nextBackground != null)
        {
            // 讓當前背景和下一個背景向上移動
            Vector3 movement = Vector3.up * scrollSpeed * Time.deltaTime;
            currentBackground.transform.Translate(movement);
            nextBackground.transform.Translate(movement);

            // 如果當前背景完全移出遮罩顯示區域上方，刪除並切換到下一個背景
            // 假設遮罩的上邊界在場景的某個固定位置（例如 y = someValue），可以進行簡單的比較
            float offscreenPosition = 10f; // 你可以根據實際情況調整這個值
            if (currentBackground.transform.position.y - backgroundHeight / 2 > offscreenPosition)
            {
                Destroy(currentBackground); // 刪除當前背景
                currentBackground = nextBackground; // 將下一個背景設為當前背景
                CreateNextBackground();             // 創建一個新的下一個背景
            }
        }
    }

    // 創建下一個背景
    private void CreateNextBackground()
    {
        if (backgroundPrefabs.Count > 0)
        {
            // 隨機選擇一個背景預置體
            int randomIndex = Random.Range(0, backgroundPrefabs.Count);

            // 計算新背景的位置，使其與當前背景銜接
            Vector3 nextPosition = currentBackground.transform.position - new Vector3(0, backgroundHeight, 0);

            // 實例化下一個背景
            nextBackground = Instantiate(backgroundPrefabs[randomIndex], nextPosition, Quaternion.identity);

            // 設置背景高度（如果背景高度可能不同）
            SetBackgroundHeight(nextBackground);
        }
    }

    // 刪除當前背景
    private void DestroyCurrentBackground()
    {
        if (currentBackground != null)
        {
            Destroy(currentBackground); // 刪除當前背景物件
        }
    }

    // 設置背景高度，用於滾動判斷
    private void SetBackgroundHeight(GameObject background)
    {
        // 嘗試獲取 Renderer 組件
        Renderer renderer = background.GetComponent<Renderer>();
        if (renderer != null)
        {
            backgroundHeight = renderer.bounds.size.y;
        }
        else
        {
            // 如果沒有 Renderer，設置一個默認高度
            backgroundHeight = 10f; // 根據你的實際背景高度設定
        }
    }
}
