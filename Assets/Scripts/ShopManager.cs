using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;

public class ShopManager : MonoBehaviour
{
    private class Bundle
    {
        public string title;
        public int type;
        public bool isCash;
        public Theme[] themes;
        public Character[] characters;
        public int price;
        public int discount;
        public string id;
    }

    private class Theme
    {
        public string title;
        public int type;
        public bool isCash;
        public int price;
        public int discount;
        public string id;
    }

    private class Character
    {
        public string title;
        public int type;
        public bool isCash;
        public int price;
        public int discount;
        public string id;
    }
    
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
