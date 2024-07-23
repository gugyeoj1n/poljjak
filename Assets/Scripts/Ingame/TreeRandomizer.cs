using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeRandomizer : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive( Random.value < 0.5f );
    }
}
