using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject groundTile;
    Vector3 nextSpawnPoint;

    public void SpawnTile(bool spawnItems) 
    {
    	GameObject temp = Instantiate(groundTile, nextSpawnPoint, Quaternion.identity);
        //Choosing rand point for tile
        int tileSpawnIndex = 1;
        if (spawnItems)
        {
            tileSpawnIndex = Random.Range(1, 4);
            temp.GetComponent<GroundTile>().SpawnObstacle(tileSpawnIndex);
            temp.GetComponent<GroundTile>().SpawnPowerUps(temp);
        }
        nextSpawnPoint = temp.transform.GetChild(tileSpawnIndex).transform.position;
    }

    private void Start()
    {
        for (int i = 0; i < 6; i++) {
            if (i < 1)
            {
                SpawnTile(false);
            }
        	else SpawnTile(true);
        }
    }

}
