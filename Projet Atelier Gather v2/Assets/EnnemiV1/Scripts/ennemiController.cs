using System.Collections;
using UnityEngine.AI;
using UnityEngine;

public class ennemiController : MonoBehaviour
{
    public int m_iMaxHP;
    public int m_iHP;
    public int m_Damage;
    public int m_KnockBackResist;
    public float m_delayPreAttack;
    public float m_delayPostAttack;
    public float m_attackDistance;
    //public int m_iKnockBackForce;
    public GameEvent m_DeathEvent;
    public AudioClip[] m_atSounds;

    protected bool m_bIsKnockBack;
    protected AbilityTransfer m_abilityTransfer;
    protected EndGameScript m_GameFinisher;

    protected GameObject m_target;
    protected bool canAttack;

    protected Animator m_Animator;
    protected NavMeshAgent m_NavMeshAgent;
    protected CapsuleCollider m_CapsuleCollider;
    protected AudioSource m_AudioSourceOneShot;
    protected AudioSource m_AudioSourceWalk;


    //---------TEST
    public bool tst_hit;
    public bool tst_walkToPlayer;
    //---------TEST FIN

    // Start is called before the first frame update
    virtual public void Start()
    {
        m_iHP = m_iMaxHP;
        m_Animator = GetComponent<Animator>();
        m_AudioSourceOneShot = GetComponents<AudioSource>()[0];
        m_AudioSourceWalk = GetComponents<AudioSource>()[1];
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        m_CapsuleCollider = GetComponent<CapsuleCollider>();
        m_abilityTransfer = GetComponent<AbilityTransfer>();
        m_GameFinisher = GetComponent<EndGameScript>();
        m_target = GameObject.FindGameObjectWithTag("Player");

        m_bIsKnockBack = false;
        canAttack = true;

        //---------TEST
        tst_hit = false;
        tst_walkToPlayer = true;
        //---------TEST FIN
    }

    // Update is called once per frame
    virtual public void Update()
    {
        if (m_iHP > 0) // On n'execute plus rien si l'ennemi est mort
        {
            if (m_NavMeshAgent.velocity.magnitude > 0.5)
            {
                m_Animator.SetBool("isWalking", true);
                if (!m_AudioSourceWalk.isPlaying)
                {
                    m_AudioSourceWalk.clip = m_atSounds[2];
                    m_AudioSourceWalk.Play();
                }
            }
            else
            {
                m_Animator.SetBool("isWalking", false);
                m_AudioSourceWalk.Stop();
            }

            //---------TEST
            if (!m_bIsKnockBack)  // Ne pas mettre à jour le chemin de l'ia si knockback !!
            {
                if (tst_walkToPlayer)
                {
                    Vector3 targetPos;
                    targetPos = m_target.transform.position;
                    float distToTarget;
                    distToTarget = Mathf.Abs((transform.position - m_target.transform.position).sqrMagnitude)/10;
                    //Debug.Log(distToTarget);
                    if (distToTarget > m_attackDistance)
                    {
                        goToTarget();
                    }
                    else
                    {
                        hitTarget();
                    }

                }
                else if (!m_bIsKnockBack)
                {
                    m_NavMeshAgent.SetDestination(gameObject.transform.position);
                }
            }

            if (tst_hit)
            {
                isHit();
            }
            tst_hit = false;
            //---------TEST FIN
        }

        else
        {
            this.enabled = false;
        }
    }

    virtual public void isHit(int damages = 1, int knockbackVelocity = 10, Vector3 knockbackDirection = new Vector3())
    {
        if (m_iHP > 0)
        {
            m_iHP -= damages;
            if (m_iHP > 0)
            {
                m_Animator.SetTrigger("isHit");
                knockbackVelocity -= m_KnockBackResist;
                if (knockbackVelocity < 0)
                {
                    knockbackVelocity = 0;
                }
                StartCoroutine(doKnockBack(knockbackVelocity, knockbackDirection));
                m_AudioSourceOneShot.PlayOneShot(m_atSounds[0]);
            }
            else
            {
                Die();
            }
        }
    }

    virtual protected void Die()
    {
        //If you have an abilityTransfer component, Time to transfer, boi!
        if (m_abilityTransfer)
        {
            m_abilityTransfer.Transfer();
        }
        m_DeathEvent.Raise();
        m_Animator.SetBool("isDead", true);
        m_CapsuleCollider.enabled = false;
        m_AudioSourceOneShot.PlayOneShot(m_atSounds[0]);
        m_AudioSourceOneShot.PlayOneShot(m_atSounds[1]);
        m_NavMeshAgent.enabled = false;
        m_AudioSourceWalk.Stop();
        if (m_GameFinisher != null)
            m_GameFinisher.End();
    }

    virtual protected IEnumerator doKnockBack(int velocity, Vector3 direction)
    {
        m_NavMeshAgent.ResetPath();
        m_bIsKnockBack = true;
        m_NavMeshAgent.speed = 10;
        m_NavMeshAgent.angularSpeed = 0;
        m_NavMeshAgent.acceleration = 20;
        m_NavMeshAgent.velocity = direction * velocity; //-transform.forward * velocity; 

        yield return new WaitForSecondsRealtime(0.5f);

        m_bIsKnockBack = false;
        m_NavMeshAgent.speed = 6;
        m_NavMeshAgent.angularSpeed = 360;
        m_NavMeshAgent.acceleration = 8;
        //m_NavMeshAgent.isStopped = false;
    }

    virtual protected void goToTarget()
    {
        Vector3 targetPos;
        targetPos = m_target.transform.position;
        m_NavMeshAgent.isStopped = false;
        m_NavMeshAgent.SetDestination(targetPos);
    }

    virtual protected void hitTarget()
    {
        m_NavMeshAgent.isStopped = true;
        Debug.Log("Attaque de " + this.name + " sur " + m_target.name);
        if (canAttack)
        {
            StartCoroutine(Attack());
        }
        transform.LookAt(m_target.transform.position);
    }



    IEnumerator Attack()
    {
        canAttack = false;
        m_Animator.SetTrigger("attack");
        yield return new WaitForSeconds(m_delayPreAttack);
        if (m_iHP > 0)
        {
            m_AudioSourceOneShot.PlayOneShot(m_atSounds[3]);
            Vector3 targetPos;
            targetPos = m_target.transform.position;
            float distToTarget;
            distToTarget = Mathf.Abs((transform.position - m_target.transform.position).sqrMagnitude) / 10;
            if (distToTarget < m_attackDistance)
            {
                m_target.GetComponent<playerController>().TakeDamages(m_Damage, transform.TransformDirection(Vector3.forward));
                yield return new WaitForSeconds(m_delayPostAttack);
            }
        }
        canAttack = true;
    }
}
