using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatComponent : MonoBehaviour
{
    [Tooltip("Player reference so we can have fun with him :)")]
    public GameObject player;
    [Header("Events to raise per key")]
    public GameEvent eventNumpPad1;
    public GameEvent eventNumpPad2;
    public GameEvent eventNumpPad3;
    public GameEvent eventNumpPad4;
    public GameEvent eventNumpPad5;
    public GameEvent eventNumpPad6;
    public GameEvent eventNumpPad7;
    public GameEvent eventNumpPad8;
    public GameEvent eventNumpPad9;
    public GameEvent eventNumpPad0;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            if(eventNumpPad1!=null)
                eventNumpPad1.Raise();
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            if (eventNumpPad2 != null)
                eventNumpPad2.Raise();
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            if (eventNumpPad3 != null)
                eventNumpPad3.Raise();
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            if (eventNumpPad4 != null)
                eventNumpPad4.Raise();
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            if (eventNumpPad5 != null)
                eventNumpPad5.Raise();
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            if (eventNumpPad6 != null)
                eventNumpPad6.Raise();
        }
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            if (eventNumpPad7 != null)
                eventNumpPad7.Raise();
        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            if (eventNumpPad8 != null)
                eventNumpPad8.Raise();
        }
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            if (eventNumpPad9 != null)
                eventNumpPad9.Raise();
        }
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            if (eventNumpPad0 != null)
                eventNumpPad0.Raise();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (player.GetComponent<playerController>() != null)
            {
                setInvinsible();
            }
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (player.GetComponent<playerController>() != null)
            {
                player.GetComponent<playerController>().Death();
            }
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (player.GetComponent<Attack>() != null)
            {
                player.GetComponent<Attack>().damage = 100;
            }
        }
    }

    private void setInvinsible()
    {
        if (player.GetComponent<playerController>().isInvincible == false)
        {
            player.GetComponent<playerController>().isInvincible = true;
            return;
        }
        if (player.GetComponent<playerController>().isInvincible == true)
        {
            player.GetComponent<playerController>().isInvincible = false;
            return;
        }
    }
}
