using System.Collections.Generic;
using UnityEngine;
using AppsFlyerSDK;

public class AppsFlyerInit : MonoBehaviour, IAppsFlyerConversionData {

#if UNITY_IOS || UNITY_ANDROID

    public string appId = string.Empty;
    public string devKey = string.Empty;
    
    void Start()
    {
        DebugLog("Start");
        #if ENABLE_DEBUG_LOG
        AppsFlyer.setIsDebug(true);
        #else 
        AppsFlyer.setIsDebug(false);
        #endif
        DebugLog("Start -> appId = " + appId);
        DebugLog("Start -> devKey = " + devKey);
        DebugLog("Start -> CALL: AppsFlyer.initSDK");
        AppsFlyer.initSDK(devKey, appId, this);
        DebugLog("Start -> CALL: AppsFlyer.startSDK");
        AppsFlyer.startSDK();
        DebugLog("Start -> initialization End");

        DebugLog("Start -> sdkVersion = " + AppsFlyer.getSdkVersion());
        DebugLog("Start -> AppsFlyerId = " + AppsFlyer.getAppsFlyerId());
        DebugLog("Start -> Register a Conversion Data Listener");
        AppsFlyer.getConversionData(this.name);
        DebugLog("Start -> End");
    }
    public void onConversionDataSuccess(string conversionData)
    {
        DebugLog("onConversionDataSuccess -> conversionData = " + conversionData);
        Dictionary<string, object> conversionDataDictionary = AppsFlyer.CallbackStringToDictionary(conversionData);
        // add deferred deeplink logic here
    }

    public void onConversionDataFail(string error)
    {
        DebugLog("onConversionDataFail -> error = " + error);
    }

    public void onAppOpenAttribution(string attributionData)
    {
        DebugLog("onAppOpenAttribution -> attributionData = " + attributionData);

        Dictionary<string, object> attributionDataDictionary = AppsFlyer.CallbackStringToDictionary(attributionData);
        // add direct deeplink logic here
    }

    public void onAppOpenAttributionFailure(string error)
    {
        DebugLog("onAppOpenAttributionFailure -> error = " + error);
    }

    private void DebugLog(string log)
    {
#if ENABLE_DEBUG_LOG
        Debug.Log("TT_DEBUG:: AppsFlyerInit[v." + AppsFlyer.kAppsFlyerPluginVersion + "]:: " + log);
#endif
    }
#endif
}

