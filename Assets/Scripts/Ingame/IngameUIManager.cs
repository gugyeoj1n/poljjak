using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.Networking;

public class IngameUIManager : MonoBehaviour
{
    [Header( "Input Buttons" )]
    public Button leftUpButton;
    public Button leftDownButton;
    public Button rightUpButton;
    public Button rightDownBUtton;

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

    public static IngameUIManager instance;

    void Awake( )
    {
        instance = this;
    }
    
    void Start()
    {
        InitButtonInput(  );
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
        
        rightDownBUtton.onClick.AddListener( ( ) =>
        {
            if( !PlayerManager.instance.isMoving )
                PlayerManager.instance.Move( PlayerManager.Direction.RightDown, GameManager.instance.currentNumber, GroundSpawner.instance.spacing );
        } );
    }

    public void SetOverPanel( )
    {
        overPanel.SetActive( true );
        var seq = DOTween.Sequence( );
        seq.AppendInterval( 1f );
        seq.Append( overText.transform.DOMove( overText.transform.position + Vector3.up * 100f, 1f ) );
        seq.Append( overScoreText.DOText( string.Format( "{0}점", GameManager.instance.score ), 1f ) );
        seq.Join( homeButtonSprites[0].DOFade( 1f, 0.6f ) );
        seq.Join( homeButtonSprites[1].DOFade( 1f, 0.6f ) );
        seq.Join( restartButtonSprites[0].DOFade( 1f, 0.6f ) );
        seq.Join( restartButtonSprites[1].DOFade( 1f, 0.6f ) );
        seq.Join( homeButtonTransform.DOMove( homeButtonTransform.position + Vector3.up * 20f, 0.4f ) );
        seq.Join( restartButtonTransform.DOMove( restartButtonTransform.position + Vector3.up * 20f, 0.4f ) );
        seq.Play( );
    }
}
