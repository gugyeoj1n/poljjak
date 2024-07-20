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
        IngameUIManager.instance.SetOverPanel(  );
    }

    public void RestartGame( )
    {
        SceneManager.LoadScene( "Classic" );
    }
}