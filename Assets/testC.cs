using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testC : MonoBehaviour
{
    // Start is called before the first frame update
    //Åö×²Æ÷²âÊÔ³ÌÐò
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter£º"+other.gameObject.name);
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("OnTriggerExit£º" + other.gameObject.name);
    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("OnTriggerStay£º" + other.gameObject.name);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter£º" + collision.gameObject.name);
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("OnCollisionExit£º" + collision.gameObject.name);
    }
    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("OnCollisionStay£º" + collision.gameObject.name);
    }
}
