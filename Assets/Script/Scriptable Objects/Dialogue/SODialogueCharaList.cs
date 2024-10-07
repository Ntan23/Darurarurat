using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DialogueCharacter
{
    public DialogueCharaName dialogueCharaName;
    public string name;
    public Color _textColor;
    public Sprite[] charaSprites;

}
public class SODialogueCharaList : ScriptableObject
{
   #if UNITY_EDITOR
    [MenuItem("SO/SODialogueCharaList")]
    public static void QuickCreate()
    {
        SODialogueCharaList asset = CreateInstance<SODialogueCharaList>();
        string name =
            AssetDatabase.GenerateUniqueAssetPath("Assets/ScriptableObjects/Cutscene//DialogueCharaList.asset");
        AssetDatabase.CreateAsset(asset, name);
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
    #endif

    public List<Dialogues> dialoguesList;

    public Dialogues SearchDialogues(DialoguesTitle title)
    {
        foreach(Dialogues dialogues in dialoguesList)
        {
            if(dialogues.dialoguesTitle == title)
            {
                return dialogues;
            }
        }
        return null;
    }
}
