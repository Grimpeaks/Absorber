using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public float timeToWait;

    public AudioMixer audioMixer;
    public Slider slider;
    public Text soundLevelText;

   
    
    public void PlayGame()
    {
        StartCoroutine(Wait());
        
    }

    public void PlayTuto()
    {
        StartCoroutine(WaitTuto());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(timeToWait);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator WaitTuto()
    {
        yield return new WaitForSeconds(timeToWait);
        SceneManager.LoadScene("Tutorial");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        
        soundLevelText.text = (int)(((volume+30)/30)*100) + "%";
    }
   

}
