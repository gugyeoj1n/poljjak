using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        
    }
}
