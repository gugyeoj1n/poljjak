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

    public static IngameUIManager instance;

    void Awake( )
    {
        instance = this;
    }
    
    void Start()
    {
        InitButtonInput(  );
    }

    public void SetText( )
    {
        scoreText.text = GameManager.instance.score.ToString();
        currentNumberText.text = GameManager.instance.currentNumber.ToString( );
    }

    private void InitButtonInput( )
    {
        leftUpButton.onClick.AddListener( ( ) =>
        {
            PlayerManager.instance.Move( PlayerManager.Direction.LeftUp, GameManager.instance.currentNumber, GroundSpawner.instance.spacing );
        } );
        
        leftDownButton.onClick.AddListener( ( ) =>
        {
            PlayerManager.instance.Move( PlayerManager.Direction.LeftDown, GameManager.instance.currentNumber, GroundSpawner.instance.spacing );
        } );
        
        rightUpButton.onClick.AddListener( ( ) =>
        {
            PlayerManager.instance.Move( PlayerManager.Direction.RightUp, GameManager.instance.currentNumber, GroundSpawner.instance.spacing );
        } );
        
        rightDownBUtton.onClick.AddListener( ( ) =>
        {
            PlayerManager.instance.Move( PlayerManager.Direction.RightDown, GameManager.instance.currentNumber, GroundSpawner.instance.spacing );
        } );
    }
}
