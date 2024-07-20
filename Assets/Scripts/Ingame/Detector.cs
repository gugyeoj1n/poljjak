using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public static Detector instance;
    public NumberCube currentCube;

    private void Awake( )
    {
        instance = this;
    }

    private void OnCollisionEnter( Collision other )
    {
        if( other.transform.CompareTag( "NumberCube" ) )
        {
            currentCube = other.transform.GetComponent<NumberCube>(  );
            GameManager.instance.currentNumber = other.transform.GetComponent<NumberCube>( ).number;
            GameManager.instance.IncreaseScore(  );
            IngameUIManager.instance.SetNumberText(  );
        }
    }

    private void OnTriggerEnter( Collider other )
    {
        if(other.transform.CompareTag( "Hole" ))
        {
            PlayerManager.instance.isMoving = true;
            GameManager.instance.GameOver(  );
        }
    }
}
