using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoleRun : MonoBehaviour
{
    //Vector3 是 Unity 中的一個結構，用來表示三維空間中的向量和點。
    //它包含三個浮點數值，代表空間中的 x、y 和 z 三個軸。Vector3 非常常見，用於位置、方向、縮放等多種情況。

    //Vector3 與 transform.Translate 之間密切相關。
    //transform.Translate 方法是用來移動物體的位置，而 Vector3 用於表示這個移動的方向和距離。

    public float speed = 0.1f; // 控制角色移動的速度
    public float sprintSpeed = 0.2f; // 衝刺速度，可以在介面中調整

    private Vector3 direction; // 儲存移動方向
    private Rigidbody2D rb; // 角色的Rigidbody2D組件
    public Animator roleani;
    public SpriteRenderer SpriteRenderer;

    public float jumpForce = 10f;  // 跳躍力量
    public Transform groundCheck;  // 檢查地面的物件（可以是角色的腳）
    public LayerMask LayerMask;  // 設定為地面層

    private bool isGrounded;  // 是否在地面上
    private bool isSprinting; // 是否在衝刺


    // Start is called before the first frame update
    void Start()
    {
        // 獲取Rigidbody2D組件
        rb = GetComponent<Rigidbody2D>();
    }

    // FixedUpdate 和 Update 是 Unity 中用來控制遊戲邏輯更新的兩種方法。
    // 它們的主要差別在於「執行頻率」和「適合的用途」。

    // Update 方法：
    // - 每一幀（畫面更新一次）執行一次。
    // - 幀率越高，執行次數越多，幀率越低，執行次數越少。
    // - 適合處理玩家輸入、動畫觸發或一般遊戲邏輯。
    //   比如：偵測按鍵是否被按下，或讓角色根據輸入移動。
    // - 缺點：因為執行頻率不穩定，處理物理運算時可能會不準確。

    // FixedUpdate 方法：
    // - 每隔固定時間執行一次（預設每秒執行 50 次，可以改變設定）。
    // - 不受幀率影響，執行頻率是固定的。
    // - 適合處理物理運算（比如：剛體的移動、力的施加、碰撞檢測）。
    //   比如：讓球在地板上彈跳，或模擬重力效果。
    // - 特點：執行間隔固定，確保物理模擬的穩定性。

    // 什麼時候用 Update？
    // - 處理與玩家操作或畫面效果相關的邏輯。
    //   比如：偵測按鍵是否被按下，或更新角色動畫。

    // 什麼時候用 FixedUpdate？
    // - 處理與物理相關的邏輯，比如剛體運動或碰撞檢測。
    //   如果要讓角色用剛體移動，建議把移動邏輯放在 FixedUpdate 裡。

    // 小結：
    // - Update 是用來做一般邏輯的，每幀執行一次。
    // - FixedUpdate 是專門為物理運算設計的，每隔固定時間執行一次。
    // - 把玩家輸入和動畫控制放在 Update，把與剛體相關的邏輯放在 FixedUpdate。


    // Update is called once per frame
    void Update()
    {
        // 設置移動方向
        direction = Vector3.zero;

        // 使用 Physics2D 檢測角色是否在地面上
        LayerMask layerMask = LayerMask.GetMask("Ground"); // 確保"Ground"是正確的層
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.05f, layerMask);



        // 判斷按鍵輸入並設置方向
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            roleani.SetInteger("Status", 1);
            direction = Vector3.left; // 向左移動
            if (SpriteRenderer.flipX == true)
            {
                SpriteRenderer.flipX = false;
            }
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            roleani.SetInteger("Status", 1);
            direction = Vector3.right; // 向右移動
            if (SpriteRenderer.flipX == false)
            {
                SpriteRenderer.flipX = true;
            }
        }
        else
        {
            roleani.SetInteger("Status", 0);
        }

        // 判斷是否在衝刺
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) 
        { 
            isSprinting = true; 
            roleani.SetBool("isRunning", true); 
        } 
        else 
        { 
            isSprinting = false; 
            roleani.SetBool("isRunning", false);
        }

        // 如果角色在地面上，才能跳躍
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            roleani.SetBool("isJumping", true);
        }

        // 根據是否在地面上更新isJumping參數
        roleani.SetBool("isJumping", !isGrounded);
    }

    // FixedUpdate 用來處理物理運算
    void FixedUpdate()
    {
        float currentSpeed = isSprinting ? sprintSpeed : speed;
        rb.velocity = new Vector2(direction.x * currentSpeed, rb.velocity.y);

    }

    // 在 Scene 視圖中顯示地面檢查範圍
    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;            // 設定顏色為綠色
            Gizmos.DrawWireSphere(groundCheck.position, 0.05f);  // 繪製一個圓圈來顯示檢查範圍
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            Debug.Log("Character landed on the ground.");
            roleani.SetBool("isJumping", false);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            Debug.Log("Character left the ground.");
        }
    }
}

