using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GateOpener : MonoBehaviour
{
    public AudioClip doorSound;
    private AudioSource audioSource;
    public int ennemyCount;
    Animator animator;
    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();        
    }

    private void OnEnable()
    {       
        animator.SetInteger("EnnemyCount", ennemyCount);
        
    }

    public void DecrementEnnemyCount()
    {
        if (ennemyCount > 0)
            ennemyCount--;
        animator.SetInteger("EnnemyCount", ennemyCount);
        if (ennemyCount == 0)
        {
            PlaySound();
            MGR_RoomManager.Instance.ActiveColliderCurrentRoom();
            
        }
    }

    public void PlaySound()
    {
        audioSource.PlayOneShot(doorSound, 0.5f);
    }

    
}
