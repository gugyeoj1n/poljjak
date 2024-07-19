using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public bool isMoving = false;

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
        isMoving = true;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + direction * distance;
        float travelTime = 1.0f;
        float height = 2.0f;

        for( float t = 0; t < 1; t += Time.deltaTime / travelTime )
        {
            Vector3 currentPos = Vector3.Lerp( startPosition, endPosition, t );
            currentPos.y = startPosition.y + height * Mathf.Sin( Mathf.PI * t );
            transform.position = currentPos;
            yield return null;
        }

        transform.position = endPosition;
        isMoving = false;
    }
}
