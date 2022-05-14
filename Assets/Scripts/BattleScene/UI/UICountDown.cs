using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICountDown : MonoBehaviour
{
    public Text m_text;
    [SerializeField]
    private int m_iMaxSecond = 3;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("CountDownFunc", 1, 1 );
    }

    private void CountDownFunc()
    {
        m_iMaxSecond -= 1;
        if( m_iMaxSecond > 0 )
        {
            m_text.text = m_iMaxSecond.ToString(); // m_iMaxSecond + ""; 
        }
        else
        {
            CancelInvoke( "CountDownFunc" );
            m_text.text = "Go";
            StartCoroutine( Wait() );
        }
    }

   IEnumerator Wait()
    {
        yield return new WaitForSeconds( 1 );
        this.gameObject.SetActive( false );
        UISkillGroup.Instance.Show();
    }
}
