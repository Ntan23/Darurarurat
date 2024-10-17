using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;


public class SOChatDialogueList : ScriptableObject
{
    #if UNITY_EDITOR
    [MenuItem("SO/SODialogueList")]
    public static void QuickCreate()
    {
        SOChatDialogueList asset = CreateInstance<SOChatDialogueList>();
        string name =
            AssetDatabase.GenerateUniqueAssetPath("Assets/Scriptable Objects/Cutscene//ChatDialogueList.asset");
        AssetDatabase.CreateAsset(asset, name);
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
    #endif

    public List<SOChatDialogues> dialoguesList;

    public SOChatDialogues SearchDialogues(ChatDialoguesTitle title)
    {
        foreach(SOChatDialogues dialogues in dialoguesList)
        {
            if(dialogues.dialoguesTitle == title)
            {
                return dialogues;
            }
        }
        return null;
    }
    public SOChatDialogues SearchDialoguesUsingString(string title)
    {
        foreach(SOChatDialogues dialogues in dialoguesList)
        {
            if(dialogues.dialoguesTitle.ToString() == title)
            {
                return dialogues;
            }
        }
        return null;
    }

}
