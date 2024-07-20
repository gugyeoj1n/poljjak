using UnityEngine;
using System.Collections.Generic;
using System.Resources;
using DG.Tweening;

public class GroundSpawner : MonoBehaviour
{
    public static GroundSpawner instance;
    public GameObject prefab;
    public GameObject holePrefab;
    public Transform groundCenter;
    public int gridSize;
    public float spacing;
    public int maxNumber;
    public int holeRatio;

    void Awake( )
    {
        instance = this;
    }

    public void SpawnGround( )
    {
        Vector3 gridCenter = new Vector3( ( gridSize - 1 ) * spacing / 2.0f, 0, ( gridSize - 1 ) * spacing / 2.0f );
        List<Vector3> positions = new List<Vector3>();

        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                Vector3 spawnPosition = new Vector3(x * spacing, -3f, z * spacing) - gridCenter;
                positions.Add(spawnPosition);
            }
        }
        
        Shuffle(positions);

        float holeProbability = holeRatio / 100f;
        foreach (Vector3 position in positions)
        {
            GameObject groundInstance = null;
            if(Random.value <= holeProbability)
            {
                if(position != Vector3.down * 3f)
                    groundInstance = AnimatedInstantiate( holePrefab, position + Vector3.up / 2f, Quaternion.identity );
                else
                {
                    groundInstance = AnimatedInstantiate( prefab, position, Quaternion.identity );
                    groundInstance.GetComponent<NumberCube>(  ).SetNumber( maxNumber );
                }
            }
            else
            {
                groundInstance = AnimatedInstantiate( prefab, position, Quaternion.identity );
                groundInstance.GetComponent<NumberCube>(  ).SetNumber( maxNumber );
            }
            
            groundInstance.transform.SetParent( groundCenter );
        }
    }

    private GameObject AnimatedInstantiate( GameObject prefab, Vector3 position, Quaternion rotation )
    {
        GameObject instance = Instantiate( prefab, position, rotation );
        float animateTime = Random.Range( 1f, 2f );
        instance.transform.DOMove( instance.transform.position + Vector3.up * 3f, animateTime );
        return instance;
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
