using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;


public class SODialogueList : ScriptableObject
{
    #if UNITY_EDITOR
    [MenuItem("SO/SODialogueList")]
    public static void QuickCreate()
    {
        SODialogueList asset = CreateInstance<SODialogueList>();
        string name =
            AssetDatabase.GenerateUniqueAssetPath("Assets/Scriptable Objects/Cutscene//DialogueList.asset");
        AssetDatabase.CreateAsset(asset, name);
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
    #endif

    public List<SODialogues> dialoguesList;

    public SODialogues SearchDialogues(DialoguesTitle title)
    {
        foreach(SODialogues dialogues in dialoguesList)
        {
            if(dialogues.dialoguesTitle == title)
            {
                return dialogues;
            }
        }
        return null;
    }

}
