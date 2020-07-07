using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    //Color lerpedColor = Color.green;

    //float health;
    //float initialHealth;
    //float size;
    public GameObject barreDeVie;
    Scrollbar healthBar;
    Text healthText;

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<Scrollbar>().size = (health/initialHealth);
        healthBar = GetComponent<Scrollbar>();
        healthText = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (size < 0.8f) { barreDeVie.GetComponent<Image>().color = Color.Lerp(Color.green, Color.red, ); }
        //Mathf.PingPong(Time.time, 1)
    }

    public void SetHealth(float health, float initialHealth)
    {
        healthBar.size = (health / initialHealth);
        healthText.text = health.ToString() + "/" + initialHealth.ToString();
        barreDeVie.GetComponent<Image>().color = Color.Lerp(Color.red, Color.green, healthBar.size);
        Debug.Log("Size should be " + health / initialHealth);

    }
}
