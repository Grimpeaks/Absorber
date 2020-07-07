using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGameScript : MonoBehaviour
{
    [Tooltip("Sound to play when the game ends!")]
    public AudioClip m_Sound;
    [Tooltip("GameEvent to raise to end the game!")]
    public GameEvent m_Event;
    public Image m_FadeImage;
    [Tooltip("Time before the game actualy ends (Seconds)")]
    public float m_Time;
    private AudioSource source;
    private Color m_Temp;

    private void Start()
    {
        source = GetComponents<AudioSource>()[1];
        m_Temp = m_FadeImage.color;
    }


    public void End()
    {
        source.PlayOneShot(m_Sound);
        m_Event.Raise();
        m_Event.Raise();
        StartCoroutine(EndGameTimer(m_Time));
    }

    private void Finish()
    {
        SceneManager.LoadScene("EndGameScene");
    }

    IEnumerator EndGameTimer(float time)
    {
        for(int i = 0; i <= 100; i++)
        {
            m_Temp.a += 0.01f;
            m_FadeImage.color = m_Temp;
            yield return new WaitForSeconds(time/100);
        }
        Finish();
    }
}
