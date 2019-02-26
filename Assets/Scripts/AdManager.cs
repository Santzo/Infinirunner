using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
//using GoogleMobileAds.Api;
/*
public class AdManager : MonoBehaviour
{
    private BannerView bannerView;
    private string appID = ""; 
    private string bannerID = ""; // Banner ID goes here!!!
    private InterstitialAd ad;
    private AdRequest request;
    private string adId = "";// Interstitial ID goes here!!!
    public bool loadBanner;
    public bool adShown;
    public bool loaded;
    public bool failedToLoad;

    // Start is called before the first frame update
    public static AdManager am;

    private void Awake()
    {
        if (am== null)
        {
            am = this;
            DontDestroyOnLoad(am);
        }
        else
        {
            Destroy(this);
        }

        MobileAds.Initialize(appID);
    }

   
    void Start()
    {
        ad = new InterstitialAd(adId);
        AdRequest requestAd = new AdRequest.Builder().Build();
        this.ad.LoadAd(requestAd);
        ad.OnAdClosed += Closed;
        ad.OnAdFailedToLoad += AdFail;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RequestBanner(bool hide)
    {
        if (!hide)
        {
            bannerView = new BannerView(bannerID, AdSize.Banner, AdPosition.TopLeft);
            request = new AdRequest.Builder().Build();
            bannerView.LoadAd(request);
            
        }
        else if (hide)
        {
            bannerView.Hide();
        }
    }

    public void RequestAd()
    {
       
        if (ad.IsLoaded())
        {
            ad.Show();
            ad = new InterstitialAd(adId);
            AdRequest requestAd = new AdRequest.Builder().Build();
            ad.LoadAd(requestAd);
            ad.OnAdClosed += Closed;
            ad.OnAdFailedToLoad += AdFail;
        }
        else if (!ad.IsLoaded())
        {
            AdRequest requestAd = new AdRequest.Builder().Build();
            ad.LoadAd(requestAd);
            ad.OnAdClosed += Closed;
            ad.OnAdFailedToLoad += AdFail;
        }
    }

    public void Closed(object sender, EventArgs args)
    {
        adShown = true;
        failedToLoad = false;
        ad.Destroy();
    }
    public void AdFail(object sender, AdFailedToLoadEventArgs args)
    {
        failedToLoad = true;
        adShown = true;
    }



}
*/