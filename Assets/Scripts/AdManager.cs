using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour
{
    public string _adUnitId;
    private RewardedInterstitialAd _rewardedInterstitialAd;
    
    void Awake( )
    {
        MobileAds.Initialize( ( InitializationStatus status ) =>
        {
            
        } );
    }

    void Start( )
    {
        LoadRewardedInterstitialAd(  );
    }
    
    public void LoadRewardedInterstitialAd()
    {
        if (_rewardedInterstitialAd != null)
        {
            _rewardedInterstitialAd.Destroy();
            _rewardedInterstitialAd = null;
        }

        Debug.Log("Loading the rewarded interstitial ad.");

        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        RewardedInterstitialAd.Load(_adUnitId, adRequest,
            (RewardedInterstitialAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("rewarded interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                _rewardedInterstitialAd = ad;
            });
    }
    
    public void ShowRewardedInterstitialAd()
    {
        const string rewardMsg =
            "Rewarded interstitial ad rewarded the user. Type: {0}, amount: {1}.";

        if (_rewardedInterstitialAd != null && _rewardedInterstitialAd.CanShowAd())
        {
            _rewardedInterstitialAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                Debug.Log(string.Format(rewardMsg, reward.Type, reward.Amount));
                GameManager.instance.Revive(  );
            });
        }
    }
}