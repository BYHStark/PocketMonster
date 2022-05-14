using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    // 一、 数学位移
    //          1. 向量 + 向量
    //          2. Translate
    //          3. MoveTowards
    // 二、角色控制器
    //          1. Move
    //          2. SimpleMove
    // 三、刚体
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

    private NavMeshAgent m_agent;
    private bool m_isAutoMove;
    private Vector3 m_v3TargetPos;

    // Start is called before the first frame update
    void Start()
    {
        // Anim/PM_Monster_Combatpikachu_Mega2
        m_anim = transform.Find("Anim").GetComponent<Animator>();

        m_agent = transform.GetComponent<NavMeshAgent>();

        if ( m_emMoveType == emMoveType.CharacterController )
        {
            m_characterController = transform.gameObject.GetComponent<CharacterController>();
            if(m_characterController == null )
                m_characterController = transform.gameObject.AddComponent<CharacterController>();

            m_characterController.center = new Vector3(0f, 0.83f, 0f);
            m_characterController.height = 1.6f;
        }
        else if( m_emMoveType == emMoveType.Rigidbody )
        {
            m_rigidbody = transform.gameObject.GetComponent<Rigidbody>();
            if( m_rigidbody == null )
                m_rigidbody = transform.gameObject.AddComponent<Rigidbody>();
            m_rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | 
                                                       RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            m_rigidbody.drag = 5f;

            CapsuleCollider collider = transform.gameObject.AddComponent<CapsuleCollider>();
            collider.center = new Vector3(0f, 0.77f, 0f);
            collider.height = 1.66f;
        }
        else if( m_emMoveType == emMoveType.Mathf )
        {
            CapsuleCollider collider = transform.gameObject.AddComponent<CapsuleCollider>();
            collider.center = new Vector3(0f, 0.77f, 0f);
            collider.height = 1.66f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 虚拟轴的取值范围： -1 - 1

        float h = Input.GetAxis( "Horizontal" );
        float v = Input.GetAxis( "Vertical" );


        if( h != 0 || v != 0 || ( m_isAutoMove && Vector3.Distance( transform.position, m_v3TargetPos ) <= 3f ) )
        {
            m_isAutoMove = false;
            m_agent.isStopped = true;
            m_v3TargetPos = Vector3.zero;
        }

        //if ( h == 0 && v == 0 )
        //    return;

        if( h != 0 || v != 0 )
        {
            Vector3 dir = new Vector3(-h, 0, -v);
            Quaternion targetQ = Quaternion.LookRotation(dir, Vector3.up);
            //transform.rotation = targetQ;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetQ, Time.deltaTime * mfRotateSpeed);

            switch (m_emMoveType)
            {
                case emMoveType.Mathf: // 第一种  普通的位移方式
                    if (m_emMathfSubType == emMathfSubType.Mathf)
                    {
                        transform.position = transform.position + transform.forward * Time.deltaTime * mfMoveSpeed; // 按帧移动
                    }
                    else if (m_emMathfSubType == emMathfSubType.Translate)
                    {
                        transform.Translate(transform.forward * Time.deltaTime * mfMoveSpeed, Space.World); // 按帧移动
                    }
                    else if (m_emMathfSubType == emMathfSubType.MoveTowards)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward * Time.deltaTime * mfMoveSpeed, Time.deltaTime * mfMoveSpeed); // 按帧移动
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
                        m_characterController.Move(-transform.up * Time.deltaTime * mfMoveSpeed); // 按帧移动
                        m_characterController.Move(transform.forward * Time.deltaTime * mfMoveSpeed); // 按帧移动
                    }
                    else if (m_emCharacterControllerSubType == emCharacterControllerSubType.Simple)
                    {
                        m_characterController.SimpleMove(transform.forward * mfMoveSpeed); // 按秒移动
                    }
                    break;
                case emMoveType.Rigidbody:

                    // 在刚体中：
                    //                若使用 velocity 和 AddForce， 控制刚体， 此时
                    //                                      现象：在不考虑摩擦力的情况之下，
                    //                                                                                           velocity           始终恒速
                    //                                                                                           AddForce       会产生越来越快的速度

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
                        m_rigidbody.MovePosition(transform.position + transform.forward * Time.deltaTime * mfMoveSpeed); // 按帧移动
                    }
                    break;
                default:
                    break;
            }
        }

        m_anim.SetBool("bMove", h != 0 || v != 0 || m_isAutoMove);
    }

    public void AutoPath( Vector3 pos )
    {
        Debug.Log( "启动自动寻路。。。" );

        m_isAutoMove = true;
        m_v3TargetPos = pos;

        m_agent.isStopped = false;
        m_agent.destination = pos;
        m_agent.speed = mfMoveSpeed;
    }
}
