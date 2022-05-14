using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum emTriggerType
{
    None,
    Enter,
    Exit,
}

public class Npc : MonoBehaviour
{
    private Transform m_tfPlayer;
    private emTriggerType m_emTriggerType;
    
    private float m_fRotateSpeed;

    private float m_fTimer;

    private Quaternion m_srcRotation;

    // Start is called before the first frame update
    void Start()
    {
        m_tfPlayer = GameObject.FindObjectOfType<Player>().transform;

        m_emTriggerType = emTriggerType.None;
        m_fRotateSpeed = 3f;
        m_fTimer = 0f;
        m_srcRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if( m_emTriggerType == emTriggerType.Enter )
        {
            // 比卡丘Pos - 妙蛙Pos = dir ( 非单位化的向量,  几何意义：得到一个 由妙蛙老师指向比卡丘的一个方向向量 )
            Vector3 dir = m_tfPlayer.transform.position - transform.position;
             Quaternion targteQ = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.Lerp( transform.rotation, targteQ, Time.deltaTime * m_fRotateSpeed );

            m_fTimer += Time.deltaTime;
            if( m_fTimer >= 0.5f )
            {
                m_emTriggerType = emTriggerType.None;
                m_fTimer = 0f;
            }
        }
        else if( m_emTriggerType == emTriggerType.Exit )
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, m_srcRotation, Time.deltaTime * m_fRotateSpeed);
            m_fTimer += Time.deltaTime;
            if (m_fTimer >= 0.5f)
            {
                m_emTriggerType = emTriggerType.None;
                m_fTimer = 0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform != m_tfPlayer)
            return;
        m_emTriggerType = emTriggerType.Enter;
        Debug.Log("OnTriggerEnter：" + other.gameObject.name);

        // 弹出 对话
        UIMain.Instance.ShowDialog( this );
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.transform != m_tfPlayer)
            return;
        m_emTriggerType = emTriggerType.Exit;
        Debug.Log("OnTriggerExit：" + other.gameObject.name);

        // 关闭 对话
        UIMain.Instance.CloseDialog();
    }
}
