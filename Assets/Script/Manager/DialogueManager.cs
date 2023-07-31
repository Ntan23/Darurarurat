using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization;

public class DialogueManager : MonoBehaviour 
{
    #region Singleton
    public static DialogueManager instance;

    void Awake()
    {
        if(instance == null) instance = this;
    }
    #endregion

    [SerializeField] private bool isIntro;
    private bool isOpen = true;
    [SerializeField] private Dialogue[] dialogues;
    [SerializeField] private Image[] actorImage;
    [SerializeField] private Color inactiveColor;
    [SerializeField] private Color activeColor;
	[SerializeField] private TextMeshProUGUI actorNameText;
	[SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject dialogueBoard;
    [SerializeField] private GameObject dialogueContainer;
    [SerializeField] private Button nextButton;
    [SerializeField] private GameObject skipButtonGO;
    private Button skipButton;
    private TextMeshProUGUI skipButtonText;
    private TextMeshProUGUI nextButtonText;
    [SerializeField] private int cutDialogueIndex;
    private int dialogueIndex;
    private Queue<Dialogue> dialogueQueue = new Queue<Dialogue>
    ();
    private ScenesManager sm;
    private AudioManager am;

    void Start() 
    {
        sm = ScenesManager.instance;
        am = AudioManager.instance;

        nextButtonText = nextButton.GetComponent<TextMeshProUGUI>();
        nextButtonText.color = inactiveColor;

        skipButton = skipButtonGO.GetComponent<Button>();
        skipButtonText = skipButtonGO.GetComponent<TextMeshProUGUI>();

        foreach(Image img in actorImage) img.color = inactiveColor;

        DialogueIntialization();
        StartCoroutine(StartDialogueAnimation());
    }

    private void DialogueIntialization()
	{
        for(int i = 0; i < dialogues.Length; i++) dialogueQueue.Enqueue(dialogues[i]);
	}

	public void DisplayNextDialogue()
	{
        nextButton.interactable = false;
        nextButtonText.color = inactiveColor;

		if(dialogueQueue.Count == 0)
		{
            isOpen = false;
			EndDialogue();
            sm.GoToNextScene();
			return;
		}
        if(dialogueIndex == cutDialogueIndex)
        {
            isOpen = false;
            EndDialogue();
            dialogueIndex++;
            return;
        }
        
        Dialogue dialogue = dialogueQueue.Dequeue();

        if(isIntro && dialogueIndex == 0) SettingDialogue(dialogue);

        if(isIntro && dialogueIndex == 1)
        {
            actorNameText.text = null;
            dialogueText.text = null;
            LeanTween.scale(actorImage[0].gameObject, Vector3.one, 0.5f).setEaseSpring();
            LeanTween.scale(actorImage[1].gameObject, Vector3.one, 0.5f).setEaseSpring().setDelay(0.5f).setOnComplete(() => SettingDialogue(dialogue));
        }

        if(!isIntro || (isIntro && dialogueIndex > 1)) SettingDialogue(dialogue);
        
        dialogueIndex++;
	}

    void SettingDialogue(Dialogue dialogue)
    {
        if(dialogue.actorIndex < 2)
        {
            if(dialogueText.transform.localPosition.y != -23.0f) dialogueText.transform.localPosition = new Vector3(dialogueText.transform.localPosition.x, -23.0f, transform.localPosition.z);

            actorNameText.text = dialogue.actorName;

            actorImage[dialogue.actorIndex].sprite = dialogue.actorSprite;
            actorImage[dialogue.actorIndex].color = activeColor;
            LeanTween.scale(actorImage[dialogue.actorIndex].gameObject, new Vector3(1.2f, 1.2f, 1.2f), 0.5f).setEaseSpring();
        
            if(dialogue.actorIndex == 0) 
            {
                actorImage[dialogue.actorIndex + 1].color = inactiveColor;
                LeanTween.scale(actorImage[dialogue.actorIndex + 1].gameObject, Vector3.one, 0.5f).setEaseSpring();
            }
            else if(dialogue.actorIndex == 1) 
            {
                actorImage[dialogue.actorIndex - 1].color = inactiveColor;
                LeanTween.scale(actorImage[dialogue.actorIndex - 1].gameObject, Vector3.one, 0.5f).setEaseSpring();
            }
        }
        
        if(dialogue.actorIndex >= 2) 
        {
            dialogueText.transform.localPosition = new Vector3(dialogueText.transform.localPosition.x, 43.0f, transform.localPosition.z);

            actorNameText.text = null;

            if(!isIntro)
            {
                foreach(Image img in actorImage) 
                {
                    LeanTween.scale(img.gameObject, Vector3.one, 0.5f).setEaseSpring();
                    img.color = inactiveColor;
                }
            }
        }
        
        //StartCoroutine(TypeSentence(dialogue.sentences, dialogue));
        StartCoroutine(TypeSentence(dialogue.dialogue.GetLocalizedString(), dialogue));
    }

	IEnumerator TypeSentence (string sentence, Dialogue dialogue)
	{
        am.PlayWritingSFX();
        if(dialogue.audioActionName != null) am.PlayDialogueActionSFX(dialogue.audioActionName);

		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return new WaitForSeconds(0.02f);
		}

        nextButton.interactable = true;
        nextButtonText.color = Color.black;
        am.StopWritingSFX();
	}

    public void ShowEndDialogue() => StartCoroutine(ShowEndDialogueAnimation());
    public void MoveStartBoard() => StartCoroutine(StartDialogueAnimation());
    public void SkipDialogue()
    {
        StopAllCoroutines();
        am.StopAllSFX();
        
        if(dialogueIndex <= cutDialogueIndex) 
        {
            isOpen = false;

            for(int i = dialogueIndex; i < cutDialogueIndex; i++) dialogueQueue.Dequeue();

            dialogueIndex = cutDialogueIndex + 1;

            EndDialogue();
            return;
        }
        
        if(dialogueIndex > cutDialogueIndex) sm.GoToNextScene();
    }

	void EndDialogue() => LeanTween.moveLocalX(dialogueBoard, 1924.0f, 0.8f).setEaseSpring().setOnComplete(() => CloseDialogueUI());
    
    void CloseDialogueUI()
    {
        actorNameText.text = null;
        dialogueText.text = null;
        dialogueContainer.transform.localScale = Vector3.zero;
        
        foreach(Image img in actorImage) 
        {
            img.gameObject.transform.localScale = Vector3.zero;
            img.color = inactiveColor;
        }

    }

    public void DeactivateSkipButton() => skipButtonGO.SetActive(false);

    public void DisableSkipButton() 
    {
        skipButton.interactable = false;
        skipButtonText.color = inactiveColor;
    }

    public void EnableSkipButton()
    {
        skipButton.interactable = true;
        skipButtonText.color = Color.black;
    }

    IEnumerator ShowEndDialogueAnimation()
    {
        isOpen = true;
        LeanTween.moveLocalX(dialogueBoard, 0.0f, 0.8f).setEaseSpring();
        yield return new WaitForSeconds(0.6f);
        LeanTween.scale(dialogueContainer, Vector3.one, 0.5f).setEaseSpring();
        LeanTween.scale(actorImage[0].gameObject, Vector3.one, 0.5f).setEaseSpring().setDelay(0.5f);
        LeanTween.scale(actorImage[1].gameObject, Vector3.one, 0.5f).setEaseSpring().setDelay(1.0f).setOnComplete(() => DisplayNextDialogue());
    }

    IEnumerator StartDialogueAnimation()
    {
        yield return new WaitForSeconds(0.6f);
        if(isIntro) 
        {
            dialogueText.transform.localPosition = new Vector3(dialogueText.transform.localPosition.x, 43.0f, transform.localPosition.z);
            LeanTween.scale(dialogueContainer, Vector3.one, 0.5f).setEaseSpring().setOnComplete(() => DisplayNextDialogue());
        }
        else if(!isIntro)
        {
            LeanTween.scale(dialogueContainer, Vector3.one, 0.5f).setEaseSpring();
            LeanTween.scale(actorImage[0].gameObject, Vector3.one, 0.5f).setEaseSpring().setDelay(0.5f);
            LeanTween.scale(actorImage[1].gameObject, Vector3.one, 0.5f).setEaseSpring().setDelay(1.0f).setOnComplete(() => DisplayNextDialogue());
        }
    }

    public bool IsOpen()
    {
        return isOpen;
    }
}
