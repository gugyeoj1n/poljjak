using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;

public class ShopManager : MonoBehaviour
{
    private class Bundle
    {
        public string name;
        public Theme theme;
        public Character[] characters;
        public int price;
        public int discountedPrice;
    }

    private class Theme
    {
        
    }

    private class Character
    {
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        GetShopData( );
    }

    async void GetShopData( )
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        CollectionReference colRef = db.Collection( "Shop" );
        QuerySnapshot snapshot = await colRef.GetSnapshotAsync( );

        foreach(DocumentSnapshot doc in snapshot.Documents)
        {
            Dictionary<string, object> item = doc.ToDictionary( );
            Debug.Log( item["s"] as string );
        }
    }
}
