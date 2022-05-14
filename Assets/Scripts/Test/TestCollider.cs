using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 一、纯Mathf向量位移
 *         遇到 目标即使是碰撞器，依然穿透
 *        不管自身是否是碰撞器或者触发器，当它接触到 目标的时候(可能是碰撞/触发器), 此时交互 没有任何反应
 *        
 * 二、Character Controller 角色控制器：
    碰撞器： 只有阻挡的效果，但是没法接收事件
    触发器：可穿透，可以接收触发器Trigger事件，但是没有办法接收碰撞器Collision事件

    三、Rigidbody 刚体
    1. 刚体自身的 Collider，如果是 碰撞器
                碰撞器：响应碰撞器事件
                触发器：不响应
    2. 刚体自身的 Collider，如果是 触发器
                碰撞器：响应触发器事件
                触发器：响应触发器事件
 */

public class TestCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log( "OnTriggerEnter：" + other.gameObject.name );
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("OnTriggerStay：" + other.gameObject.name);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("OnTriggerExit：" + other.gameObject.name);
    }

    ////////////////////////////////////////////////////////////////////////////////////

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter：" + collision.gameObject.name);
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("OnCollisionStay：" + collision.gameObject.name);
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("OnCollisionExit：" + collision.gameObject.name);
    }
}
