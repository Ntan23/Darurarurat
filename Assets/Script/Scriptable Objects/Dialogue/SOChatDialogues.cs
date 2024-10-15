using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SOChatDialogues : ScriptableObject
{
    #if UNITY_EDITOR
    [MenuItem("SO/SOChatDialogues")]
    public static void QuickCreate()
    {
        SOChatDialogues asset = CreateInstance<SOChatDialogues>();
        string name =
            AssetDatabase.GenerateUniqueAssetPath("Assets/Scriptable Objects/Cutscene//ChatDialogues.asset");
        AssetDatabase.CreateAsset(asset, name);
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
    #endif

    public DialoguesTitle dialoguesTitle;
    [Tooltip("Kalau Pake VA, VA bakal dijalanin; Kalau Gapake VA, ketik ketik sound")]
    public bool isUseSprite;
    public List<Chat_Dialogue_Line> dialogue_Lines;
}
