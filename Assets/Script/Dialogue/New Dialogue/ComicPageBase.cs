using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComicPageBase : MonoBehaviour
{
    public Image _comicBG;
    public GameObject _dialogBoxParent;
    public List<GameObject> _dialogBoxList;
    public List<TMP_Text> _textDialogContainerList;
    public void MovePosX(float newPosX, float duration)
    {
        LeanTween.moveLocalX(_comicBG.gameObject, newPosX, duration);
    }
    public void ResetPos(float newPosX)
    {
        Vector3 newPos = new Vector3(newPosX, _comicBG.transform.position.y, _comicBG.transform.position.z);
        _comicBG.transform.position = newPos;
    }
    // public void HideAllDialogue()
}
