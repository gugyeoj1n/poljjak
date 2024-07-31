using System;
using UnityEngine;
using System.Collections.Generic;
using System.Resources;
using DG.Tweening;
using Random = UnityEngine.Random;

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
    public Queue<GameObject> objectPool;
    public List<Vector3> spawnedGround;
    public List<Vector3> onlyGround;

    void Awake( )
    {
        instance = this;
        spawnedGround = new List<Vector3>( );
        onlyGround = new List<Vector3>( );
        objectPool = new Queue<GameObject>( );
    }

    public void SpawnGround( )
    {
        StartCoroutine( PlayerManager.instance.InitPlayerInput( ) );
        
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

        foreach (Vector3 position in positions)
        {
            spawnedGround.Add( FixVector( position ) + Vector3.up * 3f );
            SpawnObject( position );
        }
    }

    private void SpawnObject( Vector3 pos )
    {
        float holeProbability = holeRatio / 100f;
        GameObject groundInstance = null;
        if(Random.value <= holeProbability)
        {
            if(pos != Vector3.down * 3f)
                groundInstance = AnimatedInstantiate( holePrefab, pos + Vector3.up / 2f, Quaternion.identity );
            else
            {
                onlyGround.Add( pos );
                groundInstance = AnimatedInstantiate( prefab, pos, Quaternion.identity );
                groundInstance.GetComponent<NumberCube>(  ).SetNumber( maxNumber );
            }
        }
        else
        {
            onlyGround.Add( pos );
            groundInstance = AnimatedInstantiate( prefab, pos, Quaternion.identity );
            groundInstance.GetComponent<NumberCube>(  ).SetNumber( maxNumber );
        }
            
        groundInstance.transform.SetParent( groundCenter );
    }

    private GameObject AnimatedInstantiate( GameObject prefab, Vector3 position, Quaternion rotation )
    {
        GameObject instance;
        
        if(objectPool.Count == 0)
        {
            instance = Instantiate( prefab, position, rotation );
        }
        else
        {
            instance = objectPool.Dequeue( );
            instance.transform.position = position;
            instance.SetActive( true );
        }
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

    public void SpawnHole( Vector3 pos )
    {
        GameObject holeInstance = Instantiate( holePrefab, pos, Quaternion.identity );
        holeInstance.transform.SetParent( groundCenter );
    }
    
    public void CheckEmptyGround()
    {
        if (GameManager.instance.score == 0) return;    
        
        Vector3 prevPos = PlayerManager.instance.transform.position;
        Vector3 centerPos = new Vector3( prevPos.x, 0, prevPos.z );
        
        for (int x = -3; x <= 3; x++)
        {
            for (int z = -3; z <= 3; z++)
            {
                Vector3 checkPos = FixVector( new Vector3(centerPos.x + x * spacing, centerPos.y, centerPos.z + z * spacing) );
                if(!spawnedGround.Contains( checkPos ))
                {
                    SpawnObject( checkPos - Vector3.up * 3f );
                    spawnedGround.Add( checkPos );
                }
            }
        }
    }

    public Vector3 GetRandomGround( )
    {
        int randIdx = Random.Range( 0, onlyGround.Count );
        return onlyGround[randIdx] + Vector3.up * 4.5f;
    }

    private Vector3 FixVector( Vector3 prev )
    {
        float adjustedX = Mathf.Round(prev.x / spacing) * spacing;
        float adjustedZ = Mathf.Round(prev.z / spacing) * spacing;
        return new Vector3(adjustedX, prev.y, adjustedZ);
    }
}
