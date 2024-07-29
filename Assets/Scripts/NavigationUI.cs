using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigationUI : MonoBehaviour
{
    void Start()
    {
        RectTransform rect = GetComponent<RectTransform>( );
        float margin = Screen.width * 0.05f;
        rect.offsetMin = new Vector2( margin, rect.offsetMin.y );
        rect.offsetMax = new Vector2( -margin, rect.offsetMax.y );

        float cellWidth = rect.rect.width / 6f;
        Debug.Log(cellWidth + " / " + rect.rect.width );
        foreach(RectTransform child in rect)
        {
            child.sizeDelta = new Vector2( cellWidth, cellWidth );
        }
    }
}
