using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class SOComicDialogues : ScriptableObject
{
    #if UNITY_EDITOR
    [MenuItem("SO/SOComicDialogues")]
    public static void QuickCreate()
    {
        SOComicDialogues asset = CreateInstance<SOComicDialogues>();
        string name =
            AssetDatabase.GenerateUniqueAssetPath("Assets/Scriptable Objects/Cutscene//ComicDialogues.asset");
        AssetDatabase.CreateAsset(asset, name);
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
    #endif

    public ComicDialoguesTitle dialoguesTitle;
    public List<Comic_Dialogue_Line> dialogue_Lines;

}
