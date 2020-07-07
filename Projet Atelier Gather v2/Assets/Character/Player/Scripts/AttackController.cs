using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [HideInInspector]
    public List<Attack> m_attacks = new List<Attack>();
    [Tooltip("UI element to display the attacks")]
    public List<Image> m_AttacksImages = new List<Image>();
    [Tooltip("UI element to show the cooldown. Should be the child image of the attack's image")]
    public List<Image> m_CooldownImages = new List<Image>();
    [Tooltip("Sound to play when an ability is transfered")]
    public AudioClip m_AbsorbSound;
    [Tooltip("Sound to play when the player changes his attack")]
    public AudioClip m_SwapSound;
    private AudioSource m_AudioSource;
    private int m_SelectedAttackIndex;
    private Attack m_SelectedAttack;

    void Start()
    {
        m_AudioSource = GetComponents<AudioSource>()[2];
        m_attacks.Add(GetComponent<Attack>());
        m_attacks.Add(GetComponent<Attack2>());
        m_SelectedAttack = m_attacks[0];
        m_SelectedAttack.m_Selected = true;
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Cooldown display
            if (m_SelectedAttack.canAttack)
                m_CooldownImages[m_SelectedAttackIndex].fillAmount = 0;

            m_SelectedAttack.DoAttack();
            if (m_SelectedAttack.m_NumberOfUse <= 0)
                GoToPower(0);
            UpdateUI();
        }

        if (Input.mouseScrollDelta.y > 0)
        {
            m_CooldownImages[m_SelectedAttackIndex].fillAmount = 0;
            NextAttack();
            m_CooldownImages[m_SelectedAttackIndex].fillAmount = 1;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            m_CooldownImages[m_SelectedAttackIndex].fillAmount = 0;
            PreviousAttack();
            m_CooldownImages[m_SelectedAttackIndex].fillAmount = 1;
        }

        //Cooldown logic
        if (!m_SelectedAttack.canAttack)
        {
            m_CooldownImages[m_SelectedAttackIndex].fillAmount += 1.0f / m_SelectedAttack.cooldown * Time.deltaTime;
        }

        //Bunch of ugly if's to go directly to a specific power. 
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                GoToPower(0);
            if (Input.GetKeyDown(KeyCode.Alpha2))
                GoToPower(1);
            if (Input.GetKeyDown(KeyCode.Alpha3))
                GoToPower(2);
            if (Input.GetKeyDown(KeyCode.Alpha4))
                GoToPower(3);
            if (Input.GetKeyDown(KeyCode.Alpha5))
                GoToPower(4);
            if (Input.GetKeyDown(KeyCode.Alpha6))
                GoToPower(5);
            if (Input.GetKeyDown(KeyCode.Alpha7))
                GoToPower(6);
            if (Input.GetKeyDown(KeyCode.Alpha8))
                GoToPower(7);
            if (Input.GetKeyDown(KeyCode.Alpha9))
                GoToPower(8);
        }

    }

    private void NextAttack()
    {
        if (m_SelectedAttackIndex < m_attacks.Count - 1)
        {
            m_SelectedAttackIndex++;
            if (m_attacks[m_SelectedAttackIndex].m_NumberOfUse <= 0)
                NextAttack();
            GoToPower(m_SelectedAttackIndex);
        }
        else
        {
            m_SelectedAttackIndex = 0;
            GoToPower(m_SelectedAttackIndex);
        }
        Debug.Log("Attaque: " + m_SelectedAttack);
    }

    private void PreviousAttack()
    {
        if (m_SelectedAttackIndex > 0)
        {
            m_SelectedAttackIndex--;
            if (m_attacks[m_SelectedAttackIndex].m_NumberOfUse <= 0)
                PreviousAttack();
            GoToPower(m_SelectedAttackIndex);
        }
        else
        {
            m_SelectedAttackIndex = m_attacks.Count - 1;
            while (m_attacks[m_SelectedAttackIndex].m_NumberOfUse <= 0)
            {
                m_SelectedAttackIndex--;
            }
            GoToPower(m_SelectedAttackIndex);
        }
        Debug.Log("Attaque: " + m_SelectedAttack);
    }

    private void GoToPower(int powerIndex)
    {
        if (powerIndex < m_attacks.Count)     //If the attack exists
        {
            if (m_attacks[powerIndex].m_NumberOfUse > 0)      //If the attack can be used
            {
                m_CooldownImages[m_SelectedAttackIndex].fillAmount = 0;
                m_CooldownImages[powerIndex].fillAmount = 1;
                if (m_SelectedAttack != m_attacks[powerIndex])     //Do not play the sound if you're not actualy changing power ò.ó
                    m_AudioSource.PlayOneShot(m_SwapSound, 0.25f);
                m_SelectedAttackIndex = powerIndex;
                m_SelectedAttack.m_Selected = false;
                m_SelectedAttack = m_attacks[m_SelectedAttackIndex];
                m_SelectedAttack.m_Selected = true;
            }
        }
        UpdateUI();
    }

    public void UpdateUI()
    {

        Color tmpColorNotSelected;
        Color tmpColorSelected;
        for (int index = 0; index < m_attacks.Count; index++)
        {
            if (m_AttacksImages[index] != null)
            {
                tmpColorNotSelected = m_AttacksImages[index].color;
                tmpColorNotSelected.a = 0.5f;
                tmpColorSelected = m_AttacksImages[index].color;
                tmpColorSelected.a = 0.5f;


                if (m_attacks[index].m_NumberOfUse <= 0)
                {
                    StartCoroutine(FadeAway(m_AttacksImages[index]));
                    m_CooldownImages[index].fillAmount = 0;
                    m_AttacksImages[index].GetComponentInChildren<Text>().text = "";
                }
                else if (m_attacks[index].m_Selected)
                {
                    m_AttacksImages[index].color = tmpColorSelected;
                    if (index != 0)
                        m_AttacksImages[index].GetComponentInChildren<Text>().text = m_attacks[index].m_NumberOfUse.ToString();
                }
                else
                {
                    m_AttacksImages[index].color = tmpColorNotSelected;
                    if (index != 0)
                        m_AttacksImages[index].GetComponentInChildren<Text>().text = m_attacks[index].m_NumberOfUse.ToString();
                }
            }
        }
    }

    public void PlayAbsorbSound()
    {
        m_AudioSource.pitch = 0.5f;
        m_AudioSource.PlayOneShot(m_AbsorbSound, 0.15f);
    }

    IEnumerator FadeAway(Image imageToDisable)
    {
        Color disabledColor;
        disabledColor = imageToDisable.color;
        while (imageToDisable.color.a > 0)
        {
            disabledColor.a -= 0.05f;
            imageToDisable.color = disabledColor;
            yield return new WaitForSeconds(.01f);
        }
    }

    //IEnumerator Cooldown(int cd, int indexAttack)
    //{
    //    Transform cooldownImage = m_AttacksImages[indexAttack].transform.GetChild(0);
    //    cooldownImage.GetComponent<Image>().
    //    yield return new WaitForSeconds(cd/100);
    //}

}


