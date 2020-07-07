using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnnemiLife : MonoBehaviour
{
    public Graphic m_Graphic;
    public Color m_MyColor;
    public int m_MaxHP;
    public int m_HP;


    // Start is called before the first frame update
    void Start()
    {
        m_Graphic = GetComponentInChildren<Graphic>();
        m_MyColor = Color.green;
        m_Graphic.color = m_MyColor;
        m_MaxHP = GetComponent<ennemiController>().m_iMaxHP;
        
    }

    // Update is called once per frame
    void Update()
    {
        m_HP = GetComponent<ennemiController>().m_iHP;
        if (m_HP <= (0.75 * m_MaxHP))
        {
            m_MyColor = Color.yellow;
            m_Graphic.color = m_MyColor;
        }
        if (m_HP <= (0.5 * m_MaxHP))
        {
            m_MyColor = new Color(1.0f,0.64f,0.0f);
            m_Graphic.color = m_MyColor;
        }
        if (m_HP <= (0.25 * m_MaxHP))
        {
            m_MyColor = Color.red;
            m_Graphic.color = m_MyColor;
        }
        if (m_HP <= (0 * m_MaxHP))
        {
            m_MyColor = Color.black;
            m_Graphic.color = m_MyColor;
            StartCoroutine("Death");
        }
    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(2f);
        m_Graphic.enabled = false;
    }
}
