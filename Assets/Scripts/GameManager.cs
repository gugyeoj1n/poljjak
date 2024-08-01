using System.Collections;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using DG.Tweening;

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
    public bool isRevived = false;

    public Tween timerTween;
    public Tween colorTween;
    
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

    public void StartTimer( )
    {
        if(timerTween != null)
            timerTween.Kill(  );
        
        UIManager.instance.timerSlider.value = 1f;
        UIManager.instance.sliderImage.color = UIManager.instance.startColor;
        
        timerTween = UIManager.instance.timerSlider.DOValue( 0f, 5f ).OnComplete( GameOver );
        colorTween = UIManager.instance.sliderImage.DOColor(UIManager.instance.endColor, 5f);
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
        PlayerManager.instance.GetComponent<Rigidbody>( ).useGravity = false;
        PlayerManager.instance.GetComponent<Rigidbody>( ).constraints = RigidbodyConstraints.FreezePositionY;
        // Destroy( PlayerManager.instance.gameObject );
        UIManager.instance.SetOverPanel(  );
    }

    public void Revive( )
    {
        isRevived = true;
        isOvered = false;
        PlayerManager.instance.GetComponent<Transform>( ).position = GroundSpawner.instance.GetRandomGround( );
        PlayerManager.instance.GetComponent<Rigidbody>( ).constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        PlayerManager.instance.GetComponent<Rigidbody>( ).velocity = Vector3.zero;
        PlayerManager.instance.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        PlayerManager.instance.animator.Play( "Idle_A" );
        PlayerManager.instance.animator.Play( "Eyes_Blink" );
        PlayerManager.instance.character.transform.rotation = Quaternion.identity;
        UIManager.instance.ResetOverPanel( );
        gameOvercam.Follow = PlayerManager.instance.GetComponent<Transform>(  );
        gameOvercam.LookAt = PlayerManager.instance.GetComponent<Transform>(  );
        cam.Follow = PlayerManager.instance.GetComponent<Transform>( );
        cam.LookAt = PlayerManager.instance.GetComponent<Transform>( );
        gameOvercam.Priority = 1;
        PlayerManager.instance.isMoving = false;
        PlayerManager.instance.GetComponent<Rigidbody>( ).useGravity = true;
    }
    
    public void RestartGame( )
    {
        SceneManager.LoadScene( "Main 1" );
    }
}