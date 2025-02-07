using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int platformStepCount = 0; // ���a��L�����x�ƶq

    // �s�W UI Text ����ܨB��
    public TextMeshProUGUI stepCounterText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // �q�\���x�ͦ��ƥ�
            PlatformGenerator.OnPlatformGenerated += HandlePlatformGenerated;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ���x�ͦ��ɳB�z�]�i�H�Ψӧ�s��L���d�޿�^
    void HandlePlatformGenerated(GameObject platform)
    {
        Debug.Log("�ͦ��F�@�ӷs�����x�G" + platform.name);
    }

    // ���a��쥭�x�ɩI�s����k
    public void IncrementPlatformStep()
    {
        platformStepCount++;
        Debug.Log("���a�w�򪺥��x�ơG" + platformStepCount);
        UpdateStepCounterUI();
    }

    // ��s UI Text �W���B�����
    private void UpdateStepCounterUI()
    {
        if (stepCounterText != null)
        {
            stepCounterText.text = "�a�U�Ӽh:" + platformStepCount.ToString();
        }
    }

    void OnDestroy()
    {
        // �O�o�����q�\�ƥ�A�קK�O���鬪�|
        PlatformGenerator.OnPlatformGenerated -= HandlePlatformGenerated;
    }
}
