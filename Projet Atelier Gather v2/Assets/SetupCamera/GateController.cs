using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public class GateController : MonoBehaviour
{
    public GameObject menu;
    public bool isLastRoom;
    public AudioClip victorySound;
    public int n_idGate; //0-->Left 1-->Top 2-->Right 3-->Bottom


    public GameObject teleport;
    int[] n_idCurRoom;
    Vector2 vec_idCurRoom;
    Vector2 posAdjust;

    private AudioSource source;
    

    // Start is called before the first frame update
    void Start()
    {
        
        source = gameObject.GetComponent<AudioSource>();
       
    }

    public void SetTeleport()
    {
        vec_idCurRoom = MGR_RoomManager.Instance.GO_currentRoom.GetComponent<RoomController>().vec_idRoom;
        //Debug.Log("n_idCurRoom x "+ n_idCurRoom[0] + "n_idCurRoom y " + n_idCurRoom[1]);
        switch (n_idGate)
        {
            case 0:
                teleport = MGR_RoomManager.Instance.arrGO_rooms[(int)vec_idCurRoom.x - 1, (int)vec_idCurRoom.y];
                posAdjust = new Vector2(10, -6);                  
                break;                                          
            case 1:                                             
                teleport = MGR_RoomManager.Instance.arrGO_rooms[(int)vec_idCurRoom.x, (int)vec_idCurRoom.y + 1];
                posAdjust = new Vector2(0, -19);                 
                break;                                          
            case 2:                                             
                teleport = MGR_RoomManager.Instance.arrGO_rooms[(int)vec_idCurRoom.x + 1, (int)vec_idCurRoom.y];
                posAdjust = new Vector2(-10, -6);                 
                break;                                          
            case 3:                                             
                teleport = MGR_RoomManager.Instance.arrGO_rooms[(int)vec_idCurRoom.x, (int)vec_idCurRoom.y - 1];
                posAdjust = new Vector2(0, 0);
                break;
            default:
                Debug.Log("Y a un prblm dans l'assignement des teleport");
                break;
        }
    }

  
   


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {


            if (isLastRoom)
            {
                source.PlayOneShot(victorySound);
                other.gameObject.SetActive(false);
                menu.SetActive(true);
            }
            else
            {

                other.GetComponent<NavMeshAgent>().Warp(new Vector3(teleport.transform.position.x + posAdjust.x, teleport.transform.position.y, teleport.transform.position.z + posAdjust.y));
                MGR_RoomManager.Instance.ChangeCurrentRoom((int)teleport.GetComponent<RoomController>().vec_idRoom.x, (int)teleport.GetComponent<RoomController>().vec_idRoom.y);
                
            }
        }
    }

   



}
