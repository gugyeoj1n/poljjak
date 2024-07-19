using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    public GameObject prefab;
    public Transform groundCenter;
    public int gridSize = 10;
    public float spacing = 1.1f;
    public int maxNumber = 5;

    void Start( )
    {
        SpawnGround(  );
    }

    public void SpawnGround( )
    {
        Vector3 gridCenter = new Vector3( ( gridSize - 1 ) * spacing / 2.0f, 0, ( gridSize - 1 ) * spacing / 2.0f );
        
        for( int x = 0; x < gridSize; x++ )
        {
            for( int z = 0; z < gridSize; z++ )
            {
                if(Random.value <= 0.05f)
                    continue;
                
                Vector3 spawnPosition = new Vector3( x * spacing, 0, z * spacing ) - gridCenter;
                GameObject instance = Instantiate( prefab, spawnPosition, Quaternion.identity );
                instance.GetComponent<NumberCube>(  ).SetNumber( maxNumber );
                instance.transform.SetParent( groundCenter );
            }
        }
    }
}
