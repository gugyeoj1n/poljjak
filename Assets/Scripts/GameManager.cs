using System.Collections;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int score = 0;
    public int currentNumber;
    public CinemachineVirtualCamera cam;
    public CinemachineVirtualCamera gameOvercam;
    public CinemachineVirtualCamera lobbyCam;
    public bool isMovable = false;
    public bool isOvered = false;

    private void Awake( )
    {
        instance = this;
    }

    void Start()
    {
        GroundSpawner.instance.SpawnGround(  );
    }

    public void StartGame( )
    {
        UIManager.instance.startButton.SetActive( false );
        UIManager.instance.StartAnimation(  );
        lobbyCam.gameObject.SetActive( false );
        CloudManager.instance.DestroyAllClouds(  );
        PlayerManager.instance.GetComponent<Rigidbody>( ).useGravity = true;
    }
    
    public void IncreaseScore(  )
    {
        score += currentNumber;
        UIManager.instance.SetScoreText(  );
    }

    public void GameOver( )
    {
        if(isOvered) return;
        if(!isOvered) isOvered = true;
        cam.Follow = null;
        cam.Follow = null;

        GameOverAnimation( );
    }

    private void GameOverAnimation( )
    {
        PlayerManager.instance.animator.Play( "Fly" );
        PlayerManager.instance.animator.Play( "Eyes_Cry" );
        PlayerManager.instance.character.transform.LookAt( gameOvercam.transform );
        PlayerManager.instance.GetComponent<Rigidbody>( ).useGravity = false;
        PlayerManager.instance.GetComponent<Rigidbody>( ).constraints = RigidbodyConstraints.FreezePositionY;
        gameOvercam.Priority = 11;
        StartCoroutine( DestroyPlayer( ) );
    }
    
    

    private IEnumerator DestroyPlayer( )
    {
        float blendTime = Camera.main.GetComponent<CinemachineBrain>( ).m_DefaultBlend.BlendTime;
        yield return new WaitForSeconds( blendTime + 1f );
        
        //PlayerManager.instance.transform.LookAt( gameOvercam.transform );
        
        PlayerManager.instance.GetComponent<Rigidbody>( ).useGravity = true;
        PlayerManager.instance.GetComponent<Rigidbody>( ).constraints = RigidbodyConstraints.None;
        gameOvercam.Follow = null;
        gameOvercam.LookAt = null;
        yield return new WaitForSeconds( 1.5f );
        Destroy( PlayerManager.instance.gameObject );
        UIManager.instance.SetOverPanel(  );
    }

    public void RestartGame( )
    {
        SceneManager.LoadScene( "Classic" );
    }
}