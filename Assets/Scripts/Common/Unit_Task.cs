using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit_Task : MonoBehaviour
{
    private GameObject m_objAvailable;
    private GameObject m_objIncomplete;

    private Transform m_tfTarget;
    private Vector3 m_v3Offset;

    // Start is called before the first frame update
    void Start()
    {
        m_objAvailable = transform.Find("Available").gameObject;
        m_objIncomplete = transform.Find("Incomplete").gameObject;
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    public void Init( Transform tfTarget, Vector3 v3Offset )
    {
        m_tfTarget = tfTarget;
        m_v3Offset = v3Offset;
    }

    public void UpdateTaskState( emTaskState state )
    {
        // 我们要根据 任务的状态，刷新 头顶任务状态图标
        // 更新 任务状态图标的位置

        if( state == emTaskState.Available )
        {
            m_objAvailable.SetActive( true );
            m_objIncomplete.SetActive( false );
        }
        else if( state == emTaskState.Incomplete )
        {
            m_objAvailable.SetActive( false );
            m_objIncomplete.SetActive( true );
        }
        else if( state == emTaskState.Complete || state == emTaskState.UnAvailable )
        {
            m_objAvailable.SetActive( false );
            m_objIncomplete.SetActive( false );
        }

        transform.position = Camera.main.WorldToScreenPoint( this.m_tfTarget.position + m_v3Offset );
    }
}
