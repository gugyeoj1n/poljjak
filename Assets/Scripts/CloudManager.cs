using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour
{
    public Transform cloudStart;
    public List<Cloud> clouds;
    public static CloudManager instance;

    void Awake( )
    {
        instance = this;
        InitClouds( );
    }

    private void InitClouds( )
    {
        clouds = new List<Cloud>( );
        Transform instanceTransform = transform.parent.GetChild( 2 );
        foreach(Transform t in instanceTransform)
            clouds.Add( t.GetComponent<Cloud>(  ) );
    }
    
    private void OnTriggerExit( Collider other )
    {
        if(other.CompareTag( "Cloud" ))
        {
            other.transform.position = new Vector3(other.transform.position.x, cloudStart.position.y, cloudStart.position.z);
        }
    }

    public void DestroyAllClouds( )
    {
        foreach(Cloud c in clouds)
            c.DestroyCloud(  );
    }
}
