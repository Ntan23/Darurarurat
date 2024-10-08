using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;


public class Dialogue_Line
{
    public DialogueType dialogueType;
    public DialogueCharaName charaName;
    [Tooltip("Sesuaikan dgn banyak sprite character")]
    public int spriteNumber;
    [Space(1)]
    public float delayTypeText = 0.1f;
    public float delayBetweenLines = 0.2f;
    //Tambah Sound effect kalo mau
    [Space(1)]
    public string VAname;
    [TextArea(3, 7)]public string dialogueText;


}
public class Dialogues
{
    public DialoguesTitle dialoguesTitle;
    [Tooltip("Kalau Pake VA, VA bakal dijalanin; Kalau Gapake VA, ketik ketik sound")]
    public bool isUseVA;
    [Tooltip("Kalau Comic Style gapake sprite")]
    public bool isUseSprite;
    public List<Dialogue_Line> dialogue_Lines;
}
public class SODialogueList : ScriptableObject
{
    #if UNITY_EDITOR
    [MenuItem("SO/DialogueList")]
    public static void QuickCreate()
    {
        SODialogueList asset = CreateInstance<SODialogueList>();
        string name =
            AssetDatabase.GenerateUniqueAssetPath("Assets/ScriptableObjects/Cutscene//DialogueList.asset");
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
