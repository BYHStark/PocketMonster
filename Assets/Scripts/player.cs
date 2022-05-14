using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{

    public enum emMoveType
    {
        Mathf,
        CharacterController,
        Rigidbody,
    }

    public enum emMathfSubType   
    {
        // 一、 数学位移
        //          1. 向量 + 向量
        //          2. Translate
        //          3. MoveTowards
        Mathf,
        Translate,
        MoveTowards,
    }

    public enum emCharacterControllerSubType
    {
        // 二、角色控制器
        //          1. Move
        //          2. SimpleMove
        Move,
        Simple,
    }

    public enum emRigidbodySubType
    {
        // 三、刚体
        //          1. vcelocity
        //          2. AddForce
        //          3. MovePosition
        Velocity,
        AddForce,
        MovePosition,
    }

    [SerializeField]
    private emMoveType m_emMoveType = emMoveType.Mathf;
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
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        //-1<=h,v<=1
        if (h==0&&v==0)
        {
            return ;
        }
        Vector3 dir =new Vector3(-h,0,-v);//场景的z轴设置方向为反向
        Quaternion targetQ = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetQ, Time.deltaTime * mfRotateSpeed);

        switch (m_emMoveType)
        {
            case emMoveType.Mathf: // 第一种  普通的位移方式
                if (m_emMathfSubType == emMathfSubType.Mathf)
                {
                    transform.position += transform.forward * Time.deltaTime * mfMoveSpeed; // 按帧移动
                }
                else if (m_emMathfSubType == emMathfSubType.Translate)
                {
                    transform.Translate(transform.forward * Time.deltaTime * mfMoveSpeed, Space.World); // 按帧移动
                }
                else if (m_emMathfSubType == emMathfSubType.MoveTowards)//参数：从某个位置，到目标位置，经历时间
                {
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward * Time.deltaTime * mfMoveSpeed, Time.deltaTime * mfMoveSpeed);
                }
                break;

            case emMoveType.CharacterController:
                // 1. 角色控制与普通位移的区别
                //     角色控制器  有阻挡
                //     位移            无阻挡
                // 2. 角色控制器 Move和SimpleMove 区别
                //      Move                没有模拟“重力”的效果
                //      SimpleMove     可以模拟“重力”的效果
                if (m_emCharacterControllerSubType == emCharacterControllerSubType.Move)
                {
                    m_characterController.Move(-transform.up * Time.deltaTime * mfMoveSpeed); // 按帧移动的
                    m_characterController.Move(transform.forward * Time.deltaTime * mfMoveSpeed); // 按帧移动的
                }
                else if (m_emCharacterControllerSubType == emCharacterControllerSubType.Simple)
                {
                    m_characterController.SimpleMove(transform.forward * mfMoveSpeed); // 按秒移动的
                }
                break;

            case emMoveType.Rigidbody:
                // 在刚体中：
                //                若使用 velocity 和 AddForce， 控制刚体， 此时
                //velocity       始终恒速
                //AddForce       加速度
                if (m_emRigidbodySubType == emRigidbodySubType.Velocity)
                {
                    m_rigidbody.velocity = transform.forward * mfMoveSpeed; // 按秒移动， 恒速
                }
                else if (m_emRigidbodySubType == emRigidbodySubType.AddForce)
                {
                    m_rigidbody.AddForce(transform.forward * mfMoveSpeed * mAddForceFactor); // 按秒移动，加速度
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
