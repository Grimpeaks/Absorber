using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource m_AudioSource;
    public AudioClip m_ExplorationSong;
    public AudioClip m_BossSong;

    // Start is called before the first frame update
    void Start()
    {
        m_AudioSource.clip = m_ExplorationSong;
        m_AudioSource.Play();
    }

    public void PlayBossSong()
    {
        m_AudioSource.clip = m_BossSong;
        m_AudioSource.Play();
    }
}
