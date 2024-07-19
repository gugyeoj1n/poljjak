using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
}
