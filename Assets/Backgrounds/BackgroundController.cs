using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageManager : MonoBehaviour
{
    // �I�����󪺹w�m��C��
    public List<GameObject> backgroundPrefabs;

    // �I���u�ʳt��
    public float scrollSpeed = 2f;

    // ��e��ܪ��I��
    private GameObject currentBackground;
    // �U�@�ӭI��
    private GameObject nextBackground;
    // �I�������ס]�Ω�u�ʧP�_�^
    private float backgroundHeight;

    // �b�C���}�l�ɰ���
    void Start()
    {
        // �p�G�I���C�����w�m��A��l�ƭI��
        if (backgroundPrefabs.Count > 0)
        {
            InitializeBackgrounds();
        }
    }

    // �C�@�V��s
    void Update()
    {
        // ���I���u��
        ScrollBackground();
    }

    // ��l�ƭI��
    private void InitializeBackgrounds()
    {
        // �]�w��e�I��
        SetRandomBackground();
        // �ЫؤU�@�ӭI��
        CreateNextBackground();
    }

    // �H���]�m��e�I��
    private void SetRandomBackground()
    {
        if (backgroundPrefabs.Count > 0)
        {
            // �R����e�I��
            DestroyCurrentBackground();

            // �H����ܤ@�ӭI���w�m��
            int randomIndex = Random.Range(0, backgroundPrefabs.Count);

            // ��ҤƭI���ó]�m�������������I��m�]�ΧA�Ʊ檺�_�l��m�^
            Vector3 startPosition = Vector3.zero;
            currentBackground = Instantiate(backgroundPrefabs[randomIndex], startPosition, Quaternion.identity);

            // �]�m�I������
            SetBackgroundHeight(currentBackground);
            // �ЫؤU�@�ӭI��
            CreateNextBackground();
        }
    }

    // �ھگ��޳]�m��e�I��
    public void SetBackground(int index)
    {
        if (index >= 0 && index < backgroundPrefabs.Count)
        {
            // �R����e�I��
            DestroyCurrentBackground();

            // �ھگ��޹�ҤƭI��
            Vector3 startPosition = Vector3.zero;
            currentBackground = Instantiate(backgroundPrefabs[index], startPosition, Quaternion.identity);

            // �]�m�I������
            SetBackgroundHeight(currentBackground);
            // �ЫؤU�@�ӭI��
            CreateNextBackground();
        }
    }

    // �I���u��
    private void ScrollBackground()
    {
        if (currentBackground != null && nextBackground != null)
        {
            // ����e�I���M�U�@�ӭI���V�W����
            Vector3 movement = Vector3.up * scrollSpeed * Time.deltaTime;
            currentBackground.transform.Translate(movement);
            nextBackground.transform.Translate(movement);

            // �p�G��e�I���������X�B�n��ܰϰ�W��A�R���ä�����U�@�ӭI��
            // ���]�B�n���W��ɦb�������Y�өT�w��m�]�Ҧp y = someValue�^�A�i�H�i��²�檺���
            float offscreenPosition = 10f; // �A�i�H�ھڹ�ڱ��p�վ�o�ӭ�
            if (currentBackground.transform.position.y - backgroundHeight / 2 > offscreenPosition)
            {
                Destroy(currentBackground); // �R����e�I��
                currentBackground = nextBackground; // �N�U�@�ӭI���]����e�I��
                CreateNextBackground();             // �Ыؤ@�ӷs���U�@�ӭI��
            }
        }
    }

    // �ЫؤU�@�ӭI��
    private void CreateNextBackground()
    {
        if (backgroundPrefabs.Count > 0)
        {
            // �H����ܤ@�ӭI���w�m��
            int randomIndex = Random.Range(0, backgroundPrefabs.Count);

            // �p��s�I������m�A�Ϩ�P��e�I���α�
            Vector3 nextPosition = currentBackground.transform.position - new Vector3(0, backgroundHeight, 0);

            // ��ҤƤU�@�ӭI��
            nextBackground = Instantiate(backgroundPrefabs[randomIndex], nextPosition, Quaternion.identity);

            // �]�m�I�����ס]�p�G�I�����ץi�ण�P�^
            SetBackgroundHeight(nextBackground);
        }
    }

    // �R����e�I��
    private void DestroyCurrentBackground()
    {
        if (currentBackground != null)
        {
            Destroy(currentBackground); // �R����e�I������
        }
    }

    // �]�m�I�����סA�Ω�u�ʧP�_
    private void SetBackgroundHeight(GameObject background)
    {
        // ������� Renderer �ե�
        Renderer renderer = background.GetComponent<Renderer>();
        if (renderer != null)
        {
            backgroundHeight = renderer.bounds.size.y;
        }
        else
        {
            // �p�G�S�� Renderer�A�]�m�@���q�{����
            backgroundHeight = 10f; // �ھڧA����ڭI�����׳]�w
        }
    }
}
