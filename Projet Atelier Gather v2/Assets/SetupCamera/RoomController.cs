using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{

    public GameObject[] arrGO_gates;
    public GameObject[] arrGO_spawners;
    public GameObject[] arrGO_openers;
    public Vector2 vec_idRoom;
    public GameObject GO_room;
    public GameObject GO_BOSSlocal;
    public int ennemyCount = 0;
    private bool spawned = false;
    private bool ennemiesSpawned = false;
    private int rand;

    // Start is called before the first frame update
    void Start()
    {
        //arrn_idRoom = new int[2];

        Vector3 pos = transform.position;
        if (gameObject.name == "StartRoom")
        {
            vec_idRoom.x = MGR_RoomManager.Instance.n_sizeMaze / 2;
            vec_idRoom.y = MGR_RoomManager.Instance.n_sizeMaze / 2;
            MGR_RoomManager.Instance.GO_currentRoom = gameObject;
            MGR_RoomManager.Instance.arrGO_rooms[(int)vec_idRoom.x, (int)vec_idRoom.y] = gameObject;

        }
        if (gameObject.tag == "EndRoom")
        {
            if (MGR_RoomManager.Instance.GO_bossRoom != null)
            {

                Destroy(MGR_RoomManager.Instance.GO_bossRoom.GetComponent<RoomController>().GO_BOSSlocal);
            }

            foreach (GameObject GOgateBR in arrGO_gates)
            {
                switch (GOgateBR.GetComponent<GateController>().n_idGate)
                {
                    case 0:
                        GO_BOSSlocal = Instantiate(MGR_RoomManager.Instance.GO_spawnerBOSS, new Vector3(pos.x + 10.0f, pos.y, pos.z - 6), transform.rotation);
                        MGR_RoomManager.Instance.GO_prebossRoom = MGR_RoomManager.Instance.arrGO_rooms[(int)vec_idRoom.x - 1, (int)vec_idRoom.y];
                        break;
                    case 1:
                        GO_BOSSlocal = Instantiate(MGR_RoomManager.Instance.GO_spawnerBOSS, new Vector3(pos.x, pos.y, pos.z - 19), transform.rotation);
                        MGR_RoomManager.Instance.GO_prebossRoom = MGR_RoomManager.Instance.arrGO_rooms[(int)vec_idRoom.x, (int)vec_idRoom.y + 1];
                        break;
                    case 2:
                        GO_BOSSlocal = Instantiate(MGR_RoomManager.Instance.GO_spawnerBOSS, new Vector3(pos.x - 10.0f, pos.y, pos.z - 6), transform.rotation);
                        MGR_RoomManager.Instance.GO_prebossRoom = MGR_RoomManager.Instance.arrGO_rooms[(int)vec_idRoom.x + 1, (int)vec_idRoom.y];
                        break;
                    case 3:
                        GO_BOSSlocal = Instantiate(MGR_RoomManager.Instance.GO_spawnerBOSS, new Vector3(pos.x, pos.y, pos.z), transform.rotation);
                        MGR_RoomManager.Instance.GO_prebossRoom = MGR_RoomManager.Instance.arrGO_rooms[(int)vec_idRoom.x, (int)vec_idRoom.y - 1];
                        break;
                        
                }
                GOgateBR.GetComponent<GateController>().menu = MGR_RoomManager.Instance.GO_menu;
                GOgateBR.GetComponent<GateController>().victorySound = MGR_RoomManager.Instance.audio_victorySound;
               
            }

            MGR_RoomManager.Instance.GO_bossRoom = gameObject;          
        }

        Spawn2();

    }


    public void SpawnEnnemies()
    {
        if (!ennemiesSpawned)
        {
            foreach (GameObject Spawners in arrGO_spawners)
            {
                int rand = Random.Range(0, 2);
                if (rand == 0)
                {
                    foreach (GameObject Ennemies in Spawners.GetComponent<SpawnerController>().listGO_ennemies)
                    {
                        ennemyCount++;
                        Ennemies.SetActive(true);
                    }
                    ennemiesSpawned = true;
                }
            }

            if (!ennemiesSpawned)
            {

                int rand = Random.Range(0, arrGO_spawners.Length);
                foreach (GameObject Ennemies in arrGO_spawners[rand].GetComponent<SpawnerController>().listGO_ennemies)
                {
                    ennemyCount++;

                    Ennemies.SetActive(true);
                }
                ennemiesSpawned = true;
            }

            foreach (GameObject Openers in arrGO_openers)
            {
                Openers.GetComponent<GateOpener>().ennemyCount = ennemyCount;
            }
        }
    }


    void Spawn()
    {
        foreach (GameObject GO in arrGO_gates)
        {
            GO.GetComponent<BoxCollider>().enabled = false;
            //Debug.Log("Gameobject : " + GO);
            if (!spawned)
            {
                Vector3 center = transform.position;
                switch (GO.GetComponent<GateController>().n_idGate)
                {

                    case 0:

                        if (vec_idRoom.x - 1 != 0)
                        {
                            rand = Random.Range(0, MGR_RoomManager.Instance.arrGO_roomsRightDoor.Length);
                        }
                        else
                        {
                            rand = 0;
                        }
                        if (MGR_RoomManager.Instance.arrGO_rooms[(int)vec_idRoom.x - 1, (int)vec_idRoom.y] == null)
                        {

                            GO_room = Instantiate(MGR_RoomManager.Instance.arrGO_roomsRightDoor[rand], new Vector3(center.x - MGR_RoomManager.Instance.n_ecartRoom, center.y, center.z), MGR_RoomManager.Instance.arrGO_roomsBottomDoor[rand].transform.rotation);

                            MGR_RoomManager.Instance.arrGO_rooms[(int)vec_idRoom.x - 1, (int)vec_idRoom.y] = GO_room;
                            GO_room.GetComponent<RoomController>().vec_idRoom.x = vec_idRoom.x - 1;
                            GO_room.GetComponent<RoomController>().vec_idRoom.y = vec_idRoom.y;
                        }

                        break;
                    case 1:
                        if (vec_idRoom.y + 1 != MGR_RoomManager.Instance.n_sizeMaze - 1)
                        {
                            rand = Random.Range(0, MGR_RoomManager.Instance.arrGO_roomsBottomDoor.Length);
                        }
                        else
                        {
                            rand = 0;
                        }
                        if (MGR_RoomManager.Instance.arrGO_rooms[(int)vec_idRoom.x, (int)vec_idRoom.y + 1] == null)
                        {
                            GO_room = Instantiate(MGR_RoomManager.Instance.arrGO_roomsBottomDoor[rand], new Vector3(center.x, center.y, center.z + MGR_RoomManager.Instance.n_ecartRoom), MGR_RoomManager.Instance.arrGO_roomsBottomDoor[rand].transform.rotation);
                            MGR_RoomManager.Instance.arrGO_rooms[(int)vec_idRoom.x, (int)vec_idRoom.y + 1] = GO_room;
                            GO_room.GetComponent<RoomController>().vec_idRoom.x = vec_idRoom.x;
                            GO_room.GetComponent<RoomController>().vec_idRoom.y = vec_idRoom.y + 1;
                        }

                        break;
                    case 2:
                        if (vec_idRoom.x + 1 != MGR_RoomManager.Instance.n_sizeMaze - 1)
                        {
                            rand = Random.Range(0, MGR_RoomManager.Instance.arrGO_roomsLeftDoor.Length);
                        }
                        else
                        {
                            rand = 0;
                        }
                        if (MGR_RoomManager.Instance.arrGO_rooms[(int)vec_idRoom.x + 1, (int)vec_idRoom.y] == null)
                        {
                            GO_room = Instantiate(MGR_RoomManager.Instance.arrGO_roomsLeftDoor[rand], new Vector3(center.x + MGR_RoomManager.Instance.n_ecartRoom, center.y, center.z), MGR_RoomManager.Instance.arrGO_roomsLeftDoor[rand].transform.rotation);
                            MGR_RoomManager.Instance.arrGO_rooms[(int)vec_idRoom.x + 1, (int)vec_idRoom.y] = GO_room;
                            GO_room.GetComponent<RoomController>().vec_idRoom.x = vec_idRoom.x + 1;
                            GO_room.GetComponent<RoomController>().vec_idRoom.y = vec_idRoom.y;
                        }

                        break;
                    case 3:
                        if (vec_idRoom.y - 1 != 0)
                        {
                            rand = Random.Range(0, MGR_RoomManager.Instance.arrGO_roomsTopDoor.Length);
                        }
                        else
                        {
                            rand = 0;
                        }
                        if (MGR_RoomManager.Instance.arrGO_rooms[(int)vec_idRoom.x, (int)vec_idRoom.y - 1] == null)
                        {
                            GO_room = Instantiate(MGR_RoomManager.Instance.arrGO_roomsTopDoor[rand], new Vector3(center.x, center.y, center.z - MGR_RoomManager.Instance.n_ecartRoom), MGR_RoomManager.Instance.arrGO_roomsTopDoor[rand].transform.rotation);
                            MGR_RoomManager.Instance.arrGO_rooms[(int)vec_idRoom.x, (int)vec_idRoom.y - 1] = GO_room;
                            GO_room.GetComponent<RoomController>().vec_idRoom.x = vec_idRoom.x;
                            GO_room.GetComponent<RoomController>().vec_idRoom.y = vec_idRoom.y - 1;
                        }

                        break;
                    default:
                        break;
                }
            }
            GO.GetComponent<GateController>().SetTeleport();
            if (!MGR_RoomManager.Instance.haveBossRoom)
            {

                if (rand == 0)
                {
                    GO.GetComponent<GateController>().isLastRoom = true;
                    GO.GetComponent<GateController>().menu = MGR_RoomManager.Instance.GO_menu;
                    GO.GetComponent<GateController>().victorySound = MGR_RoomManager.Instance.audio_victorySound;

                    MGR_RoomManager.Instance.haveBossRoom = true;
                }
            }

        }
        spawned = true;
        if (gameObject.name != "StartRoom")
        {
            gameObject.SetActive(false);
        }

    }


    void Spawn2()
    {

        MGR_RoomManager.Instance.iteration++;
        GameObject[,] arrGO_rooms = MGR_RoomManager.Instance.arrGO_rooms;
        List<GameObject> lstRooms;
        GameObject room;
        foreach (GameObject GO in arrGO_gates)
        {
            int left = 0;
            int top = 0;
            int right = 0;
            int bottom = 0;

            GO.GetComponent<BoxCollider>().enabled = false;

            if (!spawned)
            {
                Vector3 center = transform.position;
                switch (GO.GetComponent<GateController>().n_idGate)
                {

                    case 0://gauche
                        right = 1;
                        if (arrGO_rooms[(int)vec_idRoom.x - 1, (int)vec_idRoom.y] == null)
                        {

                            if (vec_idRoom.x - 1 != 0)
                            {

                                if (arrGO_rooms[(int)vec_idRoom.x - 2, (int)vec_idRoom.y] == null)
                                {
                                    left = 2;
                                }
                                else
                                {
                                    left = 0;
                                    foreach (GameObject GOgate in arrGO_rooms[(int)vec_idRoom.x - 2, (int)vec_idRoom.y].GetComponent<RoomController>().arrGO_gates)
                                    {
                                        if (GOgate.GetComponent<GateController>().n_idGate == 2)
                                        {
                                            left = 1;
                                        }
                                    }
                                }

                                if (arrGO_rooms[(int)vec_idRoom.x - 1, (int)vec_idRoom.y + 1] == null)
                                {
                                    top = 2;
                                }
                                else
                                {
                                    top = 0;
                                    foreach (GameObject GOgate in arrGO_rooms[(int)vec_idRoom.x-1, (int)vec_idRoom.y + 1].GetComponent<RoomController>().arrGO_gates)
                                    {
                                        if (GOgate.GetComponent<GateController>().n_idGate == 3)
                                        {
                                            top = 1;
                                        }
                                    }
                                }

                                if (arrGO_rooms[(int)vec_idRoom.x - 1, (int)vec_idRoom.y - 1] == null)
                                {
                                    bottom = 2;
                                }
                                else
                                {
                                    bottom = 0;
                                    foreach (GameObject GOgate in arrGO_rooms[(int)vec_idRoom.x, (int)vec_idRoom.y - 1].GetComponent<RoomController>().arrGO_gates)
                                    {
                                        if (GOgate.GetComponent<GateController>().n_idGate == 1)
                                        {
                                            bottom = 1;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                left = 0;
                                top = 0;
                                bottom = 0;
                            }
                            lstRooms = RandomSalles(left, top, right, bottom);
                            rand = Random.Range(0, lstRooms.Count);
                            room = lstRooms[rand];
                            GO_room = Instantiate(room, new Vector3(center.x - MGR_RoomManager.Instance.n_ecartRoom, center.y, center.z), room.transform.rotation);
                            MGR_RoomManager.Instance.arrGO_rooms[(int)vec_idRoom.x - 1, (int)vec_idRoom.y] = GO_room;
                            GO_room.GetComponent<RoomController>().vec_idRoom.x = vec_idRoom.x - 1;
                            GO_room.GetComponent<RoomController>().vec_idRoom.y = vec_idRoom.y;
                        }

                        break;
                    case 1://haut
                        bottom = 1;
                        if (arrGO_rooms[(int)vec_idRoom.x, (int)vec_idRoom.y + 1] == null)
                        {
                            if (vec_idRoom.y + 1 != MGR_RoomManager.Instance.n_sizeMaze - 1)
                            {

                                if (arrGO_rooms[(int)vec_idRoom.x - 1, (int)vec_idRoom.y + 1] == null)
                                {
                                    left = 2;
                                }
                                else
                                {
                                    left = 0;
                                    foreach (GameObject GOgate in arrGO_rooms[(int)vec_idRoom.x - 1, (int)vec_idRoom.y + 1].GetComponent<RoomController>().arrGO_gates)
                                    {
                                        if (GOgate.GetComponent<GateController>().n_idGate == 2)
                                        {
                                            left = 1;
                                        }
                                    }
                                }

                                if (arrGO_rooms[(int)vec_idRoom.x, (int)vec_idRoom.y + 2] == null)
                                {
                                    top = 2;
                                }
                                else
                                {
                                    top = 0;
                                    foreach (GameObject GOgate in arrGO_rooms[(int)vec_idRoom.x, (int)vec_idRoom.y + 2].GetComponent<RoomController>().arrGO_gates)
                                    {
                                        if (GOgate.GetComponent<GateController>().n_idGate == 3)
                                        {
                                            top = 1;
                                        }
                                    }
                                }

                                if (arrGO_rooms[(int)vec_idRoom.x + 1, (int)vec_idRoom.y + 1] == null)
                                {
                                    right = 2;
                                }
                                else
                                {
                                    right = 0;
                                    foreach (GameObject GOgate in arrGO_rooms[(int)vec_idRoom.x + 1, (int)vec_idRoom.y + 1].GetComponent<RoomController>().arrGO_gates)
                                    {
                                        if (GOgate.GetComponent<GateController>().n_idGate == 0)
                                        {
                                            right = 1;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                left = 0;
                                top = 0;
                                right = 0;
                            }



                            lstRooms = RandomSalles(left, top, right, bottom);
                            rand = Random.Range(0, lstRooms.Count);
                            room = lstRooms[rand];
                            GO_room = Instantiate(room, new Vector3(center.x, center.y, center.z + MGR_RoomManager.Instance.n_ecartRoom), room.transform.rotation);
                            MGR_RoomManager.Instance.arrGO_rooms[(int)vec_idRoom.x, (int)vec_idRoom.y + 1] = GO_room;
                            GO_room.GetComponent<RoomController>().vec_idRoom.x = vec_idRoom.x;
                            GO_room.GetComponent<RoomController>().vec_idRoom.y = vec_idRoom.y + 1;
                        }

                        break;
                    case 2://droite
                        left = 1;
                        if (arrGO_rooms[(int)vec_idRoom.x + 1, (int)vec_idRoom.y] == null)
                        {
                            if (vec_idRoom.x + 1 != MGR_RoomManager.Instance.n_sizeMaze - 1)
                            {

                                if (arrGO_rooms[(int)vec_idRoom.x + 1, (int)vec_idRoom.y + 1] == null)
                                {
                                    top = 2;
                                }
                                else
                                {
                                    top = 0;
                                    foreach (GameObject GOgate in arrGO_rooms[(int)vec_idRoom.x + 1, (int)vec_idRoom.y + 1].GetComponent<RoomController>().arrGO_gates)
                                    {
                                        if (GOgate.GetComponent<GateController>().n_idGate == 3)
                                        {
                                            top = 1;
                                        }
                                    }
                                }

                                if (arrGO_rooms[(int)vec_idRoom.x + 2, (int)vec_idRoom.y] == null)
                                {
                                    right = 2;
                                }
                                else
                                {
                                    right = 0;
                                    foreach (GameObject GOgate in arrGO_rooms[(int)vec_idRoom.x + 2, (int)vec_idRoom.y].GetComponent<RoomController>().arrGO_gates)
                                    {
                                        if (GOgate.GetComponent<GateController>().n_idGate == 0)
                                        {
                                            right = 1;
                                        }
                                    }
                                }

                                if (arrGO_rooms[(int)vec_idRoom.x + 1, (int)vec_idRoom.y - 1] == null)
                                {
                                    bottom = 2;
                                }
                                else
                                {
                                    bottom = 0;
                                    foreach (GameObject GOgate in arrGO_rooms[(int)vec_idRoom.x + 1, (int)vec_idRoom.y - 1].GetComponent<RoomController>().arrGO_gates)
                                    {
                                        if (GOgate.GetComponent<GateController>().n_idGate == 1)
                                        {
                                            bottom = 1;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                top = 0;
                                right = 0;
                                bottom = 0;
                            }



                            lstRooms = RandomSalles(left, top, right, bottom);
                            rand = Random.Range(0, lstRooms.Count);
                            room = lstRooms[rand];
                            GO_room = Instantiate(room, new Vector3(center.x + MGR_RoomManager.Instance.n_ecartRoom, center.y, center.z), room.transform.rotation);
                            MGR_RoomManager.Instance.arrGO_rooms[(int)vec_idRoom.x + 1, (int)vec_idRoom.y] = GO_room;
                            GO_room.GetComponent<RoomController>().vec_idRoom.x = vec_idRoom.x + 1;
                            GO_room.GetComponent<RoomController>().vec_idRoom.y = vec_idRoom.y;
                        }
                        break;
                    case 3://bas
                        top = 1;
                        if (arrGO_rooms[(int)vec_idRoom.x, (int)vec_idRoom.y - 1] == null)
                        {
                            if (vec_idRoom.y - 1 != 0)
                            {

                                if (arrGO_rooms[(int)vec_idRoom.x - 1, (int)vec_idRoom.y - 1] == null)
                                {
                                    left = 2;
                                }
                                else
                                {
                                    left = 0;
                                    foreach (GameObject GOgate in arrGO_rooms[(int)vec_idRoom.x - 1, (int)vec_idRoom.y - 1].GetComponent<RoomController>().arrGO_gates)
                                    {
                                        if (GOgate.GetComponent<GateController>().n_idGate == 2)
                                        {
                                            left = 1;
                                        }
                                    }
                                }

                                if (arrGO_rooms[(int)vec_idRoom.x + 1, (int)vec_idRoom.y - 1] == null)
                                {
                                    right = 2;
                                }
                                else
                                {
                                    right = 0;
                                    foreach (GameObject GOgate in arrGO_rooms[(int)vec_idRoom.x + 1, (int)vec_idRoom.y - 1].GetComponent<RoomController>().arrGO_gates)
                                    {
                                        if (GOgate.GetComponent<GateController>().n_idGate == 0)
                                        {
                                            right = 1;
                                        }
                                    }
                                }

                                if (arrGO_rooms[(int)vec_idRoom.x, (int)vec_idRoom.y - 2] == null)
                                {
                                    bottom = 2;
                                }
                                else
                                {
                                    bottom = 0;
                                    foreach (GameObject GOgate in arrGO_rooms[(int)vec_idRoom.x, (int)vec_idRoom.y - 2].GetComponent<RoomController>().arrGO_gates)
                                    {
                                        if (GOgate.GetComponent<GateController>().n_idGate == 1)
                                        {
                                            bottom = 1;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                left = 0;
                                right = 0;
                                bottom = 0;
                            }
                            lstRooms = RandomSalles(left, top, right, bottom);
                            rand = Random.Range(0, lstRooms.Count);
                            room = lstRooms[rand];
                            GO_room = Instantiate(room, new Vector3(center.x, center.y, center.z - MGR_RoomManager.Instance.n_ecartRoom), room.transform.rotation);
                            MGR_RoomManager.Instance.arrGO_rooms[(int)vec_idRoom.x, (int)vec_idRoom.y - 1] = GO_room;
                            GO_room.GetComponent<RoomController>().vec_idRoom.x = vec_idRoom.x;
                            GO_room.GetComponent<RoomController>().vec_idRoom.y = vec_idRoom.y - 1;
                        }


                        break;
                    default:
                        break;
                }
            }
            GO.GetComponent<GateController>().SetTeleport();


        }
        spawned = true;
        foreach (GameObject Spawners in arrGO_spawners)
        {
            Spawners.GetComponent<SpawnerController>().SetEnnemies();
        }
        if (gameObject.name != "StartRoom")
        {
            gameObject.SetActive(false);
        }
        else
        {
            SpawnEnnemies();
        }
    }



    // 0 -> PAS PORTE 1-> PORTE 2-> OSEF
    public List<GameObject> RandomSalles(int left, int top, int right, int bottom)
    {

        List<GameObject> roomList = new List<GameObject>();

        //Debug.Log("l : " + left + " t : " + top + " r : " + right + " b : " + bottom);

        foreach (GameObject GOroom in MGR_RoomManager.Instance.lstGO_rooms)
        {
            bool l = false;
            bool t = false;
            bool r = false;
            bool b = false;


            foreach (GameObject GOgate in GOroom.GetComponent<RoomController>().arrGO_gates)
            {
                switch (GOgate.GetComponent<GateController>().n_idGate)
                {
                    case 0:
                        l = true;
                        break;
                    case 1:
                        t = true;
                        break;
                    case 2:
                        r = true;
                        break;
                    case 3:
                        b = true;
                        break;
                }


            }

            if (left == 0 && l || left == 1 && !l)
            {
                continue;
            }

            if (top == 0 && t || top == 1 && !t)
            {
                continue;
            }
            if (right == 0 && r || right == 1 && !r)
            {
                continue;
            }
            if (bottom == 0 && b || bottom == 1 && !b)
            {
                continue;
            }

            roomList.Add(GOroom);

        }
        /*foreach(GameObject GO in roomList)
        { 
        Debug.Log("GAMEOBJECT NAME"+GO.name);
        }*/
        return roomList;

    }
}
