using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDisplay : MonoBehaviour
{
    [Tooltip("Refference to the player.")]
    public GameObject m_PlayerRef;

    [Tooltip("Which attack does this object represents (0 being the base attack)")]
    public int m_AttackIndex;

    List<Attack> m_attacks = new List<Attack>();
    // Start is called before the first frame update
    void Start()
    {
        m_attacks = m_PlayerRef.GetComponent<AttackController>().m_attacks;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUI()
    {
        if (m_attacks[m_AttackIndex].m_Selected)
        {

        }
    }
}
