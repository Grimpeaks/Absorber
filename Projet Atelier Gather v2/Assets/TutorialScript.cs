using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    public string[] m_TutorialTexts;
    public Text m_TutorialTextBox;
    private int m_CurrIndex=1;

    public void Start()
    {
        m_TutorialTextBox.text = m_TutorialTexts[0];
    }

    public void NextHint()
    {
        if(m_CurrIndex < m_TutorialTexts.Length)
        {
            m_TutorialTextBox.text = m_TutorialTexts[m_CurrIndex];
            m_CurrIndex++;
        }
    }
}
