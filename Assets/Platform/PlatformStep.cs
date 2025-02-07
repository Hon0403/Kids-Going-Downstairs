using UnityEngine;

public class PlatformStep : MonoBehaviour
{
    private bool hasStepped = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasStepped && collision.gameObject.CompareTag("Player"))
        {
            hasStepped = true;
            Debug.Log("���a���F���x�G" + gameObject.name);

            if (GameManager.Instance != null)
            {
                GameManager.Instance.IncrementPlatformStep();
            }
            else
            {
                Debug.LogError("GameManager.Instance �� null�I");
            }
        }
    }
}
