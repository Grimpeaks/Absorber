using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{

    public List<GameObject> listGO_ennemies;
    bool spawned;
    bool bossSpawned;

    void Start()
    {
        bossSpawned = false;
    }

    public void SetEnnemies()
    {
        int numberRand = Random.Range(1, 3);
        for (int i = 0; i < numberRand; i++)
        {
            int rand = Random.Range(0, MGR_RoomManager.Instance.lst_ennemies.Count);
            GameObject GOtmp = Instantiate(MGR_RoomManager.Instance.lst_ennemies[rand], new Vector3(transform.position.x, 1.5f, transform.position.z), transform.rotation);
            GOtmp.SetActive(false);
            listGO_ennemies.Add(GOtmp);
        }
    }

    public void SpawnBoss()
    {
        if (!bossSpawned)
        {
            GameObject GOtmp = Instantiate(MGR_RoomManager.Instance.GO_BOSS, new Vector3(transform.position.x, 5, transform.position.z), transform.rotation);
            bossSpawned = true;
        }
    }


}
