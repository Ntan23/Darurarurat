using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;


[Serializable]
public class Dialogue_Line
{
    
    [Space(1)]
    [SerializeField]private float _delayTypeText = 0.1f;
    [SerializeField]private float _delayBetweenLines = 0.2f;
    //Tambah Sound effect kalo mau
    
    [Header("Kalau mau ada sfx di tengah-tengah audio tulisnya->  <sfx:namaSFX>")]
    [SerializeField]private LocalizedString _dialogueTextLocalized;

    public float DelayTypeText { get { return _delayTypeText; }}
    public float DelayBetweenLines { get { return _delayBetweenLines;}}
    public string DialogueTextLocalized { get { return _dialogueTextLocalized.GetLocalizedString();}}

}
[Serializable]
public class Chat_Dialogue_Line : Dialogue_Line, INeedChatDialogue
{
    [Space(1)]
    [SerializeField]private DialogueType _dialogueType;
    [SerializeField]private DialogueCharaName _charaName;

    [Space(1)]
    [Header("Sesuaikan dgn banyak sprite character | kalo gamau ada sprite pake saat ini tulis angka >= length sprite asli")]
    [SerializeField]private int _spriteNumber;

    public DialogueType DialogueTypeNow { get { return _dialogueType;}}

    public DialogueCharaName CharaName {get {return _charaName;}}

    public int SpriteNumber {get {return _spriteNumber;}}
}
[Serializable]
public class Comic_Dialogue_Line : Dialogue_Line, INeedVA
{
    [Space(5)]
    [Header("If you dont want va, empty it")]
    [SerializeField]private string _vaName;

    public string VAname {get {return _vaName;}}
}