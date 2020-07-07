using System.Collections;
using UnityEngine;

public class MenuInstantiator : MonoBehaviour
{
    [Tooltip("Menu to instantiate 5 times")]
    public GameObject m_Menu;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InstantiateSomeMenus());
    }

    IEnumerator InstantiateSomeMenus()
    {
        for (int i =0;i<5; i++)
        {
            Instantiate(m_Menu);
            yield return new WaitForSeconds(0.075f);
        }
    }

}
