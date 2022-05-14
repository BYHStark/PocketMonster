using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private Transform m_player;
    private Vector3 m_v3ReleativePos;

    // Start is called before the first frame update
    void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player").transform;

        m_v3ReleativePos = transform.position - m_player.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = m_player.position + m_v3ReleativePos;
    }
}
