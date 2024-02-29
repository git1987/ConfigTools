using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;

/// <summary>
/// 编辑器内使用build unity asset的editor 类
/// </summary>
public class CreateConfig
{
    static string assetPath = $"{Application.dataPath}/ConfigAsset/";
    [MenuItem("CreateAsset/AssetBundle")]
    static void BuildAssetBundle()
    {
    	string configPath = Application.streamingAssetsPath + "/Config";
        if (Directory.Exists(configPath))
        {
            Directory.Delete(configPath, true);
            Directory.CreateDirectory(configPath);
        }
        //打包资源 
        AssetBundleManifest manifest;
        DirectoryInfo configAssetFolder = new DirectoryInfo(assetPath);
        FileInfo[] files = configAssetFolder.GetFiles("*.bytes");
        AssetBundleBuild[] abb = new AssetBundleBuild[1];
        abb[0] = new AssetBundleBuild()
        {
            assetBundleName = "config",
            assetNames = new string[files.Length],
        };
        int index = 0;
        foreach (FileInfo file in files)
        {
            string unityAssetPath = "Assets/ConfigAsset/" + file.Name;
            Object obj = AssetDatabase.LoadAssetAtPath<Object>(unityAssetPath);
            Debug.Log(obj);
            abb[0].assetNames[index] = unityAssetPath;
            index++;
        }
#if UNITY_ANDROID
        string targetPath = Application.streamingAssetsPath + "/Config/Android/";
#elif UNITY_IOS
        string targetPath = Application.streamingAssetsPath + "/Config/IOS/";
#elif UNITY_STANDALONE_WIN
        string targetPath = Application.streamingAssetsPath + "/Config/Windows/";
#elif UNITY_WEBGL
        string targetPath = Application.streamingAssetsPath + "/Config/WebGL/";
#else
        string targetPath = Application.streamingAssetsPath + "/Config/Other/";
#endif
#if UNITY_ANDROID
        //移动端压缩ab
        manifest = BuildPipeline.BuildAssetBundles(targetPath, abb, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);
#elif UNITY_IOS
        //移动端压缩ab
        manifest = BuildPipeline.BuildAssetBundles(targetPath, abb, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.iOS);
#elif UNITY_STANDALONE_WIN
        //PC端不压缩ab
        manifest = BuildPipeline.BuildAssetBundles(targetPath, abb, BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.StandaloneWindows);
#elif UNITY_WEBGL
        //WebGL端压缩ab
        manifest = BuildPipeline.BuildAssetBundles(targetPath, abb, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.WebGL);
#endif
        Debug.Log("配置打包配置ab完成");
        //刷新编辑器 
        AssetDatabase.Refresh();
    }
    //判断文件夹是否存在
    private static bool IsFolderExists(string folderPath)
    {
        if (folderPath.Equals(string.Empty))
        {
            return false;
        }
        return Directory.Exists(folderPath);
    }
}