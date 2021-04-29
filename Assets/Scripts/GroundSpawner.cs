using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject groundTile;
    Vector3 nextSpawnPoint;
    bool dblJumpApplied;
    bool flipper = true;
    GameObject previousTile = null;

    public void SpawnTile(bool spawnItems, int count = 1) 
    {
        if (dblJumpApplied) flipper = !flipper;
        if (!flipper) count = 2;
        while (count > 0)
        {
            int random = Random.Range(0, 20);

            GameObject temp = Instantiate(groundTile, nextSpawnPoint, Quaternion.identity);
            if (GameManager.inst.score <= 1000) random = 10;
            if (previousTile != null && !previousTile.active) random = 10;
            if (random < 1 )
            {
                temp.SetActive(false);
                spawnItems = false;
            }

            //Choosing rand point for tile
            int tileSpawnIndex = 1;
            if (spawnItems)
            {
                tileSpawnIndex = Random.Range(1, 4);

                if (random >= 1)
                {
                    temp.GetComponent<GroundTile>().SpawnObstacle(tileSpawnIndex);
                    temp.GetComponent<GroundTile>().SpawnPowerUps(temp);
                }
            }
            nextSpawnPoint = temp.transform.GetChild(tileSpawnIndex).transform.position;
            previousTile = temp;
            count--;
        }
    }

    private void Start()
    {
        dblJumpApplied = bool.Parse(PlayerPrefs.GetString("DoubleJump" + "Applied", "False"));
        for (int i = 0; i < 6; i++) {
            if (i < 1)
            {
                SpawnTile(false);
            }
        	else SpawnTile(true);
        }
    }

}
