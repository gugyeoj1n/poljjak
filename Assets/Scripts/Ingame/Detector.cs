using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    private void OnCollisionEnter( Collision other )
    {
        if(other.transform.CompareTag( "NumberCube" ))
        {
            GameManager.instance.currentNumber = other.transform.GetComponent<NumberCube>( ).number;
            IngameUIManager.instance.SetNumberText(  );
        }
    }
}
