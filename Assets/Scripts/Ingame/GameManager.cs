using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int score = 0;
    public int currentNumber;

    private void Awake( )
    {
        instance = this;
    }

    void Start()
    {
        GroundSpawner.instance.SpawnGround(  );
    }

}