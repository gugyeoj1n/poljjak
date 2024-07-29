using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.Networking;

public class UIManager : MonoBehaviour
{
    [Header( "Lobby Objects" )]
    public Transform topBar;
    public Transform navigationBar;
    
    [Header( "Home UI Objects" )]
    public GameObject homeWindow;
    
    [Header( "Input Buttons" )]
    public Button leftUpButton;
    public Button leftDownButton;
    public Button rightUpButton;
    public Button rightDownButton;

    [Header( "Texts" )]
    public TMP_Text scoreText;
    public TMP_Text currentNumberText;

    [Header( "ETC" )]
    public Button stopButton;
    
    [Header( "Game Over" )]
    public GameObject overPanel;
    public GameObject overText;
    public TMP_Text overScoreText;
    public Image[] homeButtonSprites;
    public Image[] restartButtonSprites;
    public Transform homeButtonTransform;
    public Transform restartButtonTransform;

    public static UIManager instance;
    
    // TEST
    public GameObject startButton;

    void Awake( )
    {
        instance = this;
    }
    
    void Start()
    {
        InitButtonInput( );
        StartButtonAnimation( );
    }

    private void StartButtonAnimation( )
    {
        startButton.transform.DOMoveY( startButton.transform.position.y + 30f, 2f )
            .SetLoops( -1, LoopType.Yoyo )
            .SetEase( Ease.InOutSine );
    }

    public void SetNumberText( )
    {
        currentNumberText.text = GameManager.instance.currentNumber.ToString( );
    }

    public void SetScoreText( )
    {
        scoreText.text = GameManager.instance.score + "점";
        scoreText.DOScale( 1.15f, 0.5f ).OnComplete( ( ) =>
        {
            scoreText.DOScale( 1f, 0.2f );
        } );
    }

    private void InitButtonInput( )
    {
        leftUpButton.onClick.AddListener( ( ) =>
        {
            if( !PlayerManager.instance.isMoving )
                PlayerManager.instance.Move( PlayerManager.Direction.LeftUp, GameManager.instance.currentNumber, GroundSpawner.instance.spacing );
        } );
        
        leftDownButton.onClick.AddListener( ( ) =>
        {
            if( !PlayerManager.instance.isMoving )
                PlayerManager.instance.Move( PlayerManager.Direction.LeftDown, GameManager.instance.currentNumber, GroundSpawner.instance.spacing );
        } );
        
        rightUpButton.onClick.AddListener( ( ) =>
        {
            if( !PlayerManager.instance.isMoving )
                PlayerManager.instance.Move( PlayerManager.Direction.RightUp, GameManager.instance.currentNumber, GroundSpawner.instance.spacing );
        } );
        
        rightDownButton.onClick.AddListener( ( ) =>
        {
            if( !PlayerManager.instance.isMoving )
                PlayerManager.instance.Move( PlayerManager.Direction.RightDown, GameManager.instance.currentNumber, GroundSpawner.instance.spacing );
        } );
    }

    public void StartAnimation( )
    {
        homeWindow.SetActive( false );
        
        var seq = DOTween.Sequence( );
        seq.Append( topBar.DOMove( topBar.position + Vector3.up * 150f, 1f ) );
        seq.Join( navigationBar.DOMove( navigationBar.position - Vector3.up * 250f, 1f ) );
        
        var seq2 = DOTween.Sequence( );
        seq2.Append( leftDownButton.transform.DOMove( leftDownButton.transform.position + Vector3.up * 300f, 2f ) );
        seq2.Join( rightDownButton.transform.DOMove( rightDownButton.transform.position + Vector3.up * 300f, 2f ) );
        seq2.Join( leftUpButton.transform.DOMove( leftUpButton.transform.position + Vector3.up * 500f, 2f ) );
        seq2.Join( rightUpButton.transform.DOMove( rightUpButton.transform.position + Vector3.up * 500f, 2f ) );
        seq2.Join( scoreText.transform.DOMove( scoreText.transform.position - Vector3.up * 125f, 2f ) );
        seq2.Join( currentNumberText.transform.DOMove( currentNumberText.transform.position - Vector3.up * 225f, 2f ) );
        seq2.Join( stopButton.transform.DOMove( stopButton.transform.position - Vector3.right * 100f, 2f ) );

        seq.Play( );
        seq2.Play( );
    }

    public void SetOverPanel( )
    {
        overPanel.SetActive( true );
        var seq = DOTween.Sequence( );
        seq.AppendInterval( 1f );
        seq.Append( overText.transform.DOMove( overText.transform.position + Vector3.up * 100f, 1f ) );
        seq.Append( overScoreText.DOText( string.Format( "{0}점을 기록했어요.", GameManager.instance.score ), 1f ) );
        seq.Join( homeButtonSprites[0].DOFade( 1f, 0.6f ) );
        seq.Join( homeButtonSprites[1].DOFade( 1f, 0.6f ) );
        seq.Join( restartButtonSprites[0].DOFade( 1f, 0.6f ) );
        seq.Join( restartButtonSprites[1].DOFade( 1f, 0.6f ) );
        seq.Join( homeButtonTransform.DOMove( homeButtonTransform.position + Vector3.up * 20f, 0.4f ) );
        seq.Join( restartButtonTransform.DOMove( restartButtonTransform.position + Vector3.up * 20f, 0.4f ) );
        seq.Play( );
    }
}
