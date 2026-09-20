// SlayinBuild.cs — batch-mode Android build for the Slayin rebuild (2026-09-18).
// Run: Unity.exe -batchmode -nographics -projectPath <proj> -executeMethod SlayinBuild.Android -logFile <log> -quit
using UnityEditor;
using UnityEngine;
using System.Linq;

public static class SlayinBuild
{
    static void Tools()
    {
        EditorPrefs.SetBool("SdkUseEmbedded", false); EditorPrefs.SetString("AndroidSdkRoot", @"C:\Users\jonal\AndroidUnity\SDK");
        EditorPrefs.SetBool("NdkUseEmbedded", false); EditorPrefs.SetString("AndroidNdkRootR23", @"C:\Users\jonal\AndroidUnity\NDK"); EditorPrefs.SetString("AndroidNdkRoot", @"C:\Users\jonal\AndroidUnity\NDK");
        EditorPrefs.SetBool("JdkUseEmbedded", false); EditorPrefs.SetString("JdkPath", @"C:\Users\jonal\AndroidUnity\OpenJDK");
        EditorPrefs.SetBool("GradleUseEmbedded", true);
        // Unity 2022.3: the build pipeline reads these, not the raw EditorPrefs ("JDK not found" otherwise)
        UnityEditor.Android.AndroidExternalToolsSettings.jdkRootPath = @"C:\Users\jonal\AndroidUnity\OpenJDK";
        UnityEditor.Android.AndroidExternalToolsSettings.sdkRootPath = @"C:\Users\jonal\AndroidUnity\SDK";
        UnityEditor.Android.AndroidExternalToolsSettings.ndkRootPath = @"C:\Users\jonal\AndroidUnity\NDK";
        Debug.Log("SlayinBuild: tools jdk=" + UnityEditor.Android.AndroidExternalToolsSettings.jdkRootPath + " sdk=" + UnityEditor.Android.AndroidExternalToolsSettings.sdkRootPath + " ndk=" + UnityEditor.Android.AndroidExternalToolsSettings.ndkRootPath + " gradle=" + UnityEditor.Android.AndroidExternalToolsSettings.gradlePath);
    }

    // Variants (owner 2026-09-19): "Slayin64" = the faithful game; "Slayin64 Unlimited" = unlimited fame
    // points + everything unlocked (scripting define SLAYIN_CHEAT). Pick with env SLAYIN_VARIANT=unlimited.
    static bool Cheat => System.Environment.GetEnvironmentVariable("SLAYIN_VARIANT") == "unlimited";

    public static void Configure()
    {
        Tools();
        // Owner 2026-09-19: the unlimited build keeps the SAME package + title ("Slayin64") - the phone only
        // ever carries the unlimited one; the faithful build lives on the laptop as Slayin64-arm64.apk.
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "com.firestarter1996.slayin64");
        PlayerSettings.productName = "Slayin64";
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, Cheat ? "SLAYIN_CHEAT" : "");
        // original launcher icon, pulled from the 2016 APK (res/drawable-xxxhdpi/app_icon.png)
        var icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Icon/app_icon.png");
        if (icon != null) PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Unknown, new[] { icon });
        Debug.Log("SlayinBuild: variant=" + (Cheat ? "unlimited" : "normal") + " icon=" + (icon != null));
        PlayerSettings.bundleVersion = "2.0.13";
        PlayerSettings.Android.bundleVersionCode = 101;   // original APK was versionCode 101; Gradle refuses 0
        PlayerSettings.companyName = "firestarter1996";
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel24;
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;
        PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Android, ApiCompatibilityLevel.NET_Unity_4_8);
        PlayerSettings.Android.forceSDCardPermission = false;
        Debug.Log("SlayinBuild: configured");
    }

    public static void Android()
    {
        Configure();
        var scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
        Debug.Log("SlayinBuild: scenes = " + string.Join(", ", scenes));
        var opts = new BuildPlayerOptions { scenes = scenes, locationPathName = Cheat ? @"C:\Users\jonal\Downloads\SlayinPort\Build\Slayin64-Unlimited-arm64.apk" : @"C:\Users\jonal\Downloads\SlayinPort\Build\Slayin64-arm64.apk", target = BuildTarget.Android, options = BuildOptions.None };
        var report = BuildPipeline.BuildPlayer(opts);
        Debug.Log("SlayinBuild: result = " + report.summary.result + " errors=" + report.summary.totalErrors + " size=" + report.summary.totalSize);
        if (report.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded) EditorApplication.Exit(2);
    }
}
