using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testC : MonoBehaviour
{
    // Start is called before the first frame update
    //��ײ�����Գ���
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter��"+other.gameObject.name);
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("OnTriggerExit��" + other.gameObject.name);
    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("OnTriggerStay��" + other.gameObject.name);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter��" + collision.gameObject.name);
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("OnCollisionExit��" + collision.gameObject.name);
    }
    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("OnCollisionStay��" + collision.gameObject.name);
    }
}
