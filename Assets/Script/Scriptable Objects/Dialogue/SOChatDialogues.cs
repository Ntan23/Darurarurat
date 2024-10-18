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

    public ChatDialoguesTitle dialoguesTitle;
    [Tooltip("Kalau Pake Sprite, centang; kalo ga gausa - ini buat semuanya")]
    public bool isWholeDialogueUseSprite;
    public List<Chat_Dialogue_Line> dialogue_Lines;
}
