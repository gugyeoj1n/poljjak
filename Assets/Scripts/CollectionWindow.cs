using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionWindow : MonoBehaviour
{
    public RectTransform renderImage;
    
    void Start()
    {
        RectTransform rect = GetComponent<RectTransform>( );
        RectTransform upRect = transform.GetChild( 0 ).GetComponent<RectTransform>( );
        RectTransform downRect = transform.GetChild( 1 ).GetComponent<RectTransform>( );
        
        float margin = 50f;

        renderImage.sizeDelta = new Vector2( rect.rect.width * 0.4f, rect.rect.width * 0.4f );
        
        upRect.offsetMin = new Vector2(upRect.offsetMin.x + margin, rect.rect.height * 0.4f + margin);
        upRect.offsetMax = new Vector2(upRect.offsetMax.x - margin, upRect.offsetMax.y - margin);

        downRect.offsetMin = new Vector2(downRect.offsetMin.x + margin, downRect.offsetMin.y + margin);
        downRect.offsetMax = new Vector2(downRect.offsetMax.x - margin, -rect.rect.height * 0.6f - margin);
    }
}