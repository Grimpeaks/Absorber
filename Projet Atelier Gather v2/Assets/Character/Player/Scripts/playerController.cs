using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class playerController : MonoBehaviour
{


    public float speed = 3f;
    public float jumpSpeed = 5f;
    float verticalVelocity = 0f;
    float currentSpeed;
    public int initialPlayerHealth = 3;
    int playerHealth;
    bool blinking = false;
    bool myState = true;

    public bool isInvincible = false;
    public float comebackTime = 3f;
    public LayerMask m_LayerMask;

    bool isWalking = false;
    bool isDead = false;
    public AudioClip[] m_atSounds;

    Vector3 direction = Vector3.zero; //WSAD dir
    CharacterController cc;
    AudioSource m_AudioSourceWalk;
    AudioSource m_AudioSourceOneShot;
    public GameObject healthBar;
    private EndGameScript m_EndGame;
    NavMeshAgent m_NavMeshAgent;
    SkinnedMeshRenderer mesh;
    vignetteDegats m_vignetteDegats;

    Animator m_Animator;

    //Mouse Look
    Camera MainCamera;
    LayerMask mouseLookMask;

    // Use this for initialization
    void Start()
    {
        m_EndGame = GetComponent<EndGameScript>();
        cc = GetComponent<CharacterController>();
        m_Animator = GetComponent<Animator>();
        m_AudioSourceWalk = GetComponents<AudioSource>()[0];
        m_AudioSourceOneShot = GetComponents<AudioSource>()[1];
        m_vignetteDegats = GetComponent<vignetteDegats>();

        healthBar = GameObject.Find("HealthBar");
        playerHealth = initialPlayerHealth;

        healthBar.GetComponent<HealthBarScript>().SetHealth(playerHealth, initialPlayerHealth);
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        mesh = GetComponentInChildren<SkinnedMeshRenderer>();

        MainCamera = Camera.main;

        mouseLookMask = 1 << LayerMask.NameToLayer("Lookable");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            //WSAD movement in "direction"
            direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            if (direction.magnitude > 1f)
            {
                direction = direction.normalized;
            }

            //walk bool
            if (direction.x != 0)
            {
                isWalking = true;
            }
            else if (direction.z != 0)
            {
                isWalking = true;
            }
            else
            {
                isWalking = false;
            }

            //Debug.Log("WalkDirection: " + direction);

            //look rotation

            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Physics.Raycast(ray, out hit, 100f, mouseLookMask);
            Transform objectHit = hit.transform;
            Vector3 hitPoint = hit.point;
            Vector3 playerToHitPoint = hitPoint - transform.position;
            playerToHitPoint.y = 0;

            if (playerToHitPoint.magnitude > 1f)
            {
                playerToHitPoint = playerToHitPoint.normalized;
            }

            //Debug.Log("raycast: x: " + playerToHitPoint.x + " z: " + playerToHitPoint.z);
            transform.rotation = Quaternion.LookRotation(playerToHitPoint);

            //Debug.Log("LookRotation: " + playerToHitPoint);

            //if (direction != Vector3.zero)w
            //{
            //    transform.rotation = Quaternion.LookRotation(direction);
            //}

            //Jump

            if (cc.isGrounded && Input.GetButtonDown("Jump"))
            {
                verticalVelocity = jumpSpeed;
            }

            //if (Input.GetMouseButtonDown(0))
            //{
            //    TakeDamages(1);
            //}

            UpdateAnimAndSounds();

            //StartCoroutine(isHit());
        }
    }

    void FixedUpdate()
    {
        if (!isDead)
        {
            // "direction" is the desired movement direction, based on our player's input

            Vector3 dist = direction * currentSpeed * Time.deltaTime;

            currentSpeed = speed;

            if (cc.isGrounded && verticalVelocity < 0)
            {

                //anim.SetBool("Jump", false);

                verticalVelocity = (Physics.gravity.y * 4) * Time.deltaTime;
            }
            else
            {
                // We are either not grounded, or we have a positive verticalVelocity (i.e. we ARE starting a jump)

                // To make sure we don't go into the jump animation while walking down a slope, make sure that
                // verticalVelocity is above some arbitrary threshold before triggering the animation.
                // 75% of "jumpSpeed" seems like a good safe number, but could be a standalone public variable too.
                //
                // Another option would be to do a raycast down and start the jump/fall animation whenever we were
                // more than ___ distance above the ground.
                if (Mathf.Abs(verticalVelocity) > jumpSpeed * 0.75f)
                {
                    //anim.SetBool("Jump", true);
                }

                // Apply gravity.
                verticalVelocity += (Physics.gravity.y * 4) * Time.deltaTime;
            }

            // Add our verticalVelocity to our actual movement for this frame
            dist.y = verticalVelocity * Time.deltaTime;

            // Apply the movement to our character controller (which handles collisions for us)
            cc.Move(dist);
        }

    }

    void UpdateAnimAndSounds()
    {
        if (isWalking)
        {
            m_Animator.SetBool("walk", true);
            Vector3 localVelocity = transform.InverseTransformDirection(cc.velocity).normalized;

            m_Animator.SetFloat("SpeedX", localVelocity.x);
            m_Animator.SetFloat("SpeedY", localVelocity.z);
            //Debug.Log(transform.InverseTransformDirection(cc.velocity).normalized);

            if (!m_AudioSourceWalk.isPlaying)
            {
                m_AudioSourceWalk.clip = m_atSounds[0];
                m_AudioSourceWalk.Play();
            }
        }
        else
        {
            m_Animator.SetBool("walk", false);
            m_AudioSourceWalk.Stop();
        }
    }

    public void TakeDamages(int damages, Vector3 knockDirection)
    {
        Debug.Log(isInvincible);
        if (!isInvincible)
        {
            playerHealth -= damages;
            if (playerHealth < 0)
                playerHealth = 0;
            healthBar.GetComponent<HealthBarScript>().SetHealth(playerHealth, initialPlayerHealth);
            if (playerHealth <= 0)
            {
                Death();
            }
            else
            {
                m_Animator.SetTrigger("Hit");
                m_AudioSourceOneShot.PlayOneShot(m_atSounds[1]);
                Debug.Log("Player health is " + playerHealth);
                StartCoroutine(doKnockBack(10, knockDirection));
            }
            m_vignetteDegats.launchTakeHit();
            StartCoroutine(isHit());
        }
    }

    public void Death()
    {
        isDead = true;
        m_Animator.SetBool("dead", true);
        m_AudioSourceOneShot.PlayOneShot(m_atSounds[2]);
        m_AudioSourceOneShot.PlayOneShot(m_atSounds[3]);
        m_EndGame.End();
        Debug.Log("YOU LOSE");
    }

    IEnumerator isHit()
    {
        if (isInvincible == false)
        {
            //Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale/2, Quaternion.identity, m_LayerMask);
            //int i = 0;
            ////Check when there is a new collider coming into contact with the box
            //while (i < hitColliders.Length && hitColliders[i].tag != "Dummy")
            //{
            //Output all of the collider names
            //Debug.Log("Been Hit By : " + hitColliders[i].name + i);
            //Increase the number of Colliders in the array
            //i++;
            //TakeDamages(hitColliders[i-1].GetComponent<ennemiController>().m_Damage, hitColliders[i - 1].transform.TransformDirection(Vector3.forward));
            //TakeDamages();
            //m_vignetteDegats.launchTakeHit();
            isInvincible = true;
            yield return new WaitForSecondsRealtime(comebackTime);
            isInvincible = false;
            //}
        }
    }

    private IEnumerator doKnockBack(int velocity, Vector3 direction)
    {
        m_NavMeshAgent.speed = 10;
        m_NavMeshAgent.angularSpeed = 0;
        m_NavMeshAgent.acceleration = 20;
        m_NavMeshAgent.velocity = direction * velocity;
        StartCoroutine(blink());

        yield return new WaitForSecondsRealtime(2f);

        m_NavMeshAgent.speed = 6;
        m_NavMeshAgent.angularSpeed = 360;
        m_NavMeshAgent.acceleration = 8;
        blinking = false;
        mesh.enabled = true;

    }

    private IEnumerator blink()
    {
        blinking = true;
        while (blinking)
        {
            myState = !myState;
            mesh.enabled = myState;
            yield return new WaitForSeconds(0.10f);
        }
    }
}
