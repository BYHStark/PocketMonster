using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState
{
    /// <summary>
    /// 己方回合
    /// </summary>
    PLAYER_TURN,
    /// <summary>
    /// 敌方回合
    /// </summary>
    ENEMY_TURN,
    /// <summary>
    /// 成功
    /// </summary>
    WIN,
    /// <summary>
    /// 失败
    /// </summary>
    LOSE,
}

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    private List<Entity> m_listEnemy;
    private List<Entity> m_listPlayer;

    private Entity m_player;

    /// <summary>
    /// 施法者(攻击方)
    /// </summary>
    private Entity m_caster;
    /// <summary>
    /// 被施法者(受击打)
    /// </summary>
    private Entity m_byCaster;

    public BattleState m_emBattleState { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        BuildEntity();

        m_emBattleState = BattleState.PLAYER_TURN;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void BuildEntity()
    {
        m_listEnemy = new List<Entity>();
        m_listEnemy.Add(GameObject.FindGameObjectWithTag("Enemy1").GetComponent<Entity>());
        m_listEnemy.Add(GameObject.FindGameObjectWithTag("Enemy2").GetComponent<Entity>());
        m_listEnemy.Add(GameObject.FindGameObjectWithTag("Enemy3").GetComponent<Entity>());

        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();

        m_listPlayer = new List<Entity>();
        m_listPlayer.Add(GameObject.FindGameObjectWithTag("Player1").GetComponent<Entity>());
        m_listPlayer.Add( m_player );
        m_listPlayer.Add(GameObject.FindGameObjectWithTag("Player2").GetComponent<Entity>());
    }

    public void ChangeCaster()
    {
        if( m_emBattleState == BattleState.ENEMY_TURN )
        {
            foreach (Entity enemy in m_listEnemy )
            {
                if (enemy.mIsDead)
                    continue;
                if (enemy.mIsAlreadyAttack)
                    continue;

                // 当执行到这里的时候，表明此刻 enemy 有发起攻击的权利了
                Entity target = FindAlive(m_listPlayer);
                if (target == null)
                    continue;

                enemy.CastSkill(target);
                SetCasterAndByCaster(enemy, target);
                return;
            }

            // 重置下状态，为下一轮 切磋 做准备
            ResetEntityAttackState();

            if( FindAlive( m_listPlayer ) == null )
            {
                m_emBattleState = BattleState.LOSE;
                UIBattleResult.Instance.Show(BattleState.LOSE);
            }
            else
            {
                m_emBattleState = BattleState.PLAYER_TURN;
                ChangeCaster();
            }

        }
        else if( m_emBattleState == BattleState.PLAYER_TURN )
        {
            if( !m_player.mIsDead && !m_player.mIsAlreadyAttack )
            {
                // 转向 MainPlayer 处理
                UISkillGroup.Instance.ChoseTips();
                return;
            }
            else
            {
                //  走完foreach，代表着 该阵营内的角色，都已经发起过攻击了
                foreach (Entity player in m_listPlayer )
                {
                    if (player.tag == "Player")
                        continue;
                    if (player.mIsAlreadyAttack || player.mIsDead)
                        continue;
                    Entity target = FindAlive( m_listEnemy ); // 找一个 活着的 被施法者
                    if (null == target)
                        continue;

                    player.CastSkill( target );
                    SetCasterAndByCaster( player, target );
                    return;
                }

                // 重置下状态，为下一轮 切磋 做准备
                ResetEntityAttackState();

                // 当前回合结束后，就面临着  成功/失败的情况了
                if( FindAlive( m_listEnemy ) == null )
                {
                    m_emBattleState = BattleState.WIN;
                    UIBattleResult.Instance.Show( BattleState.WIN );
                }
                else
                {
                    // 转换到 敌方阵营攻击
                    m_emBattleState = BattleState.ENEMY_TURN;
                    ChangeCaster();
                }
            }


        }
    }

    public Entity FindAlive( List<Entity> listEntitys )
    {
        int indexEnemy = Random.Range(0, 3);
        Entity target = listEntitys[indexEnemy];
        if( target.mIsDead )
        {
            target = null;
            foreach ( Entity entity in listEntitys )
            {
                if( !entity.mIsDead )
                {
                    target = entity;
                    break;
                }
            }
        }
        return target;
    }

    public void Damage( float damage )
    {
        // 给受击方(被施法者)，减血
        m_byCaster.Damage( damage );
    }

    public void ResetMoveEvent()
    {
        m_caster.ResetMoveEvent();
    }

    public void SetCasterAndByCaster( Entity caster, Entity byCaster )
    {
        m_caster = caster;
        m_byCaster = byCaster;
    }

    private void ResetEntityAttackState()
    {
        foreach ( Entity item in m_listEnemy )
        {
            item.mIsAlreadyAttack = false;
        }

        foreach (Entity item in m_listPlayer )
        {
            item.mIsAlreadyAttack = false;
        }
    }
}
