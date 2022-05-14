using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;

    public bool IsMainScene
    {
        get
        {
            UnityEngine.SceneManagement.Scene curScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            //if(curScene.name == "MainScene")
            //{
            //    return true;
            //}
            //return false; // 意味着战斗场景

            // return curScene.name == "MainScene" ? true : false;
            return curScene.name == "MainScene";
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad( this );
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeScene( string strSceneName )
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene( strSceneName );
    }    
}
