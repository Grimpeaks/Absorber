using System.Collections;
using UnityEngine.AI;
using UnityEngine;

public class tentacleController : ennemiController
{
    private Vector3 vectorToTarget;
    // Start is called before the first frame update
    override public void Start()
    {
        m_iHP = m_iMaxHP;
        m_Animator = GetComponent<Animator>();
        m_AudioSourceOneShot = GetComponents<AudioSource>()[0];
        m_CapsuleCollider = GetComponent<CapsuleCollider>();
        m_abilityTransfer = GetComponent<AbilityTransfer>();
        m_target = GameObject.FindGameObjectWithTag("Player");
        canAttack = true;

        //---------TEST
        tst_hit = false;
        //---------TEST FIN
    }

    // Update is called once per frame
    override public void Update()
    {
        if (m_iHP > 0) // On n'execute plus rien si l'ennemi est mort
        {
            //---------TEST
            Vector3 targetPos;
            targetPos = m_target.transform.position;
            float distToTarget;
            distToTarget = Mathf.Abs((transform.position - m_target.transform.position).sqrMagnitude)/10;
            //Debug.Log(distToTarget);
            if (distToTarget <= m_attackDistance)
            {
                hitTarget();
            }

            if (tst_hit)
            {
                isHit();
            }
            tst_hit = false;
            //---------TEST FIN

            Vector3 vectorToTarget = m_target.transform.position - transform.position;
            vectorToTarget.y = 0;
            transform.rotation = Quaternion.LookRotation(vectorToTarget);
            transform.Rotate(-90, 0, -90);

        }



        else
        {
            this.enabled = false;
        }
    }

    override public void isHit(int damages = 1, int knockbackVelocity = 10, Vector3 knockbackDirection = new Vector3())
    {
        if (m_iHP > 0)
        {
            m_iHP -= damages;
            if (m_iHP > 0)
            {
                m_Animator.SetTrigger("Hit");
                m_AudioSourceOneShot.PlayOneShot(m_atSounds[0]);
            }
            else
            {
                Die();
            }
        }
    }

    private void Die()
    {
        //If you have an abilityTransfer component, Time to transfer, boi!
        if (m_abilityTransfer)
        {
            m_abilityTransfer.Transfer();
        }
        m_DeathEvent.Raise();
        m_Animator.SetBool("dead", true);
        m_CapsuleCollider.enabled = false;
        m_AudioSourceOneShot.PlayOneShot(m_atSounds[0]);
        m_AudioSourceOneShot.PlayOneShot(m_atSounds[1]);
    }

    override protected void hitTarget()
    {
        Debug.Log("Attaque de " + this.name + " sur " + m_target.name);
        if (canAttack)
        {
            Debug.DrawRay(gameObject.transform.position, transform.TransformDirection(Vector3.forward) * m_attackDistance, Color.green);
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        canAttack = false;
        m_Animator.SetTrigger("Attack");
        yield return new WaitForSeconds(m_delayPreAttack);
        if (m_iHP > 0)
        {
            m_AudioSourceOneShot.PlayOneShot(m_atSounds[3]);
            Vector3 targetPos;
            targetPos = m_target.transform.position;
            float distToTarget;
            distToTarget = Mathf.Abs((transform.position - m_target.transform.position).sqrMagnitude) / 10;
            if (distToTarget < m_attackDistance + 1)
            {
                m_target.GetComponent<playerController>().TakeDamages(m_Damage, transform.TransformDirection(Vector3.forward));
                yield return new WaitForSeconds(m_delayPostAttack);
            }
        }
        canAttack = true;
    }

}
