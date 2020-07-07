using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    
    public float f_transitionSpeed = 4;
    Vector3 Vec3_currentview;
    Vector3 PosCamera;
    // Start is called before the first frame update
    void Start()
    {
        PosCamera = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        
        if (MGR_RoomManager.Instance.GO_currentRoom)
        {
            Vector3 Vec3_curRoom = MGR_RoomManager.Instance.GO_currentRoom.transform.position;
            Vec3_currentview = new Vector3(Vec3_curRoom.x, Vec3_curRoom.y + PosCamera.y, Vec3_curRoom.z-5 + PosCamera.z);
        }
        else
        {
            Vec3_currentview = PosCamera;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, Vec3_currentview, Time.deltaTime * f_transitionSpeed);
    }
    

    public void SetCurrentView(Vector3 posview)
    {
        Vec3_currentview = new Vector3(posview.x, posview.y +PosCamera.y, posview.z - 5 + PosCamera.z);
    }
  
}
