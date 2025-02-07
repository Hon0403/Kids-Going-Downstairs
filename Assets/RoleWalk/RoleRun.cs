using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoleRun : MonoBehaviour
{
    //Vector3 �O Unity �����@�ӵ��c�A�ΨӪ�ܤT���Ŷ������V�q�M�I�C
    //���]�t�T�ӯB�I�ƭȡA�N��Ŷ����� x�By �M z �T�Ӷb�CVector3 �D�`�`���A�Ω��m�B��V�B�Y�񵥦h�ر��p�C

    //Vector3 �P transform.Translate �����K�������C
    //transform.Translate ��k�O�ΨӲ��ʪ��骺��m�A�� Vector3 �Ω��ܳo�Ӳ��ʪ���V�M�Z���C

    public float speed = 0.1f; // ����Ⲿ�ʪ��t��
    public float sprintSpeed = 0.2f; // �Ĩ�t�סA�i�H�b�������վ�

    private Vector3 direction; // �x�s���ʤ�V
    private Rigidbody2D rb; // ���⪺Rigidbody2D�ե�
    public Animator roleani;
    public SpriteRenderer SpriteRenderer;

    public float jumpForce = 10f;  // ���D�O�q
    public Transform groundCheck;  // �ˬd�a��������]�i�H�O���⪺�}�^
    public LayerMask LayerMask;  // �]�w���a���h

    private bool isGrounded;  // �O�_�b�a���W
    private bool isSprinting; // �O�_�b�Ĩ�


    // Start is called before the first frame update
    void Start()
    {
        // ���Rigidbody2D�ե�
        rb = GetComponent<Rigidbody2D>();
    }

    // FixedUpdate �M Update �O Unity ���Ψӱ���C���޿��s����ؤ�k�C
    // ���̪��D�n�t�O�b��u�����W�v�v�M�u�A�X���γ~�v�C

    // Update ��k�G
    // - �C�@�V�]�e����s�@���^����@���C
    // - �V�v�V���A���榸�ƶV�h�A�V�v�V�C�A���榸�ƶV�֡C
    // - �A�X�B�z���a��J�B�ʵeĲ�o�Τ@��C���޿�C
    //   ��p�G��������O�_�Q���U�A��������ھڿ�J���ʡC
    // - ���I�G�]�������W�v��í�w�A�B�z���z�B��ɥi��|���ǽT�C

    // FixedUpdate ��k�G
    // - �C�j�T�w�ɶ�����@���]�w�]�C����� 50 ���A�i�H���ܳ]�w�^�C
    // - �����V�v�v�T�A�����W�v�O�T�w���C
    // - �A�X�B�z���z�B��]��p�G���骺���ʡB�O���I�[�B�I���˴��^�C
    //   ��p�G���y�b�a�O�W�u���A�μ������O�ĪG�C
    // - �S�I�G���涡�j�T�w�A�T�O���z������í�w�ʡC

    // ����ɭԥ� Update�H
    // - �B�z�P���a�ާ@�εe���ĪG�������޿�C
    //   ��p�G��������O�_�Q���U�A�Χ�s����ʵe�C

    // ����ɭԥ� FixedUpdate�H
    // - �B�z�P���z�������޿�A��p����B�ʩθI���˴��C
    //   �p�G�n������έ��鲾�ʡA��ĳ�Ⲿ���޿��b FixedUpdate �̡C

    // �p���G
    // - Update �O�ΨӰ��@���޿誺�A�C�V����@���C
    // - FixedUpdate �O�M�������z�B��]�p���A�C�j�T�w�ɶ�����@���C
    // - �⪱�a��J�M�ʵe�����b Update�A��P����������޿��b FixedUpdate�C


    // Update is called once per frame
    void Update()
    {
        // �]�m���ʤ�V
        direction = Vector3.zero;

        // �ϥ� Physics2D �˴�����O�_�b�a���W
        LayerMask layerMask = LayerMask.GetMask("Ground"); // �T�O"Ground"�O���T���h
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.05f, layerMask);



        // �P�_�����J�ó]�m��V
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            roleani.SetInteger("Status", 1);
            direction = Vector3.left; // �V������
            if (SpriteRenderer.flipX == true)
            {
                SpriteRenderer.flipX = false;
            }
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            roleani.SetInteger("Status", 1);
            direction = Vector3.right; // �V�k����
            if (SpriteRenderer.flipX == false)
            {
                SpriteRenderer.flipX = true;
            }
        }
        else
        {
            roleani.SetInteger("Status", 0);
        }

        // �P�_�O�_�b�Ĩ�
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

        // �p�G����b�a���W�A�~����D
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            roleani.SetBool("isJumping", true);
        }

        // �ھڬO�_�b�a���W��sisJumping�Ѽ�
        roleani.SetBool("isJumping", !isGrounded);
    }

    // FixedUpdate �ΨӳB�z���z�B��
    void FixedUpdate()
    {
        float currentSpeed = isSprinting ? sprintSpeed : speed;
        rb.velocity = new Vector2(direction.x * currentSpeed, rb.velocity.y);

    }

    // �b Scene ���Ϥ���ܦa���ˬd�d��
    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;            // �]�w�C�⬰���
            Gizmos.DrawWireSphere(groundCheck.position, 0.05f);  // ø�s�@�Ӷ�������ˬd�d��
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

