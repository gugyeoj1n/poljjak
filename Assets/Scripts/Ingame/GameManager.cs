using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int score = 0;
    public int currentNumber;
    public CinemachineVirtualCamera cam;
    public bool isMovable = false;

    private void Awake( )
    {
        instance = this;
    }

    void Start()
    {
        GroundSpawner.instance.SpawnGround(  );
    }

    public void IncreaseScore(  )
    {
        if(!isMovable)
        {
            isMovable = true;
            return;
        }
        score += currentNumber;
        IngameUIManager.instance.SetScoreText(  );
    }

    public void GameOver( )
    {
        cam.Follow = null;
        cam.Follow = null;
        StartCoroutine( DestroyPlayer( ) );
    }

    private IEnumerator DestroyPlayer( )
    {
        yield return new WaitForSeconds( 3f );
        Destroy( PlayerManager.instance.gameObject );
    }
}