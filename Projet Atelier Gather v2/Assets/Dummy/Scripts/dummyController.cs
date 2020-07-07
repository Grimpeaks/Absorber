using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dummyController : MonoBehaviour
{
    public GameEvent deathEvent; //Event to raise on death.
    Animator m_Animator;
    public int maxHP;
    public int HP;
    public int knockBackForce;
    public AudioClip[] sounds;
    AudioSource audioSource;

    public Rigidbody dummyRigidbody;
    private CapsuleCollider selfCollider;


    //---------TEST
    public bool hit;
    //---------TEST FIN

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
        HP = maxHP;

        audioSource = GetComponent<AudioSource>();

        dummyRigidbody = GetComponent<Rigidbody>();
        selfCollider = gameObject.GetComponent<CapsuleCollider>();

        //---------TEST
        hit = false;
        //---------TEST FIN
    }

    // Update is called once per frame
    void Update()
    {
        //---------TEST
        if (hit)
        {
            isHit();
        }
        hit = false;
        //---------TEST FIN
    }

    public void isHit()
    {
        --HP;
        if (HP > 0)
        {
            m_Animator.SetTrigger("isHit");
            audioSource.PlayOneShot(sounds[0]);
            doKnockBack();
        }
        else
        {
            deathEvent.Raise();
            m_Animator.SetBool("isDead", true);
            audioSource.PlayOneShot(sounds[1]);
            dummyRigidbody.useGravity = false;
            selfCollider.enabled = false;
        }
    }

    void doKnockBack()
    {
        dummyRigidbody.AddForce(-transform.forward * knockBackForce);
    }
}
