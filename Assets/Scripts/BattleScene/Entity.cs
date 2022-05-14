using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum SkillType
{
    None,
    Skill01,
    Skill02,
    Skill03,
    Max,

    // Random.Range( 1, 3 ) ---> （1,3] ==> 1, 2
}

public enum MoveType
{
    /// <summary>
    /// 发起攻击前，向敌方移动
    /// </summary>
    MOVE,
    /// <summary>
    /// 攻击结束，回到初始的位置
    /// </summary>
    RESET,
}

public class Entity : MonoBehaviour
{
    protected Animator m_anim;
    protected Unit_Entity m_uiEntity;

    private Vector3 m_srcPos;

    [SerializeField]
    private bool mbAutoBattle;

    private float m_fHP;

    public bool mIsDead { get; private set; }

    [HideInInspector]
    public bool mIsAlreadyAttack;


    // Start is called before the first frame update
    void Start()
    {
        m_uiEntity = transform.GetComponent<Unit_Entity>();
        m_anim = transform.Find("Anim").GetComponent<Animator>();

        m_srcPos = transform.position;

        m_fHP = 100f;
        m_uiEntity.SetHP( m_fHP );

        mIsAlreadyAttack = false;
    }

    public void CastSkill( Entity target )
    {
        // 自动攻击
        int indexSkill = Random.Range( (int)SkillType.Skill01, (int)SkillType.Max); // 1,2,3; 不包含 max的值
        CastSkill( target, (SkillType)indexSkill );
    }

    public void CastSkill( Entity target, SkillType emSkillType  )
    {
        // 手选攻击
        mIsAlreadyAttack = true;
        Move( MoveType.MOVE, target.transform.position + ( target.transform.forward * 3 ), emSkillType );
    }

    public void Move( MoveType emMoveType, Vector3 targetPos, SkillType emSkillType )
    {
        if( emMoveType == MoveType.MOVE )
        {
            // 正常攻击的移动
            // 1. 播放移动动画
            m_anim.SetBool( "bMove", true );
            // 2. 位移至目标
            //              释放技能？
            transform.DOMove(targetPos, 1).OnComplete( () => {
                m_anim.SetBool( "bMove", false );
                Attack( emSkillType );
            });

        }
        else if( emMoveType == MoveType.RESET )
        {
            // 攻击完之后，归位(m_srcPos)
            transform.DOMove( targetPos, 1 ).OnComplete(() => {
                m_anim.SetBool( "bMove", false );
                // 切换 攻击对象
                BattleManager.Instance.ChangeCaster();
            });
        }
    }

    private void Attack( SkillType emSkillType )
    {
        switch (emSkillType)
        {
            case SkillType.Skill01:
                m_anim.SetTrigger("triggerAttack1");
                break;
            case SkillType.Skill02:
                m_anim.SetTrigger("triggerAttack2");
                break;
            case SkillType.Skill03:
                m_anim.SetTrigger("triggerSkill");
                break;
            default:
                break;
        }
    }

    public void Damage( float damage )
    {
        m_fHP -= damage;
        if (m_fHP <= 0)
        {
            m_fHP = 0f;
            mIsDead = true;
            m_anim.SetTrigger("triggerDead");
        }
        else
            m_anim.SetTrigger("triggerHurt");
        m_uiEntity.Damage( damage );
    }

    public void ResetMoveEvent()
    {
        Move( MoveType.RESET, m_srcPos, SkillType.None );
    }

}
