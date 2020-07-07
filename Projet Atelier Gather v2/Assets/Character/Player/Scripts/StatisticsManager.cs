using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticsManagerScript : MonoBehaviour
{
    public int nb_rooms_completed;

    public int nb_ennemies_killed;

    // Start is called before the first frame update
    void Start()
    {
        nb_rooms_completed = 0;
        nb_ennemies_killed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void roomCompleted()
    {
        nb_rooms_completed++;
    }

    public void  ennemyKilled()
    {
        nb_ennemies_killed++;
    }
}
