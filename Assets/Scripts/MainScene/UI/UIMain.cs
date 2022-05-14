using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMain : MonoBehaviour
{
    public GameObject m_objDialog;
    public GameObject m_objTask;

    private UIMain_Dialog m_dialog;
    private UIMain_Task m_task;

    public static UIMain Instance;

    private void Awake()
    {
        Instance = this;
        m_dialog = m_objDialog.AddComponent<UIMain_Dialog>();
        m_task = m_objTask.AddComponent<UIMain_Task>();

        m_dialog.gameObject.SetActive( false );
        m_task.gameObject.SetActive( false );
    }

    public void  ShowDialog( Npc npc )
    {
        m_dialog.Show( npc );
    }

    public void CloseDialog()
    {
        m_dialog.Close();
    }

    public void ShowTask( Npc npc )
    {
        m_task.Show( npc );
    }

    public void CloseTask()
    {
        m_task.Close();
    }
}
