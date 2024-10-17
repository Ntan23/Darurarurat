using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComicManager : MonoBehaviour
{
    [Header("Testing")]
    public bool isGO;
    public bool isStop;
    public ComicDialoguesTitle title;


    public static ComicManager Instance { get; private set;}
    [SerializeField]private List<ComicPageController> _comicList;
    private ComicPageController _currComic;
    public Action<ComicDialoguesTitle> OnComicFinished;

    private void Awake() 
    {
        ComicPageController[] comicList = GetComponentsInChildren<ComicPageController>();
        _comicList = comicList.ToList();
        if(Instance == null)
        {
            Instance = this;
        }
    }
    private void Update() {
        if(isGO)
        {
            // Debug.Log("????");
            isGO = false;
            PlayComic(title);
        }
        if(isStop)
        {
            isStop = false;
            StopCurrComic();
        }
    }
    public void PlayComic(ComicDialoguesTitle title)
    {
        foreach(ComicPageController comic in _comicList)
        {
            if(comic.ComicTitle == title)
            {
                _currComic = comic;
                _currComic.ShowComic();
                break;
            }
        }
        
    }
    public void PlayComicUsingString(string title)
    {
        foreach(ComicPageController comic in _comicList)
        {
            if(comic.ComicTitleInString == title)
            {
                _currComic = comic;
                _currComic.ShowComic();
                break;
            }
        }

    }

    public void StopCurrComic()
    {
        if(_currComic != null)
        {
            _currComic.HideComic();
            _currComic = null;
        }
    }

}
