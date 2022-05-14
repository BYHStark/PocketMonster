using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum emTaskState
{
    /// <summary>
    /// 不可接
    /// </summary>
    UnAvailable,
    /// <summary>
    /// 可接
    /// </summary>
    Available,
    /// <summary>
    /// 已接取，未完成
    /// </summary>
    Incomplete,
    /// <summary>
    /// 完成
    /// </summary>
    Complete,
}

public class TaskDefine
{
    public Npc mNpc;

    public int mTaskID;
    public emTaskState mTaskState;

    // NPC头顶上的任务图标的状态
    public Unit_Task mUnitTask;

    public string mStrTitle;
    public string mStrDetail;
    public string mStrTargetNpcName; // 此值 应用于 目标寻路
}

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;

    private Dictionary<Npc, TaskDefine> m_dicTaskData;

    private void Awake()
    {
        Instance = this;
        InitData();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( SceneManager.Instance.IsMainScene )
        {
            foreach ( KeyValuePair<Npc, TaskDefine> keyValue in m_dicTaskData )
            {
                keyValue.Value.mUnitTask.UpdateTaskState( keyValue.Value.mTaskState );
            }
        }
    }

    public TaskDefine GetTaskDefineByNpc( Npc npc )
    {
        TaskDefine taskDefine;
        if (false == m_dicTaskData.TryGetValue(npc, out taskDefine))
        {
            Debug.LogErrorFormat( "没有从 任务管理器中 找到{0}的任务定义", npc.name );
        }
        return taskDefine;
    }

    private void InitData()
    {
        m_dicTaskData = new Dictionary<Npc, TaskDefine>();

        GameObject model = Resources.Load<GameObject>("Prefabs/UI/Unit_Task");
        model.SetActive( false );

        // 这里是“种”小火龙 的 任务
        TaskDefine task1 = new TaskDefine();
        task1.mNpc = GameObject.Find("小火龙").GetComponent<Npc>();
        task1.mTaskID = 1001;
        task1.mTaskState = emTaskState.Available;
        task1.mUnitTask = BindUnitTask( model, task1.mNpc.transform, new Vector3(-0.35f, 3.5f, 0 ) ) ;
        task1.mStrTitle = "拜访 【斗笠菇】";
        task1.mStrDetail = "获取 ‘时空之门’钥匙";
        task1.mStrTargetNpcName = "斗笠菇";
        m_dicTaskData.Add( task1.mNpc, task1 );

        ////////////////////////////////////////////////

        // 这里是“种”斗笠菇 的 任务

        TaskDefine task2 = new TaskDefine();
        task2.mNpc = GameObject.Find("斗笠菇").GetComponent<Npc>();
        task2.mTaskID = 1002;
        task2.mTaskState = emTaskState.UnAvailable;
        task2.mUnitTask = BindUnitTask(model, task2.mNpc.transform, new Vector3(-0.35f, 3.5f, 0));
        task2.mStrTitle = "回访 【小火龙】";
        task2.mStrDetail = "激活 ‘时空之门’";
        task2.mStrTargetNpcName = "小火龙";
        m_dicTaskData.Add( task2.mNpc, task2 );
    }

    private Unit_Task BindUnitTask( GameObject model, Transform tfTarget, Vector3 v3Offset )
    {
        GameObject obj = Instantiate<GameObject>( model );
        obj.SetActive( true );
        obj.transform.parent = GameObject.Find("UnitTaskRoot").transform;
        obj.transform.position = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;
        Unit_Task unitTask = obj.AddComponent<Unit_Task>();
        unitTask.Init( tfTarget, v3Offset );
        return unitTask;
    }
}
