using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIBattleResult : MonoBehaviour
{
    public static UIBattleResult Instance;

    public GameObject mObjPanel;
    public GameObject mObjTextWin;
    public GameObject mObjTextLose;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        mObjPanel.SetActive( false );
    }

    public void Show( BattleState result )
    {
        if( result == BattleState.WIN )
        {
            mObjTextWin.SetActive( true );
            mObjTextLose.SetActive( false );
        }
        else if( result == BattleState.LOSE )
        {
            mObjTextWin.SetActive( false );
            mObjTextLose.SetActive( true );
        }

        mObjPanel.SetActive( true );
    }
}
