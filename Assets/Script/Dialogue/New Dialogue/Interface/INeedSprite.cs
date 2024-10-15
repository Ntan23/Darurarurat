using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INeedChatDialogue
{
    DialogueCharaName CharaName { get; }
    int SpriteNumber { get; }
    public DialogueType DialogueTypeNow { get;}
 }
