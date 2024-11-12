using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialSceneManager : MonoBehaviour
{
    GameManager gameManager;
    Chat_DialogueManager chat_DialogueManager;
    private List<ObjectControl> _objectControlList = new List<ObjectControl>();
    private List<bool> _isObjectAlreadyShowDialogue;
    private void Start() 
    {
        gameManager = GameManager.instance;
        chat_DialogueManager = Chat_DialogueManager.Instance;
        foreach(GameObject objectControl in gameManager.neededObjects)
        {
            ObjectControl obj = objectControl.GetComponent<ObjectControl>();
            if(obj != null)_objectControlList.Add(obj);
            obj.OnMouseEnterHoverItem += OnMouseInteractWithObject;
        }
        _isObjectAlreadyShowDialogue = new List<bool>(_objectControlList.Count);
        for(int i=0;i < _objectControlList.Count;i++)
        {
            _isObjectAlreadyShowDialogue.Add(false);
        }

    }

    private void OnMouseInteractWithObject(ObjectControl control)
    {
        if(gameManager.IsCutscene())return;
        int idx = _objectControlList.IndexOf(control);
        if(!_isObjectAlreadyShowDialogue[idx])
        {
            if(control.GetObjectIndex == gameManager.GetProcedureIndex())
            {
                _isObjectAlreadyShowDialogue[idx] = true;
                gameManager.ChangeCutsceneStatus(true);
                chat_DialogueManager.PlayDialogueScene(gameManager.GetMidTitles[idx]);
            }
        }
    }
}
