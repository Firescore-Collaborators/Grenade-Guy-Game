#if UNITY_EDITOR
using UnityEditor;

public class AfMenuItems : EditorWindow
{
    [MenuItem("AppsFlyer Tools/Add appsflyer in first scene")]
    static void AddAppsFlyerInScene()
    {
        GetWindow(typeof(AppsFlyerAppIdWindow));
    }
    
    [MenuItem("AppsFlyer Tools/Change appsflyer app id")]
    static void ChangeAppsFlyerAppId()
    {
        GetWindow(typeof(AppsFlyerAppIdWindow));
    }
    
    [MenuItem("AppsFlyer Tools/Reset EditorPrefs appsflyer key")]
    static void ResetEditorPrefs()
    {
        EditorPrefs.DeleteKey("AppsFlyerInit");
    }
    
}
#endif
