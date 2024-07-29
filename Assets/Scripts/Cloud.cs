using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Cloud : MonoBehaviour
{
    public float speed;
    
    void Start()
    {
        speed = Random.Range( 0.1f, 0.5f );
    }

    void Update( )
    {
        transform.position += Vector3.forward * (speed * Time.deltaTime);
    }

    public void DestroyCloud( )
    {
        Material material = GetComponent<MeshRenderer>( ).material;
        material.DOFade( 0f, Random.Range( 1.5f, 2f ) ).OnComplete( ( ) =>
        {
            Destroy( gameObject );
        } );
    }
}
