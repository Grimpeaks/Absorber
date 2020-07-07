using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : ennemiController
{
    public ParticleSystem m_ParticleSystem;
    [Tooltip("Event raised when the boss is spawned. Used to change the music.")]
    public GameEvent m_BossSpawnEvent;

    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
    }

    public void Awake()
    {
        m_BossSpawnEvent.Raise();
    }

    // Update is called once per frame
    override public void Update()
    {
        base.Update();
    }

    override public void isHit(int damages = 1, int knockbackVelocity = 10, Vector3 knockbackDirection = new Vector3())
    {
        base.isHit(damages, knockbackVelocity, knockbackDirection);
    }

    override protected void goToTarget()
    {
        base.goToTarget();
    }

    override protected void hitTarget()
    {
        base.hitTarget();
    }

    override protected IEnumerator doKnockBack(int velocity, Vector3 direction)
    {
        m_bIsKnockBack = true;
        velocity -= m_KnockBackResist;
        if (velocity < 0)
            velocity = 0;
        m_NavMeshAgent.speed -= velocity;
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

    override protected void Die()
    {
        base.Die();
        ParticleExplosion();
    }

    private void ParticleExplosion()
    {
        m_ParticleSystem.Play();
    }
}
