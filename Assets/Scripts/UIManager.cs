using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Firebase;
using Firebase.Auth;
using UnityEngine.Networking;

public class UIManager : MonoBehaviour
{
    [Header( "Lobby Objects" )]
    public Transform topBar;
    public Transform navigationBar;
    public TMP_Text gem;
    public TMP_Text coin;
    
    [Header( "Home UI Objects" )]
    public GameObject homeWindow;

    [Header( "Navigation Objects" )]
    public Button[] navigationButtons;
    public GameObject[] windows;
    
    [Header( "Input Buttons" )]
    public Button leftUpButton;
    public Button leftDownButton;
    public Button rightUpButton;
    public Button rightDownButton;

    public Slider timerSlider;
    public Image sliderImage;
    public Color startColor;
    public Color endColor;

    [Header( "Texts" )]
    public TMP_Text scoreText;
    public TMP_Text currentNumberText;

    [Header( "ETC" )]
    public Button stopButton;
    
    [Header( "Game Over" )]
    public GameObject overPanel;
    public GameObject overText;
    public TMP_Text overScoreText;
    public Image[] overButtonSprites;
    public TMP_Text[] overTexts;
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
        sliderImage = timerSlider.transform.GetChild( 0 ).GetComponent<Image>( );
        
        InitButtonInput( );
        InitNavigationButtons( );
        StartButtonAnimation( );
    }

    private void StartButtonAnimation( )
    {
        startButton.transform.DOMoveY( startButton.transform.position.y + 30f, 2f )
            .SetLoops( -1, LoopType.Yoyo )
            .SetEase( Ease.InOutSine );
    }

    private void InitNavigationButtons( )
    {
        for(int i = 0; i < navigationButtons.Length; i++)
        {
            int idx = i;
            navigationButtons[idx].onClick.AddListener( ( ) =>
            {
                OnNavigationClicked( idx );
            } );
        }
    }

    private void OnNavigationClicked( int idx )
    {
        foreach(GameObject obj in windows)
            obj.SetActive( false );
        windows[idx].SetActive( true );
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
        seq.Append( topBar.DOMove( topBar.position + Vector3.up * 200f, 1f ) );
        seq.Join( navigationBar.DOMove( navigationBar.position - Vector3.up * 250f, 1f ) );
        
        var seq2 = DOTween.Sequence( );
        seq2.Append( leftDownButton.transform.DOMove( leftDownButton.transform.position + Vector3.up * 300f, 2f ) );
        seq2.Join( rightDownButton.transform.DOMove( rightDownButton.transform.position + Vector3.up * 300f, 2f ) );
        seq2.Join( leftUpButton.transform.DOMove( leftUpButton.transform.position + Vector3.up * 500f, 2f ) );
        seq2.Join( rightUpButton.transform.DOMove( rightUpButton.transform.position + Vector3.up * 500f, 2f ) );
        seq2.Join( scoreText.transform.DOMove( scoreText.transform.position - Vector3.up * 125f, 2f ) );
        seq2.Join( currentNumberText.transform.DOMove( currentNumberText.transform.position - Vector3.up * 225f, 2f ) );
        seq2.Join( stopButton.transform.DOMove( stopButton.transform.position - Vector3.right * 100f, 2f ) );
        seq2.Join( timerSlider.transform.DOMove( timerSlider.transform.position + Vector3.up * 550f, 2f ) );
        seq.Play( );
        seq2.Play( ).OnComplete( GameManager.instance.StartTimer );
    }

    public void SetOverPanel( )
    {
        overPanel.SetActive( true );
        var seq = DOTween.Sequence( );
        seq.AppendInterval( 1f );
        seq.Append( overText.transform.DOMove( overText.transform.position + Vector3.up * 150f, 1f ) );
        seq.Append( overScoreText.DOText( string.Format( "{0}점을 기록했어요.", GameManager.instance.score ), 1f ) );
        seq.Join( overButtonSprites[0].DOFade( 1f, 0.6f ) );
        seq.Join( overButtonSprites[1].DOFade( 1f, 0.6f ) );
        seq.Join( overButtonSprites[2].DOFade( 1f, 0.6f ) );
        seq.Join( overButtonSprites[3].DOFade( 1f, 0.6f ) );
        seq.Join( overTexts[0].DOFade( 1f, 0.6f ) );
        seq.Join( overTexts[1].DOFade( 1f, 0.6f ) );
        if(!GameManager.instance.isRevived)
        {
            seq.Join( homeButtonTransform.DOMove( homeButtonTransform.position + Vector3.up * 20f, 0.4f ) );
            seq.Join( restartButtonTransform.DOMove( restartButtonTransform.position + Vector3.up * 20f, 0.4f ) );
        }
        else
        {
            seq.Join( restartButtonTransform.DOMove( restartButtonTransform.position + Vector3.up * 90f, 0.4f ) );
        }
        seq.Play( );
    }

    public void ResetOverPanel( )
    {
        overPanel.SetActive( false );
        
        overText.transform.position -= Vector3.up * 150f;
        overScoreText.text = "";
        homeButtonTransform.gameObject.SetActive( false );
        overButtonSprites[2].DOFade( 0f, 0.1f );
        overButtonSprites[3].DOFade( 0f, 0.1f );
        overTexts[1].DOFade( 0f, 0.1f );
        restartButtonTransform.transform.position -= Vector3.up * 20f;
    }

    public void SetGemCoinText()
    {
        
    }
}
