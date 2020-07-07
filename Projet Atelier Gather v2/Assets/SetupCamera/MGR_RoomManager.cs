using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGR_RoomManager : MonoBehaviour
{
    private static MGR_RoomManager p_instance = null;
    public static MGR_RoomManager Instance { get { return p_instance; } }

    public GameObject GO_mainCamera;
    public List<GameObject> lstGO_rooms;
    public GameObject[,] arrGO_rooms;
    public GameObject GO_currentRoom;
    public int n_sizeMaze;
    public int n_ecartRoom;

    [HideInInspector] public bool haveBossRoom;
    public GameObject GO_bossRoom;
    public GameObject GO_prebossRoom;
    public GameObject GO_prebossObject;
    private bool havePrebossObject;

    public GameObject GO_spawnerBOSS;
    public GameObject GO_BOSS;
    public List<GameObject> lst_ennemies;

    public GameObject GO_menu;
    public AudioClip audio_victorySound;

    [HideInInspector] public GameObject[] arrGO_roomsBottomDoor;
    [HideInInspector] public GameObject[] arrGO_roomsTopDoor;
    [HideInInspector] public GameObject[] arrGO_roomsLeftDoor;
    [HideInInspector] public GameObject[] arrGO_roomsRightDoor;

    public int iteration;




    void Awake()
    {
        // ===>> SingletonMAnager
        //Check if instance already exists
        if (p_instance == null)
            //if not, set instance to this
            p_instance = this;
        //If instance already exists and it's not this:
        else if (p_instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        //Sets this to not be destroyed when reloading scene
        arrGO_rooms = new GameObject[n_sizeMaze, n_sizeMaze];
        /*rooms = GameObject.FindGameObjectsWithTag("Room");
        foreach(GameObject GO in rooms)
        {
            if (GO.name == "StartRoom")
            {
                GO.GetComponent<RoomController>().arrn_idRoom[0] = n_sizeMaze / 2;
                GO.GetComponent<RoomController>().arrn_idRoom[1] = n_sizeMaze / 2;
                GO_currentRoom = GO;
                //GO.SetActive(true);
            }
            arrGO_rooms[GO.GetComponent<RoomController>().arrn_idRoom[0], GO.GetComponent<RoomController>().arrn_idRoom[1]] = GO;
            //else
            //{
            //  GO.SetActive(false);
            // }

        }*/


    }



    // Start is called before the first frame update
    void Start()
    {
        haveBossRoom = false;
        havePrebossObject = false;

    }

    public void ChangeCurrentRoom(int x, int y)
    {
        GO_currentRoom.SetActive(false);
        GO_currentRoom = arrGO_rooms[x, y];
        GO_mainCamera.GetComponent<CameraController>().SetCurrentView(GO_currentRoom.transform.position);
        foreach (GameObject GO in GO_currentRoom.GetComponent<RoomController>().arrGO_gates)
        {
            GO.GetComponent<GateController>().SetTeleport();
        }
        GO_currentRoom.SetActive(true);
        GO_currentRoom.GetComponent<RoomController>().SpawnEnnemies();

        if (GO_prebossRoom.activeSelf == true)
        {
            if (!havePrebossObject)
            {
                Vector3 pos = GO_currentRoom.transform.position;
                GameObject CubePreboss;

                switch (GO_bossRoom.GetComponent<RoomController>().arrGO_gates[0].GetComponent<GateController>().n_idGate)
                {
                    case 0:
                        CubePreboss = Instantiate(GO_prebossObject, new Vector3(pos.x, pos.y, pos.z - 7.8f), transform.rotation);
                        CubePreboss.transform.Rotate(transform.rotation.x, 180, transform.rotation.z);
                        break;
                    case 1:
                        CubePreboss = Instantiate(GO_prebossObject, new Vector3(pos.x, pos.y, pos.z - 7.8f), transform.rotation);
                        CubePreboss.transform.Rotate(transform.rotation.x, -90, transform.rotation.z);
                        break;
                    case 2:
                        CubePreboss = Instantiate(GO_prebossObject, new Vector3(pos.x, pos.y, pos.z - 7.8f), transform.rotation);
                        CubePreboss.transform.Rotate(transform.rotation.x, 0, transform.rotation.z);
                        break;
                    case 3:
                        CubePreboss = Instantiate(GO_prebossObject, new Vector3(pos.x, pos.y, pos.z - 7.8f), transform.rotation);
                        CubePreboss.transform.Rotate(transform.rotation.x, 90, transform.rotation.z);
                        break;
                }
                havePrebossObject = true;
            }

        }
        if (GO_bossRoom.activeSelf == true && !haveBossRoom)
        {
            //GO_BOSS.GetComponent<SpawnerController>().GO.SetActive(true);
            GO_bossRoom.GetComponent<RoomController>().GO_BOSSlocal.GetComponent<SpawnerController>().SpawnBoss();
            foreach (GameObject GO_gate in GO_bossRoom.GetComponent<RoomController>().arrGO_openers)
            {
                GO_gate.GetComponent<GateOpener>().ennemyCount++;
            }
        }
    }

    public void ActiveColliderCurrentRoom()
    {
        foreach (GameObject GO in GO_currentRoom.GetComponent<RoomController>().arrGO_gates)
        {
            GO.GetComponent<BoxCollider>().enabled = true;
        }
    }





    // Update is called once per frame
    void Update()
    {

    }
}
