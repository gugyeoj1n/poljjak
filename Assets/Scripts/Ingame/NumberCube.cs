using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TreeEditor;

public class NumberCube : MonoBehaviour
{
    public int number;
    private TMP_Text numberText;
    
    void Start()
    {
        
    }

    public void SetNumber( int maxNumber )
    {
        numberText = transform.GetChild( 0 ).GetChild( 0 ).GetComponent<TMP_Text>( );
        
        number = Random.Range( 1, maxNumber + 1 );
        numberText.text = number.ToString( );
    }

    public void StartDestroyCube( )
    {
        StartCoroutine( DestroyCube( ) );
    }

    private IEnumerator DestroyCube( )
    {
        Vector3 prevPos = this.transform.position;
        GroundSpawner.instance.SpawnHole( prevPos );

        Rigidbody rigidbody = GetComponent<Rigidbody>( );
        rigidbody.useGravity = true;
        rigidbody.isKinematic = false;
        rigidbody.constraints = RigidbodyConstraints.None;
        yield return new WaitForSeconds( 2f );
        Destroy( this.gameObject );
        
    }
}
