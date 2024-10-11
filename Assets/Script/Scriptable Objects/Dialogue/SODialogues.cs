using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;


[Serializable]
public class Dialogue_Line
{
    public DialogueType dialogueType;
    public DialogueCharaName charaName;
    [Tooltip("Sesuaikan dgn banyak sprite character - kalo gapake tulis angka >= length sprite asli")]
    public int spriteNumber;
    [Space(1)]
    public float delayTypeText = 0.1f;
    public float delayBetweenLines = 0.2f;
    //Tambah Sound effect kalo mau
    [Space(1)]
    public string VAname;
    [TextArea(3, 7)]public string dialogueText;


}

public class SODialogues : ScriptableObject
{
    #if UNITY_EDITOR
    [MenuItem("SO/SODialogues")]
    public static void QuickCreate()
    {
        SODialogues asset = CreateInstance<SODialogues>();
        string name =
            AssetDatabase.GenerateUniqueAssetPath("Assets/Scriptable Objects/Cutscene//Dialogues.asset");
        AssetDatabase.CreateAsset(asset, name);
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
    #endif

    public DialoguesTitle dialoguesTitle;
    [Tooltip("Kalau Pake VA, VA bakal dijalanin; Kalau Gapake VA, ketik ketik sound")]
    public bool isUseVA;
    [Tooltip("Kalau Comic Style gapake sprite")]
    public bool isUseSprite;
    public List<Dialogue_Line> dialogue_Lines;

}
