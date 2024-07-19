using UnityEngine;
using System.Collections.Generic;

public class GroundSpawner : MonoBehaviour
{
    public GameObject prefab;
    public Transform groundCenter;
    public int gridSize;
    public float spacing;
    public int maxNumber;
    public int holeRatio;

    void Start( )
    {
        SpawnGround(  );
    }

    public void SpawnGround( )
    {
        Vector3 gridCenter = new Vector3( ( gridSize - 1 ) * spacing / 2.0f, 0, ( gridSize - 1 ) * spacing / 2.0f );
        List<Vector3> positions = new List<Vector3>();

        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                Vector3 spawnPosition = new Vector3(x * spacing, 0, z * spacing) - gridCenter;
                positions.Add(spawnPosition);
            }
        }
        
        Shuffle(positions);

        float holeProbability = holeRatio / 100f;
        foreach (Vector3 position in positions)
        {
            if(Random.value <= holeProbability)
                if(position != Vector3.zero)
                    continue;
            
            GameObject instance = Instantiate( prefab, position, Quaternion.identity );
            instance.GetComponent<NumberCube>(  ).SetNumber( maxNumber );
            instance.transform.SetParent( groundCenter );
        }
    }
    
    private void Shuffle( List<Vector3> list )
    {
        for ( int i = 0; i < list.Count; i++ )
        {
            Vector3 temp = list[i];
            int randomIndex = Random.Range( i, list.Count );
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
