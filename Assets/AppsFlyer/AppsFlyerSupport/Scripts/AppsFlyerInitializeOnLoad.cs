#if UNITY_EDITOR

using System;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class AppsFlyerInitializeOnLoad : EditorWindow
{
    private const string ANDROID_NAME			                 = "android:name";
    private const string ANDROID_MANIFEST_XML 	                 = "AndroidManifest.xml";
    private const string PLUGINS_RELPATH 		                 = "Plugins";
    private const string ANDROID_RELPATH		                 = "Android";
    private const string DEFAULT_ANDROID_MANIFEST_PATH		     = "AppsFlyer/AppsFlyerSupport/DefaultAndroidManifest.xml";
    private const string USES_PERMISSION		                 = "uses-permission";
    private const string ANDROID_PERMISSION_INTERNET		     = "android.permission.INTERNET";
    private const string ANDROID_PERMISSION_ACCESS_NETWORK_STATE = "android.permission.ACCESS_NETWORK_STATE";
    private const string ANDROID_PERMISSION_ACCESS_WIFI_STATE    = "android.permission.ACCESS_WIFI_STATE";

    private static bool hasPermission_INTERNET = false;
    private static bool hasPermission_ACCESS_NETWORK_STATE = false;
    private static bool hasPermission_ACCESS_WIFI_STATE = false;
    private static bool shouldReSaveAndroidManifest = false;
    
    static AppsFlyerInitializeOnLoad()
    {
        var editorBuildScenes = EditorBuildSettings.scenes;
        if (editorBuildScenes == null || editorBuildScenes.Length == 0)
        {
            Debug.LogError("No scenes defined in Build settings. Please add needed scenes in build settings so Appsflyer can initialize");
            return;
        }
        
        var isAppsFlyerInited = EditorPrefs.GetBool("AppsFlyerInit", false);
        if (!isAppsFlyerInited)
        {
            GetWindow(typeof(AppsFlyerAppIdWindow));
        }
         
#if UNITY_ANDROID
        UpdateAndroidManifest();
#endif
    }

    [MenuItem("AppsFlyer Tools/Update Android Manifest")]
    public static void UpdateAndroidManifest() {
        // Debug.Log("DEBUG:: UpdateAndroidManifest ");
        // string androidManifestFile = Path.Combine(Application.dataPath, PLUGINS_RELPATH);
        // androidManifestFile = Path.Combine(androidManifestFile, ANDROID_RELPATH);
        // androidManifestFile = Path.Combine(androidManifestFile, ANDROID_MANIFEST_XML);
        // Debug.Log("DEBUG:: UpdateAndroidManifest -> androidManifestFile = " + androidManifestFile);

        string pluginsAndroidFolder = Path.Combine (Application.dataPath, PLUGINS_RELPATH);
        if (!System.IO.Directory.Exists(pluginsAndroidFolder))
            System.IO.Directory.CreateDirectory (pluginsAndroidFolder);
        
        pluginsAndroidFolder = Path.Combine (pluginsAndroidFolder, ANDROID_RELPATH);
        if (!System.IO.Directory.Exists(pluginsAndroidFolder))
            System.IO.Directory.CreateDirectory (pluginsAndroidFolder);

        var outputFile = Path.Combine (pluginsAndroidFolder,ANDROID_MANIFEST_XML);
        // Debug.Log("DEBUG:: UpdateAndroidManifest -> outputFile = " + outputFile);

        if (!File.Exists(outputFile))
            GenerateDefaultManifest(outputFile);
        else 
            EditManifest(outputFile);
    }
    
    public static void GenerateDefaultManifest(string outputFile)
    {
        Debug.Log("DEBUG:: ---------------------------- GenerateDefaultManifest ----------------------------");
        var inputFile = Path.Combine(Application.dataPath, DEFAULT_ANDROID_MANIFEST_PATH);
        Debug.Log("DEBUG:: GenerateDefaultManifest... Copy default AndroidManifest from \n " +
                       "inputFile = " + inputFile + "\n" +
                       "outputFile = " + outputFile);
        try
        {
            File.Copy(inputFile, outputFile);
        }
        catch (Exception ex)
        {
            Debug.LogError("DEBUG:: AppsFlyerInitializeOnLoad: GenerateDefaultManifest: failed AndroidManifest.xml copying... exception = " + ex);
            
        }
    }

    static void EditManifest(string androidManifestFilePath, bool withGradle = false)
    {
        //Debug.Log("DEBUG:: ---------------------------- EditManifest ----------------------------"); 
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(androidManifestFilePath);
        XmlNode mainManifestNode = FindChildNode(xmlDoc, "manifest");
        // Debug.LogError("DEBUG:: EditManifest: mainManifestNode.OuterXml = " + mainManifestNode.OuterXml);
        // Debug.LogError("DEBUG:: EditManifest: mainManifestNode.mainManifestNode.ChildNodes.Count = " + mainManifestNode.ChildNodes.Count);
        // Debug.LogError("DEBUG:: EditManifest: mainManifestNode.mainManifestNode.HasChildNodes = " + mainManifestNode.HasChildNodes);

        if (mainManifestNode == null)
        {
            Debug.LogError("DEBUG:: AppsFlyerInitializeOnLoad: EditManifest: AndroidManifest exists but has wrong structure...");
            return;
        }

        foreach (XmlNode childNode in mainManifestNode.ChildNodes)
        {
            if (!childNode.Name.Equals(USES_PERMISSION))
                continue;
            
            for (int i = 0; i <  childNode.Attributes.Count; i++)
            {
                // Debug.Log("DEBUG:: EditManifest: mainManifestNode -> childNode.Name = " + childNode.Name + " -> " +
                //                ", childNode.Attributes.ANDROID_NAME.Name = " + childNode.Attributes.GetNamedItem(ANDROID_NAME).Value);
                switch (childNode.Attributes.GetNamedItem(ANDROID_NAME).Value) {
                    case ANDROID_PERMISSION_INTERNET:
                        hasPermission_INTERNET = true;
                        break;
                    case ANDROID_PERMISSION_ACCESS_NETWORK_STATE:
                        hasPermission_ACCESS_NETWORK_STATE = true;
                        break;
                    case ANDROID_PERMISSION_ACCESS_WIFI_STATE:
                        hasPermission_ACCESS_WIFI_STATE = true;
                        break;
                    default:
                        break;
                }
            }
        }
         
        if (!hasPermission_INTERNET)
        {
            Debug.Log("DEBUG:: AppsFlyerInitializeOnLoad: EditManifest: ANDROID_PERMISSION_INTERNET is missing !!!!!!!!!!");
            AddPermissionInAndroidManifest(mainManifestNode, ANDROID_PERMISSION_INTERNET);
        }
        if (!hasPermission_ACCESS_NETWORK_STATE)
        {
            Debug.Log("DEBUG:: AppsFlyerInitializeOnLoad: EditManifest: ANDROID_PERMISSION_ACCESS_NETWORK_STATE is missing !!!!!!!!!!");
            AddPermissionInAndroidManifest(mainManifestNode, ANDROID_PERMISSION_ACCESS_NETWORK_STATE);
        }
        if (!hasPermission_ACCESS_WIFI_STATE) 
        { 
            Debug.Log("DEBUG:: AppsFlyerInitializeOnLoad: EditManifest: ANDROID_PERMISSION_ACCESS_WIFI_STATE is missing !!!!!!!!!!");
            AddPermissionInAndroidManifest(mainManifestNode, ANDROID_PERMISSION_ACCESS_WIFI_STATE);
        }
        if(shouldReSaveAndroidManifest)
            xmlDoc.Save(androidManifestFilePath);
    }
    
    private static XmlNode FindChildNode(XmlNode parent, string name)
    {
        XmlNode curr = parent.FirstChild;
        while (curr != null)
        {
            if (curr.Name.Equals(name))
            {
                return curr;
            }
            curr = curr.NextSibling;
        }
        return null;
    }

    private static void AddPermissionInAndroidManifest(XmlNode xmlNode, string permission)
    {
        Debug.Log("DEBUG:: AppsFlyerInitializeOnLoad: AddPermissionInAndroidManifest: permission = " + permission);
        XmlElement xmlElem = xmlNode.OwnerDocument.CreateElement(USES_PERMISSION);
        xmlElem.SetAttribute("name","http://schemas.android.com/apk/res/android",permission);
        xmlNode.AppendChild(xmlElem);
        shouldReSaveAndroidManifest = true;
    }
}
#endif