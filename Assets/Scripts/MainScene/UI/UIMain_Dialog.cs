using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain_Dialog : MonoBehaviour
{
    private Text m_txtName;
    private Text m_txtDetails;

    private Button m_btnNext;
    private Button m_btnClose;

    private Dictionary<emTaskState, List<string>> m_dicContents;

    private TaskDefine m_npcTaskDefine;
    private int m_index;

    private Player m_player;
    private Npc m_npc;

    private void Awake()
    {
        m_txtName = transform.Find("Dialog/Name/Text").GetComponent<Text>();
        m_txtDetails = transform.Find("Dialog/Text").GetComponent<Text>();

        m_btnNext = transform.Find("Dialog/Name (1)/Button").GetComponent<Button>();
        m_btnClose = transform.Find("ButtonClose").GetComponent<Button>();

        m_btnNext.onClick.AddListener(OnClickNext);
        m_btnClose.onClick.AddListener(OnClickClose);

        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show( Npc npc )
    {
        m_dicContents = DialogManager.Instance.GetDialog( npc.name );
        m_npcTaskDefine = TaskManager.Instance.GetTaskDefineByNpc( npc );
        m_npc = npc;

        if ( m_dicContents != null )
        {
            OnClickNext();
            this.gameObject.SetActive( true );
        }
    }

    public void Close()
    {
        m_index = 0;
        this.gameObject.SetActive( false );
    }

    private void OnClickNext()
    {
        List<string> list = null;

        if( m_npcTaskDefine != null )
        {
            // 如果存在任务的定义，则该NPC要么为 斗笠菇，要么为小火龙；此时 根据当前NPC身上的任务状态 提取对话列表就可以了
            list = m_dicContents[m_npcTaskDefine.mTaskState];
        }
        else
        {
            // 如果当前NPC身上不存在任务，则可以推测出当前NPC为 妙蛙老师
            Npc targetNpc = GameObject.Find("小火龙").GetComponent<Npc>();
            TaskDefine targetDefine = TaskManager.Instance.GetTaskDefineByNpc( targetNpc );
            list = m_dicContents[targetDefine.mTaskState];
        }

        if( m_index >= list.Count )
        {
            if( m_npc.gameObject.name == "妙蛙老师" )
            {
                Npc targetNpc = GameObject.Find("小火龙").GetComponent<Npc>();
                TaskDefine targetDefine = TaskManager.Instance.GetTaskDefineByNpc(targetNpc);

                if( targetDefine.mTaskState == emTaskState.Incomplete )
                {
                    m_player.AutoPath( GameObject.Find( targetDefine.mStrTargetNpcName ).transform.position );
                }
            }
            else if( m_npcTaskDefine.mNpc.name == "斗笠菇" )
            {
                if( m_npcTaskDefine.mTaskState == emTaskState.Available )
                {
                    // m_npcTaskDefine.mNpc;
                    UIMain.Instance.ShowTask( m_npc );
                }
                else if(  m_npcTaskDefine.mTaskState == emTaskState.Complete )
                {
                    m_player.AutoPath( GameObject.Find( m_npcTaskDefine.mStrTargetNpcName ).transform.position );
                }
            }
            else if ( m_npcTaskDefine.mNpc.name == "小火龙" )
            {
                // 这个NPC是斗笠菇
                Npc breloom = GameObject.Find(m_npcTaskDefine.mStrTargetNpcName).GetComponent<Npc>();
                TaskDefine breloomTaskDefine = TaskManager.Instance.GetTaskDefineByNpc(breloom);

                if( breloomTaskDefine.mTaskState == emTaskState.Available )
                {
                    m_player.AutoPath( GameObject.Find( breloom.gameObject.name ).transform.position );
                }
                else if( breloomTaskDefine.mTaskState == emTaskState.Complete || breloomTaskDefine.mTaskState == emTaskState.UnAvailable )
                {
                    UIMain.Instance.ShowTask( m_npc );
                }
            }

            Close();
            return;
        }
       
        string str = list[m_index++];
        string[] array = str.Split('|');
        m_txtName.text = array[0];
        m_txtDetails.text = array[1];
    }

    private void OnClickClose()
    {
        Close();
    }
}
