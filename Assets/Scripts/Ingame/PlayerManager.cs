using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public GameObject character;
    public bool isMoving = true;
    public Animator animator;

    public enum Direction
    {
        LeftUp,
        LeftDown,
        RightUp,
        RightDown,
    }

    void Awake( )
    {
        instance = this;
    }

    void Start( )
    {
        character = transform.GetChild( 0 ).GetChild( 0 ).gameObject;
        animator = character.GetComponent<Animator>( );
    }

    public IEnumerator InitPlayerInput( )
    {
        yield return new WaitForSeconds( 2.1f );
        isMoving = false;
    }
    
    public void Move( Direction direction, int distance, float space )
    {
        Vector3 directionVector;
        
        switch( direction )
        {
            case Direction.LeftUp :
                directionVector = Vector3.left * space;
                break;
            case Direction.LeftDown :
                directionVector = Vector3.back * space;
                break;
            case Direction.RightUp :
                directionVector = Vector3.forward * space;
                break;
            case Direction.RightDown :
                directionVector = Vector3.right * space;
                break;
            default :
                directionVector = Vector3.zero;
                break;
        }

        StartCoroutine( MoveTransform( directionVector, distance ) );
    }

    IEnumerator MoveTransform( Vector3 direction, int distance )
    {
        Detector.instance.currentCube.StartDestroyCube(  );
        
        isMoving = true;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + direction * distance;
        float travelTime = 1.0f;
        float height = 3.0f;

        character.transform.eulerAngles = GetRotationVector( direction );
        
        animator.SetTrigger( "Jump" );
        animator.speed = animator.GetCurrentAnimatorStateInfo( 0 ).length / travelTime;

        for( float t = 0; t < 1; t += Time.deltaTime / travelTime )
        {
            Vector3 currentPos = Vector3.Lerp( startPosition, endPosition, t );
            currentPos.y = startPosition.y + height * Mathf.Sin( Mathf.PI * t );
            transform.position = currentPos;
            yield return null;
        }

        transform.position = endPosition;
        animator.speed = 1f;
        isMoving = false;
    }

    private Vector3 GetRotationVector( Vector3 direction )
    {
        if(direction.x != 0f)
        {
            if(direction.x < 0f)
                return Vector3.up * -90f;
            return Vector3.up * 90f;
        }

        if(direction.z < 0f)
            return Vector3.up * 180f;
        return Vector3.up;
    }
}
