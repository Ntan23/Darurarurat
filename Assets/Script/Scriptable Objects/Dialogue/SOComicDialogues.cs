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

    [Header("Reminder - don't need to put it in DialogueList")]
    public DialoguesTitle dialoguesTitle;
    [Tooltip("Kalau Pake VA, VA bakal dijalanin; Kalau Gapake VA, ketik ketik sound")]
    public bool isUseVA;
    [Tooltip("Kalau Comic Style gapake sprite")]
    public List<Comic_Dialogue_Line> dialogue_Lines;

}
