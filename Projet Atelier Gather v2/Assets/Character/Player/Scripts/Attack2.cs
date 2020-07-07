using System.Collections;
using UnityEngine;

//This is a testing class.

public class Attack2 : Attack
{

    public GameObject tentacule;
    playerController player;
    Renderer meshRenderer;

    private void Start()
    {
        audioSource = GetComponents<AudioSource>()[1];
        player = GetComponent<playerController>();
    }

    void Update()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public override void DoAttack()
    {
        if (canAttack)
        {
            Instantiate(tentacule, (transform.position + new Vector3(0,1.5f,0)) + transform.TransformDirection(Vector3.forward) * 2, transform.rotation * new Quaternion(1, 1, 1, -1));
            Animator tentacleAnimator;
            tentacleAnimator = tentacule.GetComponent<Animator>();
            tentacleAnimator.SetTrigger("Attack");
            StartCoroutine(baseAttack());
            audioSource.PlayOneShot(attackSound, 0.5f);
        }
    }

    public override IEnumerator baseAttack()
    {
        RaycastHit[] hitColliders = Physics.SphereCastAll(transform.position + new Vector3(0, transform.localScale.y / 2, 0), radius, transform.TransformDirection(Vector3.forward), range, m_LayerMask);
        int i = 0;
        Debug.Log("ATTAQUE 2");
        while (i < hitColliders.Length)
        {
            hitColliders[i].collider.gameObject.GetComponent<ennemiController>().isHit(damage, knockback, transform.TransformDirection(Vector3.forward));
            Debug.DrawRay(transform.position + new Vector3(0, transform.localScale.y / 2, 0), transform.TransformDirection(Vector3.forward) * range, Color.green);
            //Output all of the collider names
            Debug.Log("Hit : " + hitColliders[i].collider.name + i);
            //Increase the number of Colliders in the array
            i++;
        }
        m_NumberOfUse--;
        canAttack = false;
        yield return new WaitForSecondsRealtime(cooldown);
        Destroy(GameObject.Find("AttaqueTentacule(Clone)"));
        canAttack = true;
    }

    public override void AugmentNumberOfUse(int amount)
    {
        m_NumberOfUse += amount;
    }

}
