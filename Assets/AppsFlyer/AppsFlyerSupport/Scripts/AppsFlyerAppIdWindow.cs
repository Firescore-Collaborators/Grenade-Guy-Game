#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class AppsFlyerAppIdWindow : EditorWindow
{
    private string appsFlyerAppId = string.Empty;
    private string appsFlyerDevKey = string.Empty;
    private bool isAFAppIdSet;
    private bool isAFDevKeySet;
    private bool isWarningWindowActive;

    private void OnGUI()
    {
        GUILayout.Label("Set appsflyer AppID:", EditorStyles.boldLabel);
        appsFlyerAppId = GUILayout.TextField(appsFlyerAppId);
        
        GUILayout.Label("Set appsflyer DevKey:", EditorStyles.boldLabel);
        appsFlyerDevKey = GUILayout.TextField(appsFlyerDevKey);
        
        FillAppsFlyerAppIdLabelWithOldValue();

        if (isWarningWindowActive && IsKeyPressed(KeyCode.Return))
        {
            isWarningWindowActive = false;
            return;
        }
        
        if (GUILayout.Button("Done") || IsKeyPressed(KeyCode.Return))
        {
            isWarningWindowActive = false;
            
            HandleDoneButtonPress();
        }
        else if (IsKeyPressed(KeyCode.Escape))
        {
            EditorPrefs.SetBool("AppsFlyerInit", true);
            
            Close();
        }
    }
    
    private void FillAppsFlyerAppIdLabelWithOldValue()
    {
        if (string.IsNullOrEmpty(appsFlyerAppId) && !isAFAppIdSet)
        {
            var afInScene = FindObjectOfType<AppsFlyerInit>();
            if (afInScene != null)
            {
                appsFlyerAppId = afInScene.appId;
                isAFAppIdSet = true;
            }
        }
        
        if (string.IsNullOrEmpty(appsFlyerDevKey) && !isAFDevKeySet)
        {
            var afInScene = FindObjectOfType<AppsFlyerInit>();
            if (afInScene != null)
            {
                appsFlyerDevKey = afInScene.devKey;
                isAFAppIdSet = true;
            }
        }
    }

    private void HandleDoneButtonPress()
    {
        if (!TryValidateInput())
            return;
            
        var editorBuildScenes = EditorBuildSettings.scenes;
        var firstScene = editorBuildScenes[0];
        if (EditorSceneManager.GetActiveScene().path != firstScene.path)
        {
            EditorSceneManager.OpenScene(EditorBuildSettings.scenes[0].path);
        }

        var appsflyerPrefabPath = FindAppsflyerPrefabPath();
        
        GameObject appsFlyerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(appsflyerPrefabPath);
        var appsFlyerInit = appsFlyerPrefab.GetComponent<AppsFlyerInit>();
        appsFlyerInit.appId = appsFlyerAppId.Trim();
        appsFlyerInit.devKey = appsFlyerDevKey.Trim();

        var afInScene = FindObjectOfType<AppsFlyerInit>();
        if (afInScene != null)
            DestroyImmediate(afInScene.gameObject);
            
        PrefabUtility.InstantiatePrefab(appsFlyerPrefab);

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveOpenScenes();
                
        EditorPrefs.SetBool("AppsFlyerInit", true);
        
        Close();
    }

    private bool TryValidateInput()
    {
        #if UNITY_IOS
        if (string.IsNullOrEmpty(appsFlyerAppId))
        {
            isWarningWindowActive = true;
            EditorUtility.DisplayDialog("Input error",
                "You didn't enter app id, please enter valid number for the app id", "OK");
            return false;
        }
        var isNumeric = int.TryParse(appsFlyerAppId, out _);
        if (!isNumeric)
        {
            isWarningWindowActive = true;
            EditorUtility.DisplayDialog("Input error",
                "The number that you entered is not valid, please enter valid number", "OK");
            return false;
        }
        #endif
        
        if (string.IsNullOrEmpty(appsFlyerDevKey))
        {
            isWarningWindowActive = true;
            EditorUtility.DisplayDialog("Input error",
                "You didn't enter devKey, please enter valid string for the devKey", "OK");
            return false;
        }
        return true;
    }

    private bool IsKeyPressed(KeyCode keyCode)
    {
        return Event.current.keyCode == keyCode && Event.current.type == EventType.KeyUp;
    }

    private string FindAppsflyerPrefabPath()
    {
        var assets = AssetDatabase.FindAssets("AppsFlyer");

        foreach (var guidAsset in assets)
        {
            var asset = AssetDatabase.GUIDToAssetPath(guidAsset);
            if (asset.EndsWith("CLAppsFlyer.prefab"))
            {
                return asset;
            }
        }

        return null;
    }
}

#endif