using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // һ�� ��ѧλ��
    //          1. ���� + ����
    //          2. Translate
    //          3. MoveTowards
    // ������ɫ������
    //          1. Move
    //          2. SimpleMove
    // ��������
    //          1. vcelocity
    //          2. AddForce
    //          3. MovePosition

    public enum emMoveType
    {
        Mathf,
        CharacterController,
        Rigidbody,
    }

    public enum emMathfSubType
    {
        Mathf,
        Translate,
        MoveTowards,
    }

    public enum emCharacterControllerSubType
    {
        Move,
        Simple,
    }

    public enum emRigidbodySubType
    {
        Velocity,
        AddForce,
        MovePosition,
    }

    [SerializeField]
    private emMoveType m_emMoveType = emMoveType.Mathf;
    //[HideInInspector]
    public emMathfSubType m_emMathfSubType = emMathfSubType.Mathf;
    [SerializeField]
    private emCharacterControllerSubType m_emCharacterControllerSubType = emCharacterControllerSubType.Move;
    [SerializeField]
    private emRigidbodySubType m_emRigidbodySubType = emRigidbodySubType.Velocity;

    public float mfRotateSpeed;
    public float mfMoveSpeed;
    public float mAddForceFactor;

    private CharacterController m_characterController;
    private Rigidbody m_rigidbody;

    private Animator m_anim;

    // Start is called before the first frame update
    void Start()
    {
        // Anim/PM_Monster_Combatpikachu_Mega2
        m_anim = transform.Find("Anim").GetComponent<Animator>();

        if (m_emMoveType == emMoveType.CharacterController)
        {
            m_characterController = transform.gameObject.GetComponent<CharacterController>();
            if (m_characterController == null)
                m_characterController = transform.gameObject.AddComponent<CharacterController>();

            m_characterController.center = new Vector3(0f, 0.83f, 0f);
            m_characterController.height = 1.6f;
        }
        else if (m_emMoveType == emMoveType.Rigidbody)
        {
            m_rigidbody = transform.gameObject.GetComponent<Rigidbody>();
            if (m_rigidbody == null)
                m_rigidbody = transform.gameObject.AddComponent<Rigidbody>();
            m_rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX |
                                                       RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

            CapsuleCollider collider = transform.gameObject.AddComponent<CapsuleCollider>();
            collider.center = new Vector3(0f, 0.77f, 0f);
            collider.height = 1.66f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // �������ȡֵ��Χ�� -1 ~ 1

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        m_anim.SetBool("bMove", h != 0 || v != 0);

        if (h == 0 && v == 0)
            return;

        Vector3 dir = new Vector3(-h, 0, -v);
        Quaternion targetQ = Quaternion.LookRotation(dir, Vector3.up);
        //transform.rotation = targetQ;
        transform.rotation = Quaternion.Lerp(transform.rotation, targetQ, Time.deltaTime * mfRotateSpeed);

        switch (m_emMoveType)
        {
            case emMoveType.Mathf: // ��һ��  ��ͨ��λ�Ʒ�ʽ
                if (m_emMathfSubType == emMathfSubType.Mathf)
                {
                    transform.position = transform.position + transform.forward * Time.deltaTime * mfMoveSpeed; // ��֡�ƶ�
                }
                else if (m_emMathfSubType == emMathfSubType.Translate)
                {
                    transform.Translate(transform.forward * Time.deltaTime * mfMoveSpeed, Space.World); // ��֡�ƶ�
                }
                else if (m_emMathfSubType == emMathfSubType.MoveTowards)
                {
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward * Time.deltaTime * mfMoveSpeed, Time.deltaTime * mfMoveSpeed);
                }
                break;
            case emMoveType.CharacterController:

                // 1. ��ɫ��������ͨλ�Ƶ�����
                //     ��ɫ������  ���赲
                //     λ��            ���赲
                // 2. ��ɫ������ Move��SimpleMove ����
                //      Move                û��ģ�⡰��������Ч��
                //      SimpleMove     ����ģ�⡰��������Ч��

                if (m_emCharacterControllerSubType == emCharacterControllerSubType.Move)
                {
                    m_characterController.Move(-transform.up * Time.deltaTime * mfMoveSpeed); // ��֡�ƶ���
                    m_characterController.Move(transform.forward * Time.deltaTime * mfMoveSpeed); // ��֡�ƶ���
                }
                else if (m_emCharacterControllerSubType == emCharacterControllerSubType.Simple)
                {
                    m_characterController.SimpleMove(transform.forward * mfMoveSpeed); // �����ƶ���
                }
                break;
            case emMoveType.Rigidbody:

                // �ڸ����У�
                //                ��ʹ�� velocity �� AddForce�� ���Ƹ��壬 ��ʱ
                //velocity       ʼ�պ���
                //AddForce       ���ٶ�

                if (m_emRigidbodySubType == emRigidbodySubType.Velocity)
                {
                    m_rigidbody.velocity = transform.forward * mfMoveSpeed; // �����ƶ��� ����
                }
                else if (m_emRigidbodySubType == emRigidbodySubType.AddForce)
                {
                    m_rigidbody.AddForce(transform.forward * mfMoveSpeed * mAddForceFactor); // �����ƶ������ٶ�
                }
                else if (m_emRigidbodySubType == emRigidbodySubType.MovePosition)
                {
                    m_rigidbody.MovePosition(transform.position + transform.forward * Time.deltaTime * mfMoveSpeed);
                }
                break;
            default:
                break;
        }
    }
}
