using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

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
        for( int i = 0; i < distance; i++ )
        {
            transform.position += direction;
            yield return new WaitForSeconds( 1f );
        }
    }
}
