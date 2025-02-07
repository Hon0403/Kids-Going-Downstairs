using UnityEngine;

public class PlatformStep : MonoBehaviour
{
    private bool hasStepped = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasStepped && collision.gameObject.CompareTag("Player"))
        {
            hasStepped = true;
            Debug.Log("玩家踩到了平台：" + gameObject.name);

            if (GameManager.Instance != null)
            {
                GameManager.Instance.IncrementPlatformStep();
            }
            else
            {
                Debug.LogError("GameManager.Instance 為 null！");
            }
        }
    }
}
