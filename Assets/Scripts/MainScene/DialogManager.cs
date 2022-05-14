using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance;

    private Dictionary<string, Dictionary<emTaskState, List<string>>> m_dicDialogs;

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
        
    }

    public Dictionary<emTaskState, List<string>> GetDialog( string strNpcName )
    {
        Dictionary<emTaskState, List<string>> dic;
        if ( false == m_dicDialogs.TryGetValue(strNpcName, out dic) )
        {
            Debug.LogErrorFormat( "没有从 对话管理器中 找到{0}的对话数据", strNpcName);
        }

        return dic;
    }

    private void InitData()
    {
        m_dicDialogs = new Dictionary<string, Dictionary<emTaskState, List<string>>>();

        // 小火龙的对话数据存储
        Dictionary<emTaskState, List<string>> list1 = new Dictionary<emTaskState, List<string>>();
        list1.Add(emTaskState.Available, new List<string>()
        {
            "小火龙|呜呜呜...",
            "皮卡丘|怎么哭了，谁欺负你啦",
            "小火龙|娘亲被 火焰鸡 掳走了",
            "皮卡丘|不要哭了，我去救你娘亲，可...是... 去哪救呢",
            "小火龙|这附近有个 '时空门'，娘亲就是在这里被 火焰鸡 掳走的",
            "皮卡丘|时空门？皮卡丘充满疑惑地使劲揉了揉眼睛",
            "小火龙|在你旁边，要想让这时空门，显示出来，唯一的办法需要先去 斗笠菇 那里取得时空门的钥匙",
        });

        list1.Add(emTaskState.Incomplete, new List<string>()
        {
            "小火龙|皮卡丘，斗笠菇给你 时空门的钥匙了么？",
        });

        list1.Add(emTaskState.Complete, new List<string>()
        {
            "小火龙|快看，时空之门 出来啦,  oh yeah，娘亲有救咯~",
        });

        m_dicDialogs.Add( "小火龙", list1 );

        //////////////////////////////////////////////////////////////////////////////////////////////////////

        // 斗笠菇的对话数据存储

        Dictionary<emTaskState, List<string>> list2 = new Dictionary<emTaskState, List<string>>();
        list2.Add(emTaskState.UnAvailable, new List<string>()
        {
            "斗笠菇|Hi，比卡丘，你来的正好，小火龙不知道为什么哭了，快去看看它吧",
        });

        list2.Add(emTaskState.Available, new List<string>()
        {
            "斗笠菇|嘘，我知道你的来意，这是时空门的钥匙，快去救它娘亲吧， 最后... 希望你能看到明天的太阳",
        });

        list2.Add(emTaskState.Complete, new List<string>()
        {
            "斗笠菇|咳咳，卡比丘，时空门的钥匙已经给你咯，现在快去找小火龙吧",
        });

        m_dicDialogs.Add( "斗笠菇", list2 );

        //////////////////////////////////////////////////////////////////////////////////////////////////////

        // 妙蛙老师的对话数据存储
        Dictionary<emTaskState, List<string>> list3 = new Dictionary<emTaskState, List<string>>();
        list3.Add(emTaskState.Available, new List<string>()
        {
            "妙蛙老师|Hi，你好，皮卡丘，欢迎来到 VipSkill 世界",
        });

        list3.Add(emTaskState.Incomplete, new List<string>()
        {
            "妙蛙老师|比卡丘，你是要找 斗笠菇 么？ 它在家，赶快去找它吧",
        });

        list3.Add(emTaskState.Complete, new List<string>()
        {
            "妙蛙老师|比卡丘， 听说你要去救小火龙的娘亲了，希望你能活着回来",
        });

        m_dicDialogs.Add( "妙蛙老师", list3 );
    }


}
