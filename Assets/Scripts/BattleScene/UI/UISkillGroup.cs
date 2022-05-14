using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillGroup : MonoBehaviour
{
    public GameObject mPanel;
    public Text mTxtTurnTips;
    public Text mTxtOptTips;

    public static UISkillGroup Instance;

    private Entity m_player;

    private Entity m_target;

    private void Awake()
    {
        Instance = this;
        mPanel.SetActive( false );

        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
    }

    // Update is called once per frame
    void Update()
    {
        mTxtTurnTips.text = BattleManager.Instance.m_emBattleState == BattleState.PLAYER_TURN ? "己方回合" : "敌方回合";

        if ( Input.GetMouseButtonDown( 0 ) )
        {
            if( BattleManager.Instance.m_emBattleState == BattleState.PLAYER_TURN && 
                m_player.mIsDead == false && m_player.mIsAlreadyAttack == false )
            {
                Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
                RaycastHit hit;
                if( Physics.Raycast( ray, out hit ) )
                {
                    if( hit.collider.tag == "Enemy1" || hit.collider.tag == "Enemy2" || hit.collider.tag == "Enemy3")
                    {
                        Entity enmy = hit.collider.GetComponent<Entity>();
                        if( enmy.mIsDead == false )
                        {
                            m_target = enmy;
                            Debug.Log( "当前选中的敌人是：" + hit.collider.gameObject.name );
                        }
                    }
                }

            }
        }
    }

    public void Show()
    {
        mPanel.SetActive( true );
    }

    public void OnClickSkill01()
    {
        if( CanUseSkill() )  
        {
            Debug.Log("发起 技能1");
            m_player.CastSkill(m_target, SkillType.Skill01);
            BattleManager.Instance.SetCasterAndByCaster(m_player, m_target);
            m_target = null;
        }
        else
        {
            StartCoroutine( ShowTips() );
        }
    }

    public void OnClickSkill02()
    {
        if (CanUseSkill())
        {
            Debug.Log("发起 技能2");
            m_player.CastSkill(m_target, SkillType.Skill02);
            BattleManager.Instance.SetCasterAndByCaster(m_player, m_target);
            m_target = null;
        }
        else
        {
            StartCoroutine(ShowTips());
        }
    }

    public void OnClickSkill03()
    {
        if ( CanUseSkill() )
        {
            Debug.Log("发起 技能3");
            m_player.CastSkill(m_target, SkillType.Skill02);
            BattleManager.Instance.SetCasterAndByCaster(m_player, m_target);
            m_target = null;
        }
        else
        {
            StartCoroutine(ShowTips());
        }
    }

    private bool CanUseSkill()
    {
        if( BattleManager.Instance.m_emBattleState == BattleState.ENEMY_TURN )
        {
            mTxtOptTips.text = "当前回合是敌方";
            return false;
        }
        else
        {
            if( m_player.mIsDead )
            {
                mTxtOptTips.text = "你已阵亡";
                return false;
            }
            else if( m_player.mIsAlreadyAttack )
            {
                mTxtOptTips.text = "你已发起过攻击";
                return false;
            }
            else if( m_target == null )
            {
                mTxtOptTips.text = "你需要一个目标";
                return false;
            }
        }
        return true;
    }

    public void ChoseTips()
    {
        mTxtOptTips.text = "你需要一个目标";
        StartCoroutine(ShowTips());
    }

    private IEnumerator ShowTips()
    {
        mTxtOptTips.gameObject.SetActive( true );
        yield return new WaitForSeconds(1.5f);
        mTxtOptTips.gameObject.SetActive( false );
    }
}
