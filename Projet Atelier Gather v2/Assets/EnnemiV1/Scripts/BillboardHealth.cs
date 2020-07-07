using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardHealth : MonoBehaviour
{
    public RectTransform TransformHealth;
    public GameObject mainCamera;

    void Start()
    {
        mainCamera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        TransformHealth.LookAt(mainCamera.transform);
    }
}
