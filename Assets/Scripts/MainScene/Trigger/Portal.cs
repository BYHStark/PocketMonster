using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag != "Player")
            return;


        // 切换到战斗场景
        SceneManager.Instance.ChangeScene( "Scenes/BattleScene" );
    }
}
