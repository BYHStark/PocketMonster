using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvent : MonoBehaviour
{
   public void Damage( float damage )
    {
        BattleManager.Instance.Damage( damage );
    }

    public void ResetMoveEvent()
    {
        BattleManager.Instance.ResetMoveEvent();
    }
}
