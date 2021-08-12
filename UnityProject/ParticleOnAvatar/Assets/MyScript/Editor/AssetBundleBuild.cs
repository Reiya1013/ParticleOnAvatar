using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using AssetBundleBuild = UnityEditor.AssetBundleBuild;
using System.Linq;

public class ParticleOnAvatar
{
    static string SavePass = "Assets/ParticleOnAvatar.prefab";

    [MenuItem("ParticleOnAvatar/Create File")]
    public static void Build()
    {
        var saveObject = GameObject.Find("ParticleOnAvatar");
        if (!saveObject)
        {
            EditorUtility.DisplayDialog("ParticleOnAvatar Objectがありません。\r\nルートの名前変えちゃいましたか？",
                                        "出力失敗", "OK");
            return;
        }
        // Prefabを新規作成・上書きする
        // ただし既にPrefab化されていてPrefabと違うパスを指定した場合はVariantを作る
        PrefabUtility.SaveAsPrefabAsset(saveObject, SavePass);

        //// Prefabを作成or上書きして紐づける
        //PrefabUtility.SaveAsPrefabAssetAndConnect(saveObject, "Assets/ParticleOnAvatar", InteractionMode.AutomatedAction);

        SaveAssetBundles();

    }


    private static void SaveAssetBundles()
    {
        var path = EditorUtility.SaveFilePanel("ParticleOnAvatar File Save", "", "", "particle");
        if (path == "") return;

        string[] vals = new string[1];
        vals[0] = SavePass;

        AssetBundleBuild[] buildMap = new AssetBundleBuild[1];
        buildMap[0].assetBundleName = Path.GetFileName(path);
        buildMap[0].assetNames = vals;

        string outputPath = $"{Application.temporaryCachePath}/{Path.GetRandomFileName()}";
        Directory.CreateDirectory(outputPath);
        if (BuildPipeline.BuildAssetBundles(outputPath, buildMap, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows))
        {
            //Assetbundle成功時Tempフォルダから必要ファイル移動後、フォルダ削除
            if (File.Exists(path)) File.Delete(path);
            File.Copy(outputPath + "/" + buildMap[0].assetBundleName, path);
            Directory.Delete(Application.temporaryCachePath, true);
        }

        UnityEditor.EditorUtility.DisplayDialog("ParticleOnAvatar", "ファイル出力完了", "OK");

    }
}
