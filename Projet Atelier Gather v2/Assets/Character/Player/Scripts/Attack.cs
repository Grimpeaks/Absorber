using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Tooltip("Number of uses left on this attack. If base attack, should remain above 0.")]
    public int m_NumberOfUse = 1;
    bool m_Started;
    public LayerMask m_LayerMask;
    public float range = 3f;
    public float radius = 1f;
    public float cooldown = 0.5f;
    public int damage = 1;
    public int knockback = 10;
    public bool canAttack = true;
    public bool m_Selected;
    Animator p_Animator;
    public AudioClip attackSound;
    protected AudioSource audioSource;

    void Start()
    {
        //Use this to ensure that the Gizmos are being drawn when in Play Mode.
        m_Started = true;
        p_Animator = gameObject.GetComponent<Animator>();
        audioSource = GetComponents<AudioSource>()[1];
        range = 3f;
        radius = 1f;
        cooldown = 0.5f;
        damage = 1;
        knockback = 10;
    }

    void Update()
    {

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    virtual public void DoAttack()
    {
        if (canAttack)
        {
            Debug.Log("ATTACK 1");
            p_Animator.SetTrigger("Attack");
            audioSource.PlayOneShot(attackSound, 0.1f);
            StartCoroutine(baseAttack());
        }
    }

    virtual public void AugmentNumberOfUse(int amount) { }

    virtual public IEnumerator baseAttack()
    {

        RaycastHit hit;

        if (Physics.SphereCast(transform.position + new Vector3(0, transform.localScale.y / 2, 0), radius, transform.TransformDirection(Vector3.forward), out hit, range, m_LayerMask))
        {
            Debug.DrawRay(transform.position + new Vector3(0, transform.localScale.y / 2, 0), transform.TransformDirection(Vector3.forward) * range, Color.yellow);
            Debug.Log("Did Hit");
            ennemiController ennemi = hit.collider.GetComponent<ennemiController>();
            ennemi.isHit(damage, knockback, transform.TransformDirection(Vector3.forward));
        }
        else
        {
            Debug.DrawRay(transform.position + new Vector3(0, transform.localScale.y / 2, 0), transform.TransformDirection(Vector3.forward) * range, Color.white);
            Debug.Log("Did not Hit");
        }

        canAttack = false;
        yield return new WaitForSecondsRealtime(cooldown);
        canAttack = true;

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (m_Started)
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireCube(transform.position, transform.localScale);
    }

}
