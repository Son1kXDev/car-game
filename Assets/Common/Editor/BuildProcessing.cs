using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System;
using System.IO;
using UnityEngine;

public class BuildProcessing : IPreprocessBuildWithReport, IPostprocessBuildWithReport
{
    public int callbackOrder
    {
        get { return 0; }
    }

    [MenuItem("Tools/Quick Build")]
    private static void QuickBuildMenuItem()
    {
        string buildPath = Path.Combine(Application.dataPath, "../Builds/");
        if (Directory.Exists(buildPath))
            Directory.CreateDirectory(buildPath);

        string exePath = Path.Combine(buildPath, Application.productName + ".exe");

        BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, exePath, EditorUserBuildSettings.activeBuildTarget, BuildOptions.None);
    }

    public void OnPreprocessBuild(BuildReport report)
    {
        if (report.summary.options == BuildOptions.Development) return;

        IncrementBuildVersion();
    }

    public void OnPostprocessBuild(BuildReport report)
    {
        if (report.summary.result == BuildResult.Failed) return;

        ExportVersionFile(report.summary.outputPath);
    }

    private void IncrementBuildVersion()
    {
        Version version;
        if (Version.TryParse(PlayerSettings.bundleVersion, out version))
        {
            version = new Version(version.Major, version.Minor, version.Build + 1);
            PlayerSettings.bundleVersion = version.ToString();
        }
    }

    private void ExportVersionFile(string outputPath)
    {
        string buildPath = new FileInfo(outputPath).Directory.FullName;
        string versionPath = Path.Combine(buildPath, "Version.txt");

        File.WriteAllText(versionPath, PlayerSettings.bundleVersion);
    }
}