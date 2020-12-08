using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;

#if UNITY_IOS

public class AppsFlyerPostBuildProcess : MonoBehaviour
{
    // [PostProcessBuild]
    // public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
    // {
    //     if (buildTarget == BuildTarget.iOS) {
    //         BuildForiOS(path);
    //     }
    // }
    //
    // private static void BuildForiOS(string path)
    // {
    //     string projPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";
    //
    //     PBXProject proj = new PBXProject();
    //     var file = File.ReadAllText(projPath);
    //     proj.ReadFromString(file);
    //
    //     string target = proj.TargetGuidByName("Unity-iPhone");
    //     proj.AddFrameworkToProject(target, "Security.framework", false);
    //     proj.AddFrameworkToProject(target, "AdSupport.framework", false);
    //     proj.AddFrameworkToProject(target, "iAd.framework", false);
    //
    //     proj.SetBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", "$(SRCROOT)/Frameworks");
    //
    //     File.WriteAllText(projPath, proj.WriteToString());
    //     
    //     Debug.Log("AppsFlyer post build script completed succesfully.");
    // }
}
#endif