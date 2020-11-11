using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    //serialized fields
    [SerializeField] List<GameObject> enemies;  //enemies present in current level. Add them in reverse order w.r.t.
                                                //their encounter with the player
    [SerializeField] bool level2 = false;
    [SerializeField] float destroyEnemyAfterTime = 3f;

    int numEnemies;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        numEnemies = enemies.Count;              //total number of enemies present in current level
        player = FindObjectOfType<Player>();
    }

    //Delete last enemy in the list from the list and the scene
    public void DecrementNumEnemies()
    {
        
        Destroy(enemies[enemies.Count - 1], destroyEnemyAfterTime);  //destroy enemy from the scent
        enemies.RemoveAt(enemies.Count - 1);                         //remove enmy from list
        numEnemies--;                                                //decrease total number of enemies alive

        //for level 1 move from certain waypoint to another
        if (!level2)
        {
            if (numEnemies == 1)
                player.SetMove(0, 2);
            else
                player.SetMove(2, 9);
        }

        //for level 2 move from differnet waypoint to another
        else
        {
            if (numEnemies == 2)
                player.SetMove(0, 2);
            else if (numEnemies == 1)
                player.SetMove(2, 8);
            else
                player.SetMove(8, 13);
        }
            
            
    }

    //get total enemies currently alive
    public int GetNumEnemies()
    {
        return numEnemies;
    }
}
